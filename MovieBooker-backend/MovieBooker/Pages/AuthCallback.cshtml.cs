using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MovieBooker.Pages
{
    public class AuthCallbackModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;

        public AuthCallbackModel(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection)
        {
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return RedirectToPage("/Login");
            }

            var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
            var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);
            var phone = authenticateResult.Principal.FindFirstValue(ClaimTypes.MobilePhone);
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:5000/api/User/CheckSignUpEmail/{email}");
            if (response.IsSuccessStatusCode)
            {
                var getUser = await response.Content.ReadAsStringAsync();
                var convertUser = JsonConvert.DeserializeObject<UserDTO>(getUser);
                var response3 = await client.PostAsJsonAsync("https://localhost:5000/api/User/GenerateTokens", convertUser);
                if (response3.IsSuccessStatusCode)
                {
                    var gettoken = await response3.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenModel>(gettoken);
                    var redis = _redisConnection.GetDatabase();
                    var accessToken = await redis.StringGetAsync("savetoken");
                    var refreshToken = await redis.StringGetAsync($"RefreshToken:{token.RefreshToken}");
                    if (accessToken.HasValue && refreshToken.HasValue)
                    {
                        Response.Cookies.Append("Token", "savetoken");
                        Response.Cookies.Append("RefreshToken", token.RefreshToken);
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }
            else
            {
                var user = new User { Email = email, UserName = name, RoleId = 3, PhoneNumber = phone };
                var client2 = _httpClientFactory.CreateClient();
                var response2 = await client2.PostAsJsonAsync("https://localhost:5000/api/User/LoginGoogle", user);
                if (response2.IsSuccessStatusCode)
                {
                    var content = await response2.Content.ReadAsStringAsync();
                    var tokens = JsonConvert.DeserializeObject<TokenModel>(content);
                    var redis = _redisConnection.GetDatabase();
                    var accessToken = await redis.StringGetAsync("savetoken");
                    var refreshToken = await redis.StringGetAsync($"RefreshToken:{tokens.RefreshToken}");
                    if (accessToken.HasValue && refreshToken.HasValue)
                    {
                        Response.Cookies.Append("Token", "savetoken");
                        Response.Cookies.Append("RefreshToken", tokens.RefreshToken);
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }
            return RedirectToPage("/Error");
        }
    }
}

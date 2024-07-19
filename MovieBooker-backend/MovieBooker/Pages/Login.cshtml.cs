using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using System.Net.Http;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MovieBooker.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
            _httpClient = httpClient;
        }

        [BindProperty]
        public User User { get; set; } = default!;
        public void OnGet()
        {
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var loginData = new { User.Email, User.Password };

            var response = await httpClient.PostAsJsonAsync("https://localhost:5000/api/User/SignIn", loginData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokens = JsonConvert.DeserializeObject<TokenModel>(content);
                // Lấy access token và refresh token từ Redis
                var redis = _redisConnection.GetDatabase();
                var accessToken = await redis.StringGetAsync("savetoken");
                var refreshToken = await redis.StringGetAsync($"RefreshToken:{tokens.RefreshToken}");

                // Kiểm tra xem có tồn tại token và refresh token
                if (accessToken.HasValue && refreshToken.HasValue)
                {
                    // Lưu token và refresh token vào cookie
                    Response.Cookies.Append("Token", "savetoken");
                    Response.Cookies.Append("RefreshToken", tokens.RefreshToken);

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;
                    var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                    if (roleClaim == "Admin")
                    {
                        return RedirectToPage("/Manage/Dashboard");
                    }else if(roleClaim == "Customer")
                    {
                        return RedirectToPage("/Index");
                    }else if(roleClaim == "Staff")
                    {
                        return RedirectToPage("");
                    }              
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Token not found.");
                    return Page();
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Your email has been locked.");
            }else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Email or password invalid.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed.");
            }
            return Page();
        }
        public async Task<IActionResult> OnGetLogoutAsync()
        {
            var redis = _redisConnection.GetDatabase();
            var accessToken = Request.Cookies["Token"];
            var refreshToken = Request.Cookies["RefreshToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Xóa token và refresh token khỏi Redis
                await redis.KeyDeleteAsync(accessToken);
                await redis.KeyDeleteAsync($"RefreshToken:{refreshToken}");
            }

            // Xóa cookie
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToPage("/Index");
        }

        public IActionResult OnGetLoginGoogle()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/AuthCallback")
            };
            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }






    }
}

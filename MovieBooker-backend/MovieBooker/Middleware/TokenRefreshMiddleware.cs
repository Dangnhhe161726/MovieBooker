using MovieBooker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace MovieBooker.Middleware
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;

        public TokenRefreshMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(token))
            {
                var refreshToken = context.Request.Cookies["RefreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var rediss = _redisConnection.GetDatabase();
                    var getaccessToken = await rediss.StringGetAsync("savetoken");
                    //var getrefreshToken = await rediss.StringGetAsync($"{refreshToken}");
                    if (string.IsNullOrEmpty(getaccessToken))
                    {
                        var httpClient = _httpClientFactory.CreateClient();
                        var response = await httpClient.PostAsJsonAsync("https://localhost:5000/api/User/RefreshToken",refreshToken);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var tokens = JsonConvert.DeserializeObject<TokenModel>(content);

                            var redis = _redisConnection.GetDatabase();

                            var accessToken = await redis.StringGetAsync("savetoken");
                            var RefreshToken = await redis.StringGetAsync($"RefreshToken:{tokens.RefreshToken}");

                            if (accessToken.HasValue && RefreshToken.HasValue)
                            {
                                //context.Response.Cookies.Append("Token", "savetoken", new CookieOptions { HttpOnly = true, Secure = true });
                                //context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });
                                context.Response.Cookies.Append("Token", "savetoken");
                                context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken);
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
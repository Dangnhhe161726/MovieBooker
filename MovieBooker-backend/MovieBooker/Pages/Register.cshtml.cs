using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using StackExchange.Redis;

namespace MovieBooker.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
            _httpClient = httpClient;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var loginData = new { User.Email, User.Password };

            var response = await httpClient.PostAsJsonAsync("https://localhost:5000/api/User/SignUp", loginData);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            return Page();  
        }
    }
}

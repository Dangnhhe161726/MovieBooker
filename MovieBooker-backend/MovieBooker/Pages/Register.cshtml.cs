using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using StackExchange.Redis;
using System.Net;

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
            if (User.Password != User.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Confirm Password incorrect.");
                return Page();
            }


            var httpClient = _httpClientFactory.CreateClient();
            var SignUpData = new { User.UserName, User.Email, User.Password, User.PhoneNumber, User.Address, User.Gender, User.Dob };

            var response = await httpClient.PostAsJsonAsync("https://localhost:5000/api/User/SignUp", SignUpData);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Login");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) //401 
            {
                ModelState.AddModelError(string.Empty, "Email already exists");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Register Failed!!!");
            }

            return Page();
        }
    }
}

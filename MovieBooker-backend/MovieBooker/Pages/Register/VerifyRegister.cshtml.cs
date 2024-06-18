using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net.Http;
using System.Net.Http.Json;

namespace MovieBooker.Pages.Register
{
    public class VerifyRegisterModel : PageModel
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;

        public VerifyRegisterModel(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection)
        {
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
        }

        [BindProperty]
        public int verify { get; set; } = default!;

        [BindProperty]
        public User User { get; set; } = default!;


        public void  OnGet()
        {
            if (TempData["SignUpData"] != null)
            {
                var signUpDataJson = TempData["SignUpData"].ToString();
                User = JsonConvert.DeserializeObject<User>(signUpDataJson);
                TempData["SignUpData"] = signUpDataJson;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var verifycode = _redisConnection.GetDatabase();
            var getcode = await verifycode.StringGetAsync("VerifyEmail");
            if (verify == getcode)
            {
                if (TempData["SignUpData"] != null)
                {
                    var signUpDataJson = TempData["SignUpData"].ToString();
                    User = JsonConvert.DeserializeObject<User>(signUpDataJson);
                    var signupdata = new { User.UserName, User.Email, User.Password, User.PhoneNumber, User.Address, User.Gender, User.Dob };
                    var response = await httpClient.PostAsJsonAsync("https://localhost:5000/api/User/SignUp", signupdata);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sign Up Failed!!!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Verify code incorrect! Input again.");
            }
            return Page();
        }
    }
}

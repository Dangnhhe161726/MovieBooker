using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using StackExchange.Redis;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MovieBooker.Pages.Admin
{
    //[Authorize]
    public class ManageUsersModel : PageModel
    {
        private readonly IAuthenService _authenticationService;
        public ManageUsersModel(IAuthenService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public List<UserDTO> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:5000/api/User/GetAllUser");
                if (response.IsSuccessStatusCode)
                {
                    Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                }
                else
                {
                    return RedirectToPage("/AccessDenied");
                }
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetChangeStatusUserAsync(int id)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.PutAsync($"https://localhost:5000/api/User/ChangeStatusUser/{id}",null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage(new { handler = "OnGet" });
                }
                else
                {
                    return Page();
                }
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }          
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using System.Net.Http.Headers;

namespace MovieBooker.Pages.Admin
{
    public class ManageUserDetailModel : PageModel
    {
        private readonly IAuthenService _authenticationService;
        public ManageUserDetailModel(IAuthenService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public List<UserDTO> Users { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=userId eq {id}");
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
    }
}

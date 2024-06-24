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
        public List<RoleDTO> Roles { get; set; } = default!;

        [BindProperty]
        public UserDTO UserDTO { get; set; }
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
                HttpResponseMessage response2 = await _httpClient.GetAsync("https://localhost:5000/api/Role/GetRole");
                if (response2.IsSuccessStatusCode)
                {
                    Roles = await response2.Content.ReadFromJsonAsync<List<RoleDTO>>();
                }
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int userRole, int id)
        {
            var updateUser = new UpdateUserDTO {UserName = UserDTO.UserName, PhoneNumber = UserDTO.PhoneNumber, Address = UserDTO.Address,
            Gender = UserDTO.Gender, Dob = UserDTO.Dob, Avatar = UserDTO.Avatar, RoleId = userRole  };
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"https://localhost:5000/api/User/UpdateUser/{UserDTO.Email}", updateUser);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Change role success";
                return await OnGetAsync(id);
            }
            else
            {
                return RedirectToPage("/Error");
            }
           
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
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
        public int PageSize { get; } = 5;
        public int PageIndex { get; set; } = 1;

        public int TotalItemsCustomer { get; set; }
        public int TotalItemsStaff { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;
                var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                if (roleClaim == "Admin")
                {
                    PageIndex = pageIndex;
                    HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=roleId eq 3&$top={PageSize}&$skip={(PageIndex - 1) * PageSize}");
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["User"] = "Customer";
                        Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

                        HttpResponseMessage responseTotalNotSeen = await _httpClient.GetAsync($"https://localhost:5000/odata/User/$count?$filter=roleId eq 3");
                        if (responseTotalNotSeen.IsSuccessStatusCode)
                        {
                            TotalItemsCustomer = await responseTotalNotSeen.Content.ReadFromJsonAsync<int>();
                        }

                    }
                    else
                    {
                        return RedirectToPage("/Error");
                    }
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

        public async Task<IActionResult> OnGetStaffAsync(int pageIndex = 1)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=roleId eq 2&$top={PageSize}&$skip={(PageIndex - 1) * PageSize}");
                if (response.IsSuccessStatusCode)
                {
                    PageIndex = pageIndex;
                    Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                    TempData["User"] = "Staff";

                    HttpResponseMessage responseTotalNotSeen = await _httpClient.GetAsync($"https://localhost:5000/odata/User/$count?$filter=roleId eq 2");
                    if (responseTotalNotSeen.IsSuccessStatusCode)
                    {
                        TotalItemsStaff = await responseTotalNotSeen.Content.ReadFromJsonAsync<int>();
                    }

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

        public async Task<IActionResult> OnGetChangeStatusUserAsync(int id, string check)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.PutAsync($"https://localhost:5000/api/User/ChangeStatusUser/{id}",null);
                if (response.IsSuccessStatusCode)
                {
                    if (check == "Customer")
                    {
                        return RedirectToPage(new { handler = "OnGet" });
                    }
                    else
                    {
                        return RedirectToPage(new { handler = "Staff" });
                    }
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

        public async Task<IActionResult> OnPostAsync(string check, string search)
        {
            HttpClient _httpClient = new HttpClient();
            var accessToken = await _authenticationService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if(check == "Customer")
                {

                    HttpResponseMessage response =
                await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=roleId " +
                "eq 3 and (contains(userName, '"+search+"') or contains(email, '"+search+"') or contains(phoneNumber, '"+search+"') " +
                "or contains(address, '"+search+"'))");
                    if (response.IsSuccessStatusCode)
                    {
                        Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                        TempData["User"] = "Customer";
                    }
                }
                else
                {
                    HttpResponseMessage response =
                        await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=roleId " +
                        "eq 2 and (contains(userName, '" + search + "') or contains(email, '" + search + "') or contains(phoneNumber, '" + search + "') " +
                        "or contains(address, '" + search + "'))");
                    if (response.IsSuccessStatusCode)
                    {
                        Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                        TempData["User"] = "Staff";
                    }
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;
using MovieBooker.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MovieBooker.Pages.Users.Profile
{
    public class MyTicketModel : PageModel
    {
        private readonly IAuthenService _authenticationService;
        private readonly HttpClient _httpClient;

        public MyTicketModel(IAuthenService authenticationService, IHttpClientFactory httpClientFactory)
        {
            _authenticationService = authenticationService;
            _httpClient = httpClientFactory.CreateClient();
        }

        public List<ReservationDTO> ReserSeen { get; set; }
        public List<ReservationDTO> ReserNotSeen { get; set; }
        public List<UserDTO> Users { get; set; }

        public int PageSize { get; } = 5;
        public int PageIndex { get; set; } = 1;

        public int TotalItemsSeen { get; set; }
        public int TotalItemsNotSeen { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;

            var accessToken = await _authenticationService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;
            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            HttpResponseMessage responseUser = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=email eq '{email}'");
            if (responseUser.IsSuccessStatusCode)
            {
                Users = await responseUser.Content.ReadFromJsonAsync<List<UserDTO>>();
            }

            int userId = (int)Users.FirstOrDefault()?.UserId;
            DateTime now = DateTime.Now;
            string recentDate = now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseTotalSeen = await _httpClient.GetAsync($"https://localhost:5000/odata/Reservation/$count?$filter=UserId eq {userId} and reservationDate lt {recentDate}");
            if (responseTotalSeen.IsSuccessStatusCode)
            {
                TotalItemsSeen = await responseTotalSeen.Content.ReadFromJsonAsync<int>();
            }

            HttpResponseMessage responseTotalNotSeen = await _httpClient.GetAsync($"https://localhost:5000/odata/Reservation/$count?$filter=UserId eq {userId} and reservationDate gt {recentDate}");
            if (responseTotalNotSeen.IsSuccessStatusCode)
            {
                TotalItemsNotSeen = await responseTotalNotSeen.Content.ReadFromJsonAsync<int>();
            }

            HttpResponseMessage responseSeen = await _httpClient.GetAsync($"https://localhost:5000/api/Reservation?$filter=UserId eq {userId} and reservationDate lt {recentDate}&$orderby=reservationId desc&$top={PageSize}&$skip={(PageIndex - 1) * PageSize}");
            if (responseSeen.IsSuccessStatusCode)
            {
                ReserSeen = await responseSeen.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }

            HttpResponseMessage responseNotSeen = await _httpClient.GetAsync($"https://localhost:5000/api/Reservation?$filter=UserId eq {userId} and reservationDate gt {recentDate}&$orderby=reservationId desc&$top={PageSize}&$skip={(PageIndex - 1) * PageSize}");
            if (responseNotSeen.IsSuccessStatusCode)
            {
                ReserNotSeen = await responseNotSeen.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }
        }
    }
}

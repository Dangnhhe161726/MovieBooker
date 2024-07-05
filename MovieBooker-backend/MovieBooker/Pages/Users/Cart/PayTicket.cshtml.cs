using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;

namespace MovieBooker.Pages.Users.Cart
{
    public class PayTicketModel : PageModel
    {
        private readonly IAuthenService _authenticationService;
        public PayTicketModel(IAuthenService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public List<ScheduleDTO> schedules { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public List<SeatDTO> seats { get; set; }
        public List<UserDTO> Users { get; set; }

        public async Task<IActionResult> OnPostBuyTicketAsync(int timeSlotId, double movieprice, List<int> seatId, 
            int movieId, int scheduleId, double finalTotalPrice)
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await
           _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=schedulesId eq {scheduleId}");
            if (response.IsSuccessStatusCode)
            {
                schedules = await response.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
            HttpResponseMessage response2 = await _httpClient.GetAsync($"https://localhost:5000/api/Seat/GetSeat");
            if (response2.IsSuccessStatusCode)
            {
                var allSeats = await response2.Content.ReadFromJsonAsync<List<SeatDTO>>();
                seats = allSeats.Where(seat => seatId.Contains(seat.SeatId)).ToList();
            }
            TempData["totalprice"] = finalTotalPrice;
            TempData["seatId"] = JsonConvert.SerializeObject(seatId);
            return Page();
        }

        public async Task<IActionResult> OnPostPaymentAsync(int timeslotid, int movieid, double totalprice, DateTime resdate)
        {
            var seatIdJson = TempData["seatId"].ToString();
            List<int> seatId = JsonConvert.DeserializeObject<List<int>>(seatIdJson);
            var accessToken = await _authenticationService.GetAccessTokenAsync();
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;
            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=email eq '{email}'");
            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            }
            int userId = 0; 
            foreach(var id in Users)
            {
                userId = id.UserId;
            }

            for (int i = 0; i < seatId.Count(); i++ ) {
                var res = new CreateResevationDTO
                {
                    MovieId = movieid,
                    UserId = userId,
                    TimeSlotId = timeslotid,
                    SeatId = seatId[i],
                    ReservationDate = resdate,
                    Status = false, 
                    TotalAmount = totalprice/seatId.Count(),
                };
                HttpResponseMessage response2 = await
               _httpClient.PostAsJsonAsync("https://localhost:5000/api/Reservation/CreateReservation", res);
            }
            return RedirectToPage("/Users/Cart/BookedSuccess");
        }
    }
}

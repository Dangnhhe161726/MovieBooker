using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;

namespace MovieBooker.Pages.Manage
{
    public class ReservationHistoryModel : PageModel
    {
        public List<ReservationDTO> reservations = new List<ReservationDTO>();
        public async Task OnGetAsync()
        {

            HttpClient _httpClient = new HttpClient();
            var apiUrlRes = "https://localhost:5000/api/Reservation";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrlRes);
            if (response.IsSuccessStatusCode)
            {
                reservations = await response.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }
        }
    }
}

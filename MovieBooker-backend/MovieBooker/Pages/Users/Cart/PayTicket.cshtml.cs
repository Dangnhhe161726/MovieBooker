using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;

namespace MovieBooker.Pages.Users.Cart
{
    public class PayTicketModel : PageModel
    {
        public List<ScheduleDTO> schedules { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostBuyTicketAsync(int timeSlotId, double movieprice, List<int> seatId, int movieId, int scheduleId)
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await
           _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=schedulesId eq {scheduleId}");
            if (response.IsSuccessStatusCode)
            {
                schedules = await response.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
            return Page();
        }
    }
}

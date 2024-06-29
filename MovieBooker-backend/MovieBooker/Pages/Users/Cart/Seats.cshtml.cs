using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;

namespace MovieBooker.Pages.Users.Cart
{
    public class BookSeatModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public TicketDTO ticket {  get; set; }
        public List<ScheduleDTO> schedules { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string? date = ticket.start;
            int? movieId = ticket.movieId;
            int? theartr = ticket.theartr;
            int? slot = ticket.timeslot;
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response2 = await
            _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate eq {date} and movieId eq {movieId} and timeSlotId eq {slot} and theaterId eq {theartr}");
            if (response2.IsSuccessStatusCode)
            {
                schedules = await response2.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
            return Page();  
        }
    }
}

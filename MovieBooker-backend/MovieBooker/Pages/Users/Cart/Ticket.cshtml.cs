using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using System.Net.Http;

namespace MovieBooker.Pages.Users.Cart
{
    public class TicketModel : PageModel
    {
        [BindProperty]
        public int? SelectedTheaterId { get; set; }

        [BindProperty]
        public int? SelectedTimeSlotId { get; set; }
        public List<DateTime> SelectedDates { get; set; }
        public List<TheaterDTO> theaters { get; set; }

        public List<ScheduleDTO> schedules { get; set; }
        private readonly IAuthenService _authenticationService;
        public TicketModel(IAuthenService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var accessToken = await _authenticationService.GetAccessTokenAsync();
            if (accessToken == null)
            {
                return RedirectToPage("/Login");
            }
            else
            {

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today.AddDays(28);

                string start;
                if (Request.Cookies.TryGetValue("selectedDate", out var selectedDate))
                {
                    start = selectedDate;
                }
                else
                {
                    start = startDate.ToString("yyyy-MM-dd");
                }

                //string end = endDate.ToString("yyyy-MM-dd");
                SelectedDates = InitializeDefaults(startDate, endDate);
                HttpClient _httpClient = new HttpClient();
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:5000/api/Theater/GetAllTheater");
                if (response.IsSuccessStatusCode)
                {
                    theaters = await response.Content.ReadFromJsonAsync<List<TheaterDTO>>();
                }
                if (schedules == null)
                {
                    HttpResponseMessage response2 = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate eq {start} and movieId eq {id}");
                    if (response2.IsSuccessStatusCode)
                    {
                        schedules = await response2.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
                    }
                }
                TempData["movieId"] = id;
                return Page();
            }
        }
        private List<DateTime> InitializeDefaults(DateTime startDate, DateTime endDate)
        {
            List<DateTime> datesInRange = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                datesInRange.Add(date);
            }
            return datesInRange;
        }

        public async Task<IActionResult> OnGetScheduleAsync(DateTime selectedate, int movieId)
        {

            string date = selectedate.ToString("yyyy-MM-dd");
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response2 = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate eq {date} and movieId eq {movieId}");
            if (response2.IsSuccessStatusCode)
            {
                schedules = await response2.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }

            return await OnGetAsync(movieId);
        }

        public async Task<IActionResult> OnPostAsync(int movieId)
        {
            string? start = null;
            if (Request.Cookies.TryGetValue("selectedDate", out var selectedDate))
            {
                start = selectedDate;
            }
            int theartr = (int)SelectedTheaterId;
            int timeslot = (int) SelectedTimeSlotId;


            return RedirectToPage("/Users/Cart/Seats", new TicketDTO
            {
                start = start,
                theartr = theartr,
                timeslot = timeslot,
                movieId = movieId
            });
        }
    }
}

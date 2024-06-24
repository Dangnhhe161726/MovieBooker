using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;

namespace MovieBooker.Pages.Admin
{
    public class ManageSchedulesModel : PageModel
    {

        public List<ScheduleDTO> Schedules { get; set; }
        public List<TimeSlotDTO> TimeSlots { get; set; }

        [BindProperty]
        public SelectDateModel Input { get; set; }
        public List<DateTime> SelectedDates { get; set; }
        public async Task<IActionResult> OnGet()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(6);

            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");
            SelectedDates = InitializeDefaults(startDate, endDate);
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate gt {start} and scheduleDate lt {end}");
            if (response.IsSuccessStatusCode)
            {
                Schedules = await response.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
            HttpResponseMessage response2 = await _httpClient.GetAsync("https://localhost:5000/api/TimeSlot/GetAllTimeSlot");
            if (response2.IsSuccessStatusCode)
            {
                TimeSlots = await response2.Content.ReadFromJsonAsync<List<TimeSlotDTO>>();
            }
            return Page();  
        }

        public async Task<IActionResult> OnPostAsync()
        {
            DateTime startDate = Input.StartDate;
            DateTime endDate = Input.EndDate;
            // Lấy danh sách các ngày trong khoảng thời gian từ startDate đến endDate
            SelectedDates = GetDatesInRange(startDate, endDate);

            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");
            HttpClient _httpClient = new HttpClient();

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate gt {start} and scheduleDate lt {end}");
            if (response.IsSuccessStatusCode)
            {
                Schedules = await response.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }

            HttpResponseMessage response2 = await _httpClient.GetAsync("https://localhost:5000/api/TimeSlot/GetAllTimeSlot");
            if (response2.IsSuccessStatusCode)
            {
                TimeSlots = await response2.Content.ReadFromJsonAsync<List<TimeSlotDTO>>();
            }

            return Page();
        }
        private List<DateTime> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            List<DateTime> datesInRange = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                datesInRange.Add(date);
            }
            return datesInRange;
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
        public async Task<IActionResult> OnGetCreateScheduleAsync()
        {
            return RedirectToPage("/Admin/CreateSchedule");
        }

        public class SelectDateModel
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using MovieBooker.Models;
using System.Net.Http;

namespace MovieBooker.Pages.Admin
{
    public class CreateScheduleModel : PageModel
    {
        public List<MovieDto> moives { get; set; }
        public List<TheaterDTO> theaters { get; set; }
        public List<TimeSlotDTO> TimeSlots { get; set; }
        public List<RoomDTO> Rooms { get; set; }
        public TimeSlotDTO TimeSlot { get; set; }

        [BindProperty]
        public CreateScheduleDTO Creates { get; set; }

        [BindProperty]
        public List<int> SelectedTimeSlotIds { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:5000/api/Movie?$filter=statusId eq 1 or statusId eq 2");
            if (response.IsSuccessStatusCode)
            {
                moives = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
            }

            HttpResponseMessage response2 = await _httpClient.GetAsync("https://localhost:5000/api/TimeSlot/GetAllTimeSlot");
            if (response2.IsSuccessStatusCode)
            {
                TimeSlots = await response2.Content.ReadFromJsonAsync<List<TimeSlotDTO>>();
            }

            HttpResponseMessage response3 = await _httpClient.GetAsync("https://localhost:5000/api/Theater/GetAllTheater");
            if (response3.IsSuccessStatusCode)
            {
                theaters = await response3.Content.ReadFromJsonAsync<List<TheaterDTO>>();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedTimeSlotIds == null || !SelectedTimeSlotIds.Any())
            {
                TempData["mess"] = "Please select at least one time slot.";
                return await OnGetAsync();
            }

            List<int> duplicateSchedules = new List<int>();
            List<string> duplicateSche = new List<string>();
            List<string> successfulSchedules = new List<string>();

            HttpClient _httpClient = new HttpClient();
            string rlDate = Creates.ScheduleDate.Value.ToString("yyyy-MM-dd");

            foreach (var timeSlotId in SelectedTimeSlotIds)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/CheckExistSchedule/{Creates.TheaterId}/{timeSlotId}/{rlDate}/{Creates.RoomId}");
                if (response.IsSuccessStatusCode)
                {
                    HttpResponseMessage response3 = await _httpClient.GetAsync($"https://localhost:5000/api/TimeSlot/GetTimeSlotById/{timeSlotId}");
                    if (response3.IsSuccessStatusCode)
                    {
                        TimeSlot = await response3.Content.ReadFromJsonAsync<TimeSlotDTO>();
                        duplicateSche.Add(TimeSlot.StartTime.ToString());
                        duplicateSchedules.Add(timeSlotId);
                    }
                }
            }

            foreach (var timeSlotId in SelectedTimeSlotIds.Except(duplicateSchedules))
            {
                Creates.TimeSlotId = timeSlotId;
                HttpResponseMessage response2 = await _httpClient.PostAsJsonAsync("https://localhost:5000/api/Schedule/CreateSchedule", Creates);
                if (response2.IsSuccessStatusCode)
                {
                    HttpResponseMessage response3 = await _httpClient.GetAsync($"https://localhost:5000/api/TimeSlot/GetTimeSlotById/{timeSlotId}");
                    if (response3.IsSuccessStatusCode)
                    {
                        TimeSlot = await response3.Content.ReadFromJsonAsync<TimeSlotDTO>();
                        successfulSchedules.Add(TimeSlot.StartTime.ToString());
                    }
                }
            }

            if (duplicateSche.Any())
            {
                TempData["exist"] = $"Some schedules already exist: {string.Join(", ", duplicateSche)}";
            }

            if (successfulSchedules.Any())
            {
                TempData["mess"] = $"Successfully created schedules: {string.Join(", ", successfulSchedules)}";
            }

            return await OnGetAsync();
        }
    }
}

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

        [BindProperty]
        public CreateScheduleDTO Creates { get; set; }

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
            string rlDate = Creates.ScheduleDate.Value.ToString("yyyy-MM-dd");
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5000/api/Schedule/CheckExistSchedule/{Creates.MovieId}/{Creates.TheaterId}/{Creates.TimeSlotId}/{rlDate}");
            if (response.IsSuccessStatusCode)
            {
                TempData["mess"] = "Schedule movie is exited. Create Schedule again!!!";
                return await OnGetAsync();
            }
            else
            {
                HttpResponseMessage response2 = await _httpClient.PostAsJsonAsync("https://localhost:5000/api/Schedule/CreateSchedule", Creates);
                if (response2.IsSuccessStatusCode)
                {
                    TempData["mess"] = "Create Schedule Movie Successfully";
                    return await OnGetAsync();
                }
                else
                {
                    TempData["mess"] = "Create Schedule Movie Failed";
                    return await OnGetAsync();
                }
            }
        }
    }
}

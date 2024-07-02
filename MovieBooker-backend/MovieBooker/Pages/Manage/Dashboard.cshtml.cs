using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;

namespace MovieBooker.Pages.Manage
{
    public class DashboardModel : PageModel
    {
        public WeekDashboardDTO weekDashboard = new WeekDashboardDTO();
        public async Task OnGetAsync()
        {


            HttpClient _httpClient = new HttpClient();
            var apiUrlRes = "https://localhost:5000/api/Dashboard/GetWeeklyDashboard";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrlRes);
            if (response.IsSuccessStatusCode)
            {
                weekDashboard = await response.Content.ReadFromJsonAsync<WeekDashboardDTO>();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;
using System.Net.Http;

namespace MovieBooker.Pages.Manage
{
    public class DashboardModel : PageModel
    {
        public WeekDashboardDTO weekInfo = new WeekDashboardDTO();
        public List<MovieDashboardDTO> movieList = new List<MovieDashboardDTO>();
        public List<MonthDashboardDTO> monthInfo = new List<MonthDashboardDTO>();
        public async Task OnGetAsync()
        {
            HttpClient _httpClient = new HttpClient();
            var apiUrlWeek = "https://localhost:5000/api/Dashboard/GetWeeklyDashboard";
            var apiUrlMovie = "https://localhost:5000/api/Dashboard/GetMovieDashboard";
            var apiUrlMonth = "https://localhost:5000/api/Dashboard/GetMonthlyDashboard";

            HttpResponseMessage weekResponse = await _httpClient.GetAsync(apiUrlWeek);
            if (weekResponse.IsSuccessStatusCode)
            {
                weekInfo = await weekResponse.Content.ReadFromJsonAsync<WeekDashboardDTO>();
            }

            HttpResponseMessage movieResponse = await _httpClient.GetAsync(apiUrlMovie);
            if (movieResponse.IsSuccessStatusCode)
            {
                movieList = await movieResponse.Content.ReadFromJsonAsync<List<MovieDashboardDTO>>();
            }

            HttpResponseMessage monthResponse = await _httpClient.GetAsync(apiUrlMonth);
            if (monthResponse.IsSuccessStatusCode)
            {
                monthInfo = await monthResponse.Content.ReadFromJsonAsync<List<MonthDashboardDTO>>();
            }
        }

        public async Task<IActionResult> OnGetExportReportAsync(string type)
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage exportResponse = await _httpClient.GetAsync($"https://localhost:5000/api/Dashboard/ExportReport?type={type}");

            if (exportResponse.IsSuccessStatusCode)
            {
                var fileBytes = await exportResponse.Content.ReadAsByteArrayAsync();
                var fileName = exportResponse.Content.Headers.ContentDisposition.FileName.Trim('"');
                var contentType = exportResponse.Content.Headers.ContentType.ToString();

                return File(fileBytes, contentType, fileName);
            }
            else
            {
                return BadRequest("Failed to download the report");

            }
        }
    }
}

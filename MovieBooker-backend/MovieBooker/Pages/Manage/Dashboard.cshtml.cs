using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;
using System.Net.Http;

namespace MovieBooker.Pages.Manage
{
    public class DashboardModel : PageModel
    {
        public string ViewType = "default";
        public WeekDashboardDTO WeekInfo = new WeekDashboardDTO();
        public List<MovieDashboardDTO> MovieList = new List<MovieDashboardDTO>();
        public List<ChartDashboardDTO> ChartInfo = new List<ChartDashboardDTO>();
        public async Task OnGetAsync(string timeFilter)
        {

            HttpClient _httpClient = new HttpClient();

            ViewType = timeFilter ?? "default";
            var apiUrlChart = $"https://localhost:5000/api/Dashboard/GetChartDashboard/{ViewType}";
            HttpResponseMessage chartResponse = await _httpClient.GetAsync(apiUrlChart);
            if (chartResponse.IsSuccessStatusCode)
            {
                ChartInfo = await chartResponse.Content.ReadFromJsonAsync<List<ChartDashboardDTO>>();
            }

            var apiUrlWeek = "https://localhost:5000/api/Dashboard/GetWeeklyDashboard";
            HttpResponseMessage weekResponse = await _httpClient.GetAsync(apiUrlWeek);
            if (weekResponse.IsSuccessStatusCode)
            {
                WeekInfo = await weekResponse.Content.ReadFromJsonAsync<WeekDashboardDTO>();
            }

            var apiUrlMovie = "https://localhost:5000/api/Dashboard/GetMovieDashboard";
            HttpResponseMessage movieResponse = await _httpClient.GetAsync(apiUrlMovie);
            if (movieResponse.IsSuccessStatusCode)
            {
                MovieList = await movieResponse.Content.ReadFromJsonAsync<List<MovieDashboardDTO>>();
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

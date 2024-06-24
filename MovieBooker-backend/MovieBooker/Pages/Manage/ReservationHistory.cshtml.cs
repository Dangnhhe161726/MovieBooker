using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;

namespace MovieBooker.Pages.Manage
{
    public class ReservationHistoryModel : PageModel
    {
        public List<ReservationDTO> reservations = new List<ReservationDTO>();
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? StatusFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? StartTime { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? EndTime { get; set; }

        public async Task OnGetAsync()
        {

            HttpClient _httpClient = new HttpClient();
            var apiUrlRes = "https://localhost:5000/api/Reservation";
            /*var apiUrlRes = $"https://localhost:5000/odata/Reservation?$filter=contains(MovieTitle, '{SearchTerm}')";

            if (StatusFilter.HasValue)
            {
                apiUrlRes += $" and Status eq {StatusFilter.Value}";
            }
            if (StartTime.HasValue && EndTime.HasValue)
            {
                apiUrlRes += $" and StartTime ge {StartTime.Value:o} and EndTime le {EndTime.Value:o}";
            }*/
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrlRes);
            if (response.IsSuccessStatusCode)
            {
                reservations = await response.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }
        }
    }
}

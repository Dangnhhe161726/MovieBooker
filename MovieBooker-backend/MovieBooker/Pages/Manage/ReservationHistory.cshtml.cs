using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;
using System.Drawing.Printing;
using System.Globalization;
using System.Net.Http;

namespace MovieBooker.Pages.Manage
{
    public class ReservationHistoryModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ReservationHistoryModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public List<ReservationDTO> Reservations = new List<ReservationDTO>();
        public int NumReservations { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 15;
        public string KeySearch { get; set; } = "";
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";

        public async Task OnGet(int currentPage = 1, string sortBy = "MovieId", string sortOrder = "asc")
        {
            CurrentPage = currentPage;
            SortBy = sortBy;
            SortOrder = sortOrder;

            // Get count of filtered movies
            var countResponse = await _httpClient.GetAsync($"https://localhost:5000/odata/Reservation/$count");
            if (countResponse.IsSuccessStatusCode)
            {
                var numReservationString = await countResponse.Content.ReadAsStringAsync();
                NumReservations = int.TryParse(numReservationString, out int count) ? count : 0;
            }

            int skip = (currentPage - 1) * PageSize;
            int top = PageSize;

            // Get sorted and paginated movies
            var sortParameter = $"{SortBy} {(SortOrder == "asc" ? "asc" : "desc")}";
            var dataResponse = await _httpClient.GetAsync($"https://localhost:5000/api/Reservation?$top={top}&$skip={skip}&$orderby={sortParameter}");
            if (dataResponse.IsSuccessStatusCode)
            {
                Reservations = await dataResponse.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }


        }
    }
}

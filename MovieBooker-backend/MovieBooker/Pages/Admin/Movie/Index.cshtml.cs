using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieBooker.Pages.Admin.Movie
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<MovieDto> Movies { get; set; }

        public int NumMovies { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public string KeySearch { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc";

        public async Task OnGet(string keySearch, int currentPage = 1, string sortBy = "MovieId", string sortOrder = "asc")
        {
            CurrentPage = currentPage;
            KeySearch = keySearch;
            SortBy = sortBy;
            SortOrder = sortOrder;

            var query = string.IsNullOrWhiteSpace(keySearch) ? "" : $"$filter=contains(tolower(movieTitle), '{keySearch.ToLower()}')";

            // Get count of filtered movies
            var countResponse = await _httpClient.GetAsync($"https://localhost:5000/odata/Movie/$count?{query}");
            if (countResponse.IsSuccessStatusCode)
            {
                var numMovieString = await countResponse.Content.ReadAsStringAsync();
                NumMovies = int.TryParse(numMovieString, out int count) ? count : 0;
            }

            int skip = (currentPage - 1) * PageSize;
            int top = PageSize;

            // Get sorted and paginated movies
            var sortParameter = $"{SortBy} {(SortOrder == "asc" ? "asc" : "desc")}";
            var dataResponse = await _httpClient.GetAsync($"https://localhost:5000/api/Movie?{query}&$top={top}&$skip={skip}&$orderby={sortParameter}");
            if (dataResponse.IsSuccessStatusCode)
            {
                Movies = await dataResponse.Content.ReadFromJsonAsync<List<MovieDto>>();
            }
        }
    }

}

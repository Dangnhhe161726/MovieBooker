using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;

namespace MovieBooker.Pages.Admin.Movie
{
    public class MovieManagerModel : PageModel
    {

        private readonly HttpClient _httpClient = new HttpClient();
        public List<MovieDto> moives { get; set; }
        public async Task OnGet()
        {
            var apiUrl = "https://localhost:5000/api/movie";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                moives = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
            }
        }
    }
}

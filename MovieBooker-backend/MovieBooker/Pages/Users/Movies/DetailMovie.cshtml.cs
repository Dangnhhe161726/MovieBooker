using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;

namespace MovieBooker.Pages.Users.Movies
{
    public class DetailMovieModel : PageModel
    {
        public List<MovieDto> moives { get; set; }
        public List<MovieImageDto> movieImages { get; set; }
        public List<MovieImageDto> m{ get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.GetAsync($"https://localhost:5000/api/movie?$filter=movieId eq {id}");
            if (response.IsSuccessStatusCode)
            {
                moives = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
            }

            var response2 = await _httpClient.GetAsync($"https://localhost:5000/api/MovieImage");
            if (response2.IsSuccessStatusCode)
            {
                m = await response2.Content.ReadFromJsonAsync<List<MovieImageDto>>();
                movieImages = m.Where(s => s.MovieId  == id).ToList();
            }
            return Page();
        }

       
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Net.Http;

namespace MovieBooker.Pages.Users.Movies
{
    public class ListMovieModel : PageModel
    {
        public List<MovieDto> moives { get; set; }
        public List<MovieCategoryDto> category { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.GetAsync($"https://localhost:5000/api/movie?$filter=statusId eq {id}");
            if (response.IsSuccessStatusCode)
            {
                moives = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
                if(id == 2)
                {
                    TempData["movie"] = "status2";
                }else if (id == 1)
                {

                    TempData["movie"] = "status1";
                }
            }

            var response2 = await _httpClient.GetAsync("https://localhost:5000/api/MovieCategory");
            if (response2.IsSuccessStatusCode)
            {
                category = await response2.Content.ReadFromJsonAsync<List<MovieCategoryDto>>();
            }

                TempData["enable"] = "search";
                TempData["movieStatusId"] = id;

            return Page();
        }

        public async Task<IActionResult> OnPostSearchMovieAsync(int categories, string moviename, int statusId)
        {
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.GetAsync($"https://localhost:5000/api/movie?$filter=statusId eq {statusId}" +
            (categories != 0 ? $" and categoryId eq {categories}" : "") +
            (!string.IsNullOrEmpty(moviename) ? $" and contains(movieTitle, '{moviename}')" : ""));

            if (response.IsSuccessStatusCode)
            {
                moives = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
                if (statusId == 2)
                {
                    TempData["movie"] = "status2";
                }
                else if (statusId == 1)
                {

                    TempData["movie"] = "status1";
                }
            }
            var response2 = await _httpClient.GetAsync("https://localhost:5000/api/MovieCategory");
            if (response2.IsSuccessStatusCode)
            {
                category = await response2.Content.ReadFromJsonAsync<List<MovieCategoryDto>>();
            }
            TempData["enable"] = "search";
            TempData["movieStatusId"] = statusId;
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Net.Http;

namespace MovieBooker.Pages.Users.Movies
{
    public class ListMovieModel : PageModel
    {
        public List<MovieDto> moives { get; set; }
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
   

            return Page();
        }
    }
}

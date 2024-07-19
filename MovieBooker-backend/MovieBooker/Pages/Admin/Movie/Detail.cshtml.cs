using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Net.Http;

namespace MovieBooker.Pages.Admin.Movie
{
    public class DetailModel : PageModel
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<EditModel> _logger;

		public DetailModel(IHttpClientFactory httpClientFactory, ILogger<EditModel> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		[BindProperty]
		public MovieDto movie { get; set; }

		public async Task<IActionResult> OnGetDetailHandler(int? id)
		{
			if (id == null)
			{
				_logger.LogError("Movie ID is null.");
				return RedirectToPage("/Admin/Movie/Index");
			}

			var httpClient = _httpClientFactory.CreateClient();

			try
			{
				movie = await FetchMovieAsync(httpClient, id.Value);
				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching movie data.");
				return Page();
			}
		}

		private async Task<MovieDto> FetchMovieAsync(HttpClient httpClient, int id)
		{
			var apiUrl = $"https://localhost:5000/api/movie?filter=MovieId eq {id}";
			var response = await httpClient.GetAsync(apiUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException($"Failed to fetch movie with ID {id}");
			}

			var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
			if (movies == null || movies.Count != 1)
			{
				throw new Exception($"Movie with ID {id} not found or multiple entries returned.");
			}

			return movies.First();
		}
	}
}

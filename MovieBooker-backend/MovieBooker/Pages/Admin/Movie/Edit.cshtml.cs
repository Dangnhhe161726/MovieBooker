using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MovieBooker.Pages.Admin.Movie
{
	public class EditModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<EditModel> _logger;

		public EditModel(IHttpClientFactory httpClientFactory, ILogger<EditModel> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		[BindProperty]
		public MovieDto movie { get; set; }
		public List<VideoDetailDto> VideoDetails { get; set; }
		public List<MovieCategoryDto> MovieCategories { get; set; }
		public List<MovieStatusDto> MovieStatuses { get; set; }
		public List<MovieImageDto> Images { get; set; }
		public IFormFileCollection NewImages { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
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
				var fetchTasks = new List<Task>
				{
					FetchVideoDetailsAsync(httpClient),
					FetchMovieCategoriesAsync(httpClient),
					FetchMovieStatusesAsync(httpClient),
					FetchMovieImagesAsync(httpClient)
				};

				await Task.WhenAll(fetchTasks);

				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching movie data.");
				return RedirectToPage("/Admin/Movie/Index");
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			int a = 0;
			return RedirectToPage("/Admin/Movie/Edit", new { id = movie.MovieId });
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

		private async Task FetchVideoDetailsAsync(HttpClient httpClient)
		{
			var apiUrl = "https://localhost:5000/api/Video";
			var response = await httpClient.GetAsync(apiUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Failed to fetch video details.");
			}

			VideoDetails = await response.Content.ReadFromJsonAsync<List<VideoDetailDto>>()
						   ?? new List<VideoDetailDto>();
		}

		private async Task FetchMovieCategoriesAsync(HttpClient httpClient)
		{
			var apiUrl = "https://localhost:5000/api/MovieCategory";
			var response = await httpClient.GetAsync(apiUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Failed to fetch movie categories.");
			}

			MovieCategories = await response.Content.ReadFromJsonAsync<List<MovieCategoryDto>>()
							  ?? new List<MovieCategoryDto>();
		}

		private async Task FetchMovieStatusesAsync(HttpClient httpClient)
		{
			var apiUrl = "https://localhost:5000/api/MovieStatus";
			var response = await httpClient.GetAsync(apiUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Failed to fetch movie statuses.");
			}

			MovieStatuses = await response.Content.ReadFromJsonAsync<List<MovieStatusDto>>()
							?? new List<MovieStatusDto>();
		}

		private async Task FetchMovieImagesAsync(HttpClient httpClient)
		{
			var apiUrl = "https://localhost:5000/api/MovieImage";
			var response = await httpClient.GetAsync(apiUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Failed to fetch movie images.");
			}

			Images = await response.Content.ReadFromJsonAsync<List<MovieImageDto>>()
					 ?? new List<MovieImageDto>();
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Dtos;
using System.Net.Http;
using System.Text.Json;

namespace MovieBooker.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		public List<String> Banners { get; set; }
		public List<MovieDto> MoviesShowing { get; set; }
		public List<MovieDto> MoviesComing { get; set; }
		public List<MovieDto> MoviesHot { get; set; }
		public MovieDto MovieComing { get; set; }

		public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			Banners = new List<string>
			{
				"./images/banners/banner_1.jpg",
				"./images/banners/banner_2.jpg",
				"./images/banners/banner_3.png",
				"./images/banners/banner_4.jpg"
			};

			using (var client = _httpClientFactory.CreateClient())
			{
				var responseMoviesComing = await client.GetAsync("https://localhost:5000/api/Movie?$filter=statusId eq 1 and enable eq true&$orderby=movieId desc&$top=8");
				if (responseMoviesComing.IsSuccessStatusCode)
				{
					MoviesComing = await responseMoviesComing.Content.ReadFromJsonAsync<List<MovieDto>>();
				}
				else
				{
					_logger.LogError("Error retrieving movies coming: {StatusCode}", responseMoviesComing.StatusCode);
				}

				var responseMovieShowing = await client.GetAsync("https://localhost:5000/api/Movie?$filter=statusId eq 2 and enable eq true&$orderby=movieId desc&$top=8");
				if (responseMovieShowing.IsSuccessStatusCode)
				{
					MoviesShowing = await responseMovieShowing.Content.ReadFromJsonAsync<List<MovieDto>>();
				}
				else
				{
					_logger.LogError("Error retrieving movies showing: {StatusCode}", responseMovieShowing.StatusCode);
				}

				var responseMovieComing = await client.GetAsync("https://localhost:5000/api/Movie?$filter=enable eq true&$orderby=releaseDate desc&$top=1");
				if (responseMovieComing.IsSuccessStatusCode)
				{
					try
					{
						var movies = await responseMovieComing.Content.ReadFromJsonAsync<List<MovieDto>>();
						MovieComing = movies.First();
					}
					catch (JsonException ex)
					{
						var json = await responseMovieComing.Content.ReadAsStringAsync();
						_logger.LogError("Error deserializing MovieComing: {Exception}. JSON: {Json}", ex, json);
					}
				}
				else
				{
					_logger.LogError("Error retrieving movie coming: {StatusCode}", responseMovieComing.StatusCode);
				}
			}

			return Page();
		}

	}
}
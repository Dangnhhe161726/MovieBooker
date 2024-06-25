using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MovieBooker_backend.Repositories.MovieRepository;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MovieController : ControllerBase
	{
		private readonly IMovieRepository movieRepository;

		public MovieController(IMovieRepository movieRepository)
		{
			this.movieRepository = movieRepository;
		}

		[HttpGet()]
		[EnableQuery]
		public async Task<IActionResult> Get()
		{
			var listMovie = movieRepository.getAll();
			return Ok(listMovie);
		}

	}
}

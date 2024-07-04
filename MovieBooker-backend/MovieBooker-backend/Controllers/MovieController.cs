using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MovieBooker_backend.DTO;
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

		[HttpPut()]
		public async Task<IActionResult> Update(MovieDto updateMovie)
		{
			try
			{
				var movie = movieRepository.update(updateMovie);
				return Ok(movie);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				movieRepository.deleteById(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost()]
		public async Task<IActionResult> Insert(MovieDto insertMovie)
		{
			try
			{
				var movie = movieRepository.insert(insertMovie);
				return Ok(movie);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

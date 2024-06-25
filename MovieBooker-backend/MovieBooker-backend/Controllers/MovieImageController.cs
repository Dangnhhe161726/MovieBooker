using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.MovieImageRepository;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MovieImageController : ControllerBase
	{
		private readonly IMovieImageRepository movieImageRepository;

		public MovieImageController(IMovieImageRepository movieImageRepository)
		{
			this.movieImageRepository = movieImageRepository;
		}

		[HttpGet]
		public IActionResult getAll()
		{
			return Ok(movieImageRepository.getAll());
		}

		[HttpPost]
		public IActionResult insert(List<MovieImageResponse> newImage)
		{
			var images = movieImageRepository.insert(newImage);
			return Ok(images);
		}

		[HttpPut("{id}")]
		public IActionResult update(int id, MovieImageResponse update)
		{
			try
			{
				var image = movieImageRepository.update(id, update);
				return Ok(image);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult delete(int id)
		{
			try
			{
				movieImageRepository.deleteById(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

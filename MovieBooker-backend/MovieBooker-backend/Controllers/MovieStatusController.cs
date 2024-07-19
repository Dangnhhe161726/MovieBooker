using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.MovieStatusRepository;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MovieStatusController : ControllerBase
	{
		private readonly IMovieStatusRepository movieStatusRepository;

		public MovieStatusController(IMovieStatusRepository movieStatusRepository)
		{
			this.movieStatusRepository = movieStatusRepository;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			return Ok(movieStatusRepository.getAll());
		}

		[HttpPost]
		public IActionResult Insert(MovieStatusResponse newStatus)
		{
			var status = movieStatusRepository.insert(newStatus);
			return Ok(status);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, MovieStatusResponse updateStatus)
		{
			try
			{
				var status = movieStatusRepository.update(id, updateStatus);
				return Ok(status);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				movieStatusRepository.deleteById(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.MovieCategoryRepository;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MovieCategoryController : ControllerBase
	{
		private readonly IMovieCategoryRepsitory movieCategoryRepsitory;

		public MovieCategoryController(IMovieCategoryRepsitory movieCategoryRepsitory)
		{
			this.movieCategoryRepsitory = movieCategoryRepsitory;
		}

		[HttpGet]
		public IActionResult getAll()
		{
			var category = movieCategoryRepsitory.getAll();
			return Ok(category);
		}

		[HttpPost]
		public IActionResult insert(MovieCategoryResponse newCategory)
		{
			var category = movieCategoryRepsitory.insert(newCategory);
			return Ok(category);
		}

		[HttpPut("{id}")]
		public IActionResult update(int id, MovieCategoryResponse updateCategory)
		{
			try
			{
				var category = movieCategoryRepsitory.update(id, updateCategory);
				return Ok(category);
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
				movieCategoryRepsitory.deleteById(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

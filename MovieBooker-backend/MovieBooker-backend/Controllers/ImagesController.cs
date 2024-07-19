using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.CloudinaryRepository;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly ICloudinaryRepository _cloudinaryRepository;

		public ImagesController(ICloudinaryRepository cloudinaryRepository)
		{
			_cloudinaryRepository = cloudinaryRepository;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
		{
			if (files == null || files.Count == 0)
			{
				return BadRequest("No files uploaded.");
			}

			var uploadResults = new List<object>();

			foreach (var file in files)
			{
				using (var stream = file.OpenReadStream())
				{
					var fileName = file.FileName;
					var result = await _cloudinaryRepository.UploadImageAsync(stream, fileName);

					if (result.Error != null)
					{
						return BadRequest(result.Error.Message);
					}

					uploadResults.Add(new { result.PublicId, result.SecureUrl });
				}
			}

			return Ok(uploadResults);
		}

		[HttpDelete("{publicId}")]
		public async Task<IActionResult> DeleteImage(string publicId)
		{
			var deleteResult = await _cloudinaryRepository.DeleteImageAsync(publicId);
			if (deleteResult.Result.ToLower().Equals("ok"))
			{
				return Ok(deleteResult.Result.ToString());
			}
			else
			{
				return BadRequest(deleteResult.Result.ToString());
			}
		}
	}
}

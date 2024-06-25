using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.YoutubeRepository;

namespace MovieBooker_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VideoController : ControllerBase
	{
		public readonly IYoutubeRepository YoutubeRepository;

		public VideoController(IYoutubeRepository youtubeRepository)
		{
			YoutubeRepository = youtubeRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await YoutubeRepository.Get());
		}
	}
}

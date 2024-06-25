using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.YoutubeRepository
{
	public interface IYoutubeRepository
	{
		public Task<List<VideoDetail>> Get();
	}
}

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using MovieBooker_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieBooker_backend.Repositories.YoutubeRepository
{
	public class YoutubeRepository : IYoutubeRepository
	{
		private readonly IConfiguration _config;

		public YoutubeRepository(IConfiguration config)
		{
			_config = config;
		}

		public async Task<List<VideoDetail>> Get()
		{
			var youtubeSettings = _config.GetSection("YoutubeSetting").Get<YoutubeSetting>();
			var youtubeService = new YouTubeService(new BaseClientService.Initializer
			{
				ApiKey = youtubeSettings.ApiKey,
				ApplicationName = youtubeSettings.ApplicationName
			});

			var request = youtubeService.Search.List("snippet");
			request.ChannelId = youtubeSettings.ChannelId;
			request.Order = SearchResource.ListRequest.OrderEnum.Date;
			request.MaxResults = 50;
			var response = await request.ExecuteAsync();

			var videos = new List<VideoDetail>();

			foreach (var item in response.Items)
			{
				string videoId = item.Id.VideoId;
				string embedLink = $"https://www.youtube.com/embed/{videoId}";

				var videoDetail = new VideoDetail
				{
					Title = item.Snippet.Title,
					Link = $"https://www.youtube.com/watch?v={videoId}",
					Thumbnail = item.Snippet.Thumbnails.Medium.Url,
					PublishedAt = item.Snippet.PublishedAt.Value,
					LinkIframe = embedLink
				};

				videos.Add(videoDetail);
			}
			videos = videos.OrderByDescending(x => x.PublishedAt).ToList();

			return videos;
		}
	}
}

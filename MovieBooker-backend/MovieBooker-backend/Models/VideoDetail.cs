namespace MovieBooker_backend.Models
{
	public class VideoDetail
	{
		public string? Title { get; set; }
		public string? Link { get; set; }
		public string? Thumbnail { get; set; }
		public DateTimeOffset? PublishedAt { get; set; }
		public string? LinkIframe { get; set; }
	}
}

namespace MovieBooker.Dtos
{
	public class VideoDetailDto
	{
		public string? Title { get; set; }
		public string? Link { get; set; }
		public string? Thumbnail { get; set; }
		public DateTimeOffset? PublishedAt { get; set; }
		public string? LinkIframe { get; set; }
	}
}

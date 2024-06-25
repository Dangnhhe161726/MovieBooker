using MovieBooker_backend.Responses;

namespace MovieBooker_backend.DTO
{
	public class MovieDto
	{
		public MovieDto()
		{
			MovieImages = new HashSet<MovieImageResponse>();
		}

		public int MovieId { get; set; }
		public string? MovieTitle { get; set; }
		public string? Description { get; set; }
		public double? Price { get; set; }
		public string? Director { get; set; }
		public string? Durations { get; set; }
		public string? Trailer { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public int? CategoryId { get; set; }
		public bool? Enable { get; set; }
		public int? StatusId { get; set; }
		public virtual ICollection<MovieImageResponse> MovieImages { get; set; }
	}
}

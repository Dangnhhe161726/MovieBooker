namespace MovieBooker.Dtos
{
	public class MovieImageDto
	{
		public int Id { get; set; }
		public int? MovieId { get; set; }
		public string? LinkImage { get; set; } = null;
		public string? PublicId { get; set; }
	}
}

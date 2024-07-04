using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieImageRepository
{
	public interface IMovieImageRepository
	{
		public List<MovieImageResponse> getAll();
		public MovieImageResponse insert(MovieImageResponse newImage);
		public MovieImageResponse update(int id, MovieImageResponse updateImage);
		public void deleteById(int id);
	}
}

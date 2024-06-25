using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieImageRepository
{
	public interface IMovieImageRepository
	{
		public List<MovieImageResponse> getAll();
		public List<MovieImageResponse> insert(List<MovieImageResponse> newImage);
		public MovieImageResponse update(int id, MovieImageResponse updateImage);
		public void deleteById(int id);
	}
}

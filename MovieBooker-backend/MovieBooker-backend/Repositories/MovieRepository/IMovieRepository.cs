using MovieBooker_backend.DTO;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieRepository
{
	public interface IMovieRepository
	{
		public List<MovieResponse> getAll();

		public MovieResponse getById(int id);

		public MovieResponse update(MovieDto updateMovie);
	}
}

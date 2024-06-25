using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieStatusRepository
{
	public interface IMovieStatusRepository
	{
		public List<MovieStatusResponse> getAll();
		public MovieStatusResponse insert(MovieStatusResponse newStatus);
		public MovieStatusResponse update(int id, MovieStatusResponse updateStatus);
		public void deleteById(int id);
	}
}

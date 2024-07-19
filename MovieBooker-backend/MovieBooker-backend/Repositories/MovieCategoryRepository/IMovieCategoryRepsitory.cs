using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieCategoryRepository
{
	public interface IMovieCategoryRepsitory
	{
		public List<MovieCategoryResponse> getAll();
		public MovieCategoryResponse insert(MovieCategoryResponse newCategory);
		public MovieCategoryResponse update(int id, MovieCategoryResponse updateCategory);
		public void deleteById(int id);

	}
}

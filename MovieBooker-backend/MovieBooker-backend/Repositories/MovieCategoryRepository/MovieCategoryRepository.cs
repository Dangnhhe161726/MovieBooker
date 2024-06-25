using AutoMapper;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieCategoryRepository
{
	public class MovieCategoryRepository : IMovieCategoryRepsitory
	{
		private readonly bookMovieContext _context;
		private readonly IMapper _mapper;
		public MovieCategoryRepository(bookMovieContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void deleteById(int id)
		{
			var existingCategory = _context.MovieCategories.FirstOrDefault(c => c.CategoryId == id);
			if (existingCategory == null) throw new Exception("Not Found");
			_context.MovieCategories.Remove(existingCategory);
			_context.SaveChanges();
		}

		public List<MovieCategoryResponse> getAll()
		{
			var categories = _context.MovieCategories.ToList();
			return _mapper.Map<List<MovieCategoryResponse>>(categories);
		}

		public MovieCategoryResponse insert(MovieCategoryResponse newCategory)
		{
			var category = new MovieCategory
			{
				CategoryId = newCategory.CategoryId,
				CategoryName = newCategory.CategoryName
			};
			_context.MovieCategories.Add(category);
			_context.SaveChanges();
			return newCategory;
		}

		public MovieCategoryResponse update(int id, MovieCategoryResponse updateCategory)
		{
			var existingCategory = _context
				.MovieCategories
				.FirstOrDefault(c => c.CategoryId == id);
			if (existingCategory == null) throw new Exception("Not Found");
			existingCategory.CategoryName = updateCategory.CategoryName;
			_context.MovieCategories.Update(existingCategory);
			_context.SaveChanges();
			return updateCategory;
		}
	}
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;
using System.Collections.Generic;
using System.Linq;

namespace MovieBooker_backend.Repositories.MovieRepository
{
	public class MovieRepository : IMovieRepository
	{
		private readonly bookMovieContext _context;
		private readonly IMapper _mapper;

		public MovieRepository(bookMovieContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public List<MovieResponse> getAll()
		{
			var movies = _context.Movies
				.Include(m => m.MovieImages)
				.Include(m => m.Category)
				.Include(m => m.Status)
				.ToList();
			var movieResponses = _mapper.Map<List<MovieResponse>>(movies);
			return movieResponses;
		}

		public MovieResponse getById(int id)
		{
			var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
			return _mapper.Map<MovieResponse>(movie);
		}

		public MovieResponse update(MovieDto updateMovie)
		{
			var existingMovie = _context.Movies.FirstOrDefault(m => m.MovieId == updateMovie.MovieId);
			if (existingMovie == null) throw new Exception("Not found movie by id");
			existingMovie.MovieId = updateMovie.MovieId;
			existingMovie.MovieTitle = updateMovie.MovieTitle;
			existingMovie.Description = updateMovie.Description;
			existingMovie.Price = updateMovie.Price;
			existingMovie.Director = updateMovie.Director;
			existingMovie.Durations = updateMovie.Durations;
			existingMovie.Trailer = updateMovie.Trailer;
			existingMovie.ReleaseDate = updateMovie.ReleaseDate;
			existingMovie.CategoryId = updateMovie.CategoryId;
			var category = _context.MovieCategories.FirstOrDefault(c => c.CategoryId == updateMovie.CategoryId);
			if (category == null) throw new Exception("Not found category");
			existingMovie.Category = category;
			existingMovie.Enable = updateMovie.Enable;
			existingMovie.StatusId = updateMovie.StatusId;
			var status = _context.MovieStatuses.FirstOrDefault(s => s.StatusId == updateMovie.StatusId);
			if (status == null) throw new Exception("Not found status");
			existingMovie.Status = status;
			return _mapper.Map<MovieResponse>(existingMovie);
		}
	}
}

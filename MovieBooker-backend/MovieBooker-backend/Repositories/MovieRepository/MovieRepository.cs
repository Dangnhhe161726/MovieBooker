using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;
using System.Collections.Generic;
using System.IO;
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

		public void deleteById(int id)
		{
			var existingMovie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
			if (existingMovie != null)
			{
				if ((bool)existingMovie.Enable)
				{
					existingMovie.Enable = false;
				}
				else
				{
					existingMovie.Enable = true;
				}
				_context.Movies.Update(existingMovie);
				_context.SaveChanges();
			}
			else
			{
				throw new Exception("Movie not found");
			}
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

		public MovieResponse insert(MovieDto movieDto)
		{
			var movie = new Movie
			{
				MovieTitle = movieDto.MovieTitle,
				Description = movieDto.Description,
				Price = movieDto.Price,
				Director = movieDto.Director,
				Durations = movieDto.Durations,
				Trailer = movieDto.Trailer,
				ReleaseDate = movieDto.ReleaseDate,
				CategoryId = movieDto.CategoryId,
				Enable = true,
				StatusId = movieDto.StatusId
			};
			if (movie == null) throw new Exception("Movie not mapped");
			var category = _context.MovieCategories.FirstOrDefault(c => c.CategoryId == movie.CategoryId);
			if (category == null) throw new Exception("Not found category");
			var status = _context.MovieStatuses.FirstOrDefault(s => s.StatusId == movie.StatusId);
			if (status == null) throw new Exception("Not found status");
			movie.Status = status;
			movie.Category = category;
			_context.Movies.Add(movie);
			_context.SaveChanges();
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
			existingMovie.StatusId = updateMovie.StatusId;
			var status = _context.MovieStatuses.FirstOrDefault(s => s.StatusId == updateMovie.StatusId);
			if (status == null) throw new Exception("Not found status");
			existingMovie.Status = status;
			_context.Movies.Update(existingMovie);
			_context.SaveChanges();
			return _mapper.Map<MovieResponse>(existingMovie);
		}
	}
}

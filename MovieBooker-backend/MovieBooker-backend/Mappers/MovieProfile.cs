using AutoMapper;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;
namespace MovieBooker_backend.Mappers
{
	public class MovieProfile : Profile
	{
		public MovieProfile()
		{
			CreateMap<Movie, MovieResponse>();
			CreateMap<MovieImage, MovieImageResponse>();
			CreateMap<MovieCategory, MovieCategoryResponse>();
			CreateMap<MovieStatus, MovieStatusResponse>();
		}
	}
}

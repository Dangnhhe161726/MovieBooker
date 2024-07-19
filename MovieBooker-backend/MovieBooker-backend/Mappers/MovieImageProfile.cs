using AutoMapper;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Mappers
{
	public class MovieImageProfile : Profile
	{
		public MovieImageProfile()
		{
			CreateMap<MovieImage, MovieImageResponse>();
		}
	}
}

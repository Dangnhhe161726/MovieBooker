using AutoMapper;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Mappers
{
	public class MovieStatusProfile : Profile
	{
		public MovieStatusProfile()
		{
			CreateMap<MovieStatus, MovieStatusResponse>();
		}
	}
}

using AutoMapper;
using MovieBooker_backend.Models;
using MovieBooker_backend.Responses;

namespace MovieBooker_backend.Repositories.MovieStatusRepository
{
	public class MovieStatusRepository : IMovieStatusRepository
	{
		private readonly bookMovieContext _context;
		private readonly IMapper _mapper;

		public MovieStatusRepository(bookMovieContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void deleteById(int id)
		{
			var existingStatus = _context.MovieStatuses.FirstOrDefault(s => s.StatusId == id);
			if (existingStatus == null) throw new Exception("Not found");
			_context.MovieStatuses.Remove(existingStatus);
			_context.SaveChanges();
		}

		public List<MovieStatusResponse> getAll()
		{
			var statuses = _context.MovieStatuses.ToList();
			return _mapper.Map<List<MovieStatusResponse>>(statuses);
		}

		public MovieStatusResponse insert(MovieStatusResponse newStatus)
		{
			var status = new MovieStatus
			{
				StatusId = newStatus.StatusId,
				StatusName = newStatus.StatusName
			};
			_context.MovieStatuses.Add(status);
			_context.SaveChanges();
			return newStatus;
		}

		public MovieStatusResponse update(int id, MovieStatusResponse updateStatus)
		{
			var existingStatus = _context.MovieStatuses.FirstOrDefault(s => s.StatusId == id);
			if (existingStatus == null) throw new Exception("Not found");
			existingStatus.StatusName = updateStatus.StatusName;
			_context.MovieStatuses.Update(existingStatus);
			_context.SaveChanges();
			return updateStatus;
		}
	}
}

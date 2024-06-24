using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.ScheduleRepository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly bookMovieContext _context;
        public ScheduleRepository(bookMovieContext context)
        {
            _context = context;
        }
        public IEnumerable<ScheduleDTO> GetSchedules()
        {
            var schedules = _context.Schedules.Include(s => s.Movie).Include(s => s.TimeSlot)
                .Select(s => new ScheduleDTO
                {
                    SchedulesId = s.SchedulesId, 
                    MovieId = s.MovieId,
                    TheaterId = s.TheaterId,
                    TimeSlotId = s.TimeSlotId,
                    ScheduleDate = s.ScheduleDate,
                    MovieTitle = s.Movie.MovieTitle,
                    Durations = s.Movie.Durations
                    //TimeSlot = s.TimeSlot,
                }).ToList();
            return schedules;
        }
    }
}

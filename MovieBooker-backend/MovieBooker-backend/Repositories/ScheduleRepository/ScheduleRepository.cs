using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using System.Globalization;

namespace MovieBooker_backend.Repositories.ScheduleRepository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly bookMovieContext _context;
        public ScheduleRepository(bookMovieContext context)
        {
            _context = context;
        }

        public void AddSchedule(CreateScheduleDTO sc)
        {
            var schedule = new Schedule
            {
                MovieId = sc.MovieId,
                TheaterId = sc.TheaterId,
                TimeSlotId = sc.TimeSlotId,
                ScheduleDate = sc.ScheduleDate,
            };
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }

        public bool CheckExistSchedule(int movieId, int theaterId, int timeSlotId, string date)
        {
            DateTime targetDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var schedule = _context.Schedules
                .Where(s => s.MovieId == movieId
                            && s.TheaterId == theaterId
                            && s.TimeSlotId == timeSlotId
                            && s.ScheduleDate.HasValue
                            && s.ScheduleDate.Value.Date == targetDate.Date)
                .FirstOrDefault();

            if (schedule == null)
            {
                return true;
            }
            return false;
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

using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.TimeSlotRepository
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly bookMovieContext _context;
        public TimeSlotRepository(bookMovieContext context)
        {
            _context = context;
        }
        public IEnumerable<TimeSlotDTO> GetAllTimeSlot()
        { 
            var timeSlots = _context.TimeSlots.Select(t => new TimeSlotDTO
            {
                TimeSlotId = t.TimeSlotId,
                StartTime = t.StartTime,
            }).ToList();
            return timeSlots;
        }

        public TimeSlotDTO GetTimeSlotById(int id)
        {
            var timeSlots = _context.TimeSlots.Select(t => new TimeSlotDTO
            {
                TimeSlotId = t.TimeSlotId,
                StartTime = t.StartTime,
            }).FirstOrDefault(t => t.TimeSlotId == id);
            return timeSlots;
        }
    }
}

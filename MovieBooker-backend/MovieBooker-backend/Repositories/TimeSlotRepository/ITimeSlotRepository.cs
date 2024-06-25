using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.TimeSlotRepository
{
    public interface ITimeSlotRepository
    {
        public IEnumerable<TimeSlotDTO> GetAllTimeSlot();
    }
}

using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.ScheduleRepository
{
    public interface IScheduleRepository
    {
        public IEnumerable<ScheduleDTO> GetSchedules();

        public bool CheckExistSchedule(int theaterId, int timeSlotId, string date, int roomId);

        public void AddSchedule(CreateScheduleDTO schedule);


    }
}

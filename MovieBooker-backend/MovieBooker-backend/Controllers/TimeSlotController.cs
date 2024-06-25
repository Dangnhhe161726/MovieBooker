using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.ScheduleRepository;
using MovieBooker_backend.Repositories.TimeSlotRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotRepository _timeSlotRepository;

        public TimeSlotController(ITimeSlotRepository timeSlotRepository)
        {
            _timeSlotRepository = timeSlotRepository;
        }

        [HttpGet("GetAllTimeSlot")]
        public IActionResult Get()
        {
            var timeslot = _timeSlotRepository.GetAllTimeSlot();
            return Ok(timeslot);
        }
    }
}

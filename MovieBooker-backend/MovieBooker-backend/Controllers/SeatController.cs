using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.ScheduleRepository;
using MovieBooker_backend.Repositories.SeatRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;

        public SeatController(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }
        [HttpGet("GetSeat")]
        public IActionResult Get()
        {
            var seat = _seatRepository.GetSeat();
            return Ok(seat);
        }
    }
}

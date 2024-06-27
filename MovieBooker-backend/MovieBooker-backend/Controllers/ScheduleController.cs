using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Repositories.ScheduleRepository;
using MovieBooker_backend.Repositories.UserRepository;
using StackExchange.Redis;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
       
        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;         
        }

        [EnableQuery]
        [HttpGet("GetSchedule")]
        public IActionResult Get()
        {
           var schedules =  _scheduleRepository.GetSchedules();
           return Ok(schedules);
        }

        [HttpGet("CheckExistSchedule/{movieId}/{theaterId}/{timeSlotId}/{date}")]
        public IActionResult CheckExistSchedule(int movieId, int theaterId, int timeSlotId, DateTime date) {
            string d = date.ToString("yyyy-MM-dd");

            var schedule = _scheduleRepository.CheckExistSchedule(movieId, theaterId, timeSlotId, d);
            if(schedule == true)
            {
                return NotFound(); 
            }
            return Ok();
      
        }

        [HttpPost("CreateSchedule")]
        public IActionResult Create(CreateScheduleDTO sc)
        {
            try
            {
                _scheduleRepository.AddSchedule(sc);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

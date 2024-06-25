using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
    }
}

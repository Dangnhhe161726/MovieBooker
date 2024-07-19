using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.TheaterRepository;
using MovieBooker_backend.Repositories.TimeSlotRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterRepository _theaterRepository;

        public TheaterController(ITheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }

        [HttpGet("GetAllTheater")]
        public ActionResult Get()
        {
            var theater = _theaterRepository.GetAllTheater();
            return Ok(theater);
        }
    }
}

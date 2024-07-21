using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.RoomRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        [HttpGet("GetRoom/{id}")]
        public IActionResult Get(int id)
        {
            var rooms = _roomRepository.GetRoomByTheaterId(id);
            return Ok(rooms);
        }
    }
}

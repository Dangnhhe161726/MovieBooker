using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.ReservationRepository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using MovieBooker_backend.Models;
using MovieBooker_backend.DTO;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ODataController
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        // Method uses OData query options for filtering, sorting, and pagination
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var reservations = _reservationRepository.GetAllReservation().AsQueryable();
            return Ok(reservations);
        }

        // Method to get a specific reservation by key
        [HttpGet("{key}")]
        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var reservation = _reservationRepository.GetReservationById(key);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpPost("AddReservation")]
        [EnableQuery]
        public async Task<IActionResult> AddReservation(Revervation res)
        {
            var result = await _reservationRepository.AddNewReservation(res);
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest("Failed to add reservation");
        }

        [HttpPost("CreateReservation")]
        public IActionResult CreateReservation(CreateReservationDTO createReservation)
        {
            try
            {
                _reservationRepository.CreateReservation(createReservation);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

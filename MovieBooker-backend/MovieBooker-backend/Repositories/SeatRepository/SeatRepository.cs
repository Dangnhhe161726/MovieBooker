using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.SeatRepository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly bookMovieContext _context;
        public SeatRepository(bookMovieContext context)
        {
            _context = context;
        }

        public IEnumerable<SeatDTO> GetSeat()
        {
          var seat = _context.Seats.Include(s => s.Room).Include(s => s.SeatType).Select(s => new SeatDTO
          {
              SeatId = s.SeatId,
              RoomId= s.RoomId,
              RoomName = s.Room.RoomNumber,
              SeatNumber = s.SeatNumber,
              Row = s.Row,
              Status = s.Status, 
              SeatTypeId = s.SeatTypeId,
              SeatTypeName = s.SeatType.TypeName,
          }).ToList();
            return seat;
        }
    }
}

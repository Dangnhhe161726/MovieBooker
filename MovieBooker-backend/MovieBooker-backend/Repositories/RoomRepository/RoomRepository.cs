using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly bookMovieContext _context;
        public RoomRepository(bookMovieContext context)
        {
            _context = context;
        }
        public IEnumerable<RoomDTO> GetRoomByTheaterId(int id)
        {
            var rooms = _context.Rooms.Include(s => s.Theater).Where(s => s.TheaterId == id).Select(s => new RoomDTO
            {
                TheaterId = s.TheaterId,
                RoomId = s.RoomId,
                RoomNumber = s.RoomNumber,
                TheaterName = s.Theater.TheaterName,
            }).ToList();
            return rooms;
        }
    }
}

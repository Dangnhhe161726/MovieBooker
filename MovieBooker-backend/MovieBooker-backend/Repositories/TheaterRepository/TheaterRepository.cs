using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.TheaterRepository
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly bookMovieContext _context;
        public TheaterRepository(bookMovieContext context)
        {
            _context = context;
        }
        public IEnumerable<TheaterDTO> GetAllTheater()
        {
            var theater = _context.Theaters.Select(t => new TheaterDTO
            {
                TheaterId = t.TheaterId, 
                TheaterName = t.TheaterName,
                Address = t.Address, 
                PhoneNumber = t.PhoneNumber,
            }).ToList();
            return theater;
        }
    }
}

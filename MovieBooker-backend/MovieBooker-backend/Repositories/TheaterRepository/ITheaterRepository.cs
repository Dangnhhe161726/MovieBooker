using MovieBooker_backend.DTO;

namespace MovieBooker_backend.Repositories.TheaterRepository
{
    public interface ITheaterRepository
    {
        public IEnumerable<TheaterDTO> GetAllTheater();
    }
}

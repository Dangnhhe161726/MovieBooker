using MovieBooker_backend.DTO;

namespace MovieBooker_backend.Repositories.SeatRepository
{
    public interface ISeatRepository
    {

        public IEnumerable<SeatDTO> GetSeat();
    }
}

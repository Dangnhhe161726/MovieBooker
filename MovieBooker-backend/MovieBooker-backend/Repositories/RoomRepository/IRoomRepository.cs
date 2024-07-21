using MovieBooker_backend.DTO;

namespace MovieBooker_backend.Repositories.RoomRepository
{
    public interface IRoomRepository
    {
        public IEnumerable<RoomDTO> GetRoomByTheaterId(int id);
    }
}

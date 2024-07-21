namespace MovieBooker_backend.DTO
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public int? TheaterId { get; set; }
        public string? TheaterName { get; set; }
        public string? RoomNumber { get; set; }
    }
}

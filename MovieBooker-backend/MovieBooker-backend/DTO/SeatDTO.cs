using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend.DTO
{
    public class SeatDTO
    {
        [Key]
        public int SeatId { get; set; }
        public int? RoomId { get; set; }
        public int? TheatersId { get; set; }
        public string? RoomName { get; set; }
        public string? SeatNumber { get; set; }
        public string? Row { get; set; }
        public bool? Status { get; set; }
        public int? SeatTypeId { get; set; }
        public string? SeatTypeName { get; set; }
    }
}

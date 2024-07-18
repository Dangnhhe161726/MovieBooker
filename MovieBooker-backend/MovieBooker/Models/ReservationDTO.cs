using MovieBooker.Models;

namespace MovieBooker.DTO
{
    public class ReservationDTO
    {

        public int ReservationId { get; set; }
        public int? UserId { get; set; }
        public int? SeatId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? MovieId { get; set; }
        public string? SeatNumber { get; set; }
        public string? RoomNumber { get; set; }
        public string? MovieTitle { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public DateTime? ReservationDate { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
        public string? SeatType { get; set; }
    }
}

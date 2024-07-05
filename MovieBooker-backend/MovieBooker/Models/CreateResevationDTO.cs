namespace MovieBooker.Models
{
    public class CreateResevationDTO
    {
        public int? UserId { get; set; }
        public int? MovieId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? SeatId { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool? Status { get; set; }
        public double? TotalAmount { get; set; }
    }
}

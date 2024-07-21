namespace MovieBooker.Models
{
    public class CreateScheduleDTO
    {
        public int? MovieId { get; set; }
        public int? TheaterId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int RoomId { get; set; }
    }
}

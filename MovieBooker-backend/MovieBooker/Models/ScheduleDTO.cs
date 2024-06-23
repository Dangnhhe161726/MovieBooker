namespace MovieBooker.Models
{
    public class ScheduleDTO
    {
        public int SchedulesId { get; set; }
        public int? MovieId { get; set; }
        public int? TheaterId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        //public Movie? Movie { get; set; }
        //public TimeSlot? TimeSlot { get; set; }
    }
}

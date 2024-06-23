using MovieBooker_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend.DTO
{
    public class ScheduleDTO
    {
        [Key]
        public int SchedulesId { get; set; }
        public int? MovieId { get; set; }
        public int? TheaterId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        //public  Movie? Movie { get; set; }
        //public TimeSlot? TimeSlot { get; set; }
    }
}

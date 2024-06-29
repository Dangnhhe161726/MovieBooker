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
        public string? MovieTitle { get; set; }
        public string? Durations { get; set; }
        public string? TimeSlot { get; set; }
        public string? TheaterName { get; set; }
        public string? movieImage { get; set; }
    }
}

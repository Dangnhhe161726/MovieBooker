using Microsoft.AspNetCore.Mvc;

namespace MovieBooker.Models
{
    public class TicketDTO
    {
        public string? start { get; set; }
        public int? theartr { get; set; }
        public int? timeslot { get; set; }
        public int? movieId { get; set; }
    }
}

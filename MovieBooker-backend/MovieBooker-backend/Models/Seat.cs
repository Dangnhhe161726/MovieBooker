using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Seat
    {
        public Seat()
        {
            Revervations = new HashSet<Revervation>();
        }

        public int SeatId { get; set; }
        public int? RoomId { get; set; }
        public string? SeatNumber { get; set; }

        public virtual Room? Room { get; set; }
        public virtual ICollection<Revervation> Revervations { get; set; }
    }
}

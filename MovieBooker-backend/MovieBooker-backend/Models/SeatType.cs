using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class SeatType
    {
        public SeatType()
        {
            Seats = new HashSet<Seat>();
        }

        public int SeatTypeId { get; set; }
        public string? TypeName { get; set; }
        public double? Price { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
    }
}

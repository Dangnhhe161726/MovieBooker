using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Room
    {
        public Room()
        {
            Seats = new HashSet<Seat>();
        }

        public int RoomId { get; set; }
        public int? TheaterId { get; set; }
        public string? RoomNumber { get; set; }
        public bool? Status { get; set; }

        public virtual Theater? Theater { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}

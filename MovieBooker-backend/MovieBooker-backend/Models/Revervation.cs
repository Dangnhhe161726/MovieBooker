using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Revervation
    {
        public int ReservationId { get; set; }
        public int? UserId { get; set; }
        public int? MovieId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? SeatId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public bool? Status { get; set; }
        public double? TotalAmount { get; set; }

        public virtual Movie? Movie { get; set; }
        public virtual Seat? Seat { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
        public virtual User? User { get; set; }
    }
}

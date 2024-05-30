using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? ReservationId { get; set; }
        public double? TotalAmount { get; set; }

        public virtual Revervation? Reservation { get; set; }
    }
}

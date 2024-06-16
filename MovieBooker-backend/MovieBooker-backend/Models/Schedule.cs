using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Schedule
    {
        public int SchedulesId { get; set; }
        public string? MovieId { get; set; }
        public int? TheaterId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Movie? Movie { get; set; }
        public virtual Theater? Theater { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}

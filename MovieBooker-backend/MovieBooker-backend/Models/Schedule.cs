using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Schedule
    {
        public int SchedulesId { get; set; }
        public int? MovieId { get; set; }
        public int? TheaterId { get; set; }
        public int? TimeSlotId { get; set; }
        public DateTime? ScheduleDate { get; set; }

        public virtual Movie? Movie { get; set; }
        public virtual Theater? Theater { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Theater
    {
        public Theater()
        {
            Rooms = new HashSet<Room>();
            Schedules = new HashSet<Schedule>();
        }

        public int TheaterId { get; set; }
        public string? TheaterName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}

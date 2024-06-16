using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Status
    {
        public Status()
        {
            Revervations = new HashSet<Revervation>();
        }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public virtual ICollection<Revervation> Revervations { get; set; }
    }
}

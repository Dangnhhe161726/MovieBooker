using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class MovieStatus
    {
        public MovieStatus()
        {
            Movies = new HashSet<Movie>();
        }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}

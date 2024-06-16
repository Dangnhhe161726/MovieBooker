using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class MovieCategory
    {
        public MovieCategory()
        {
            Movies = new HashSet<Movie>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}

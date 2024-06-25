using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class MovieImage
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public string? LinkImage { get; set; }

        public virtual Movie? Movie { get; set; }
    }
}

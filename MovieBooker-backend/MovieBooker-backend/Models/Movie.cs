using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieImages = new HashSet<MovieImage>();
            Schedules = new HashSet<Schedule>();
        }

        public string MovieId { get; set; } = null!;
        public string? MovieTitle { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? CategoryId { get; set; }
        public bool? Status { get; set; }

        public virtual MovieCategory? Category { get; set; }
        public virtual ICollection<MovieImage> MovieImages { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}

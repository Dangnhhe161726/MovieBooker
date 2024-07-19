using MovieBooker_backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend.Responses
{
    public partial class MovieResponse
    {
        public MovieResponse()
        {
            MovieImages = new HashSet<MovieImageResponse>();
        }
		[Key]
		public int MovieId { get; set; }
		public string? MovieTitle { get; set; }
		public string? Description { get; set; }
		public double? Price { get; set; }
		public string? Director { get; set; }
		public string? Durations { get; set; }
		public string? Trailer { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public int? CategoryId { get; set; }
		public bool? Enable { get; set; }
		public int? StatusId { get; set; }
		public virtual MovieStatusResponse? Status { get; set; }
		public virtual MovieCategoryResponse? Category { get; set; }
        public virtual ICollection<MovieImageResponse> MovieImages { get; set; }
    }
}

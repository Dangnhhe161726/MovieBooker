using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Responses
{
    public partial class MovieImageResponse
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public string? LinkImage { get; set; }
        public string? PublicId { get; set; }
    }
}

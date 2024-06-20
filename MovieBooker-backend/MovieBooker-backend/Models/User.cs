using System;
using System.Collections.Generic;

namespace MovieBooker_backend.Models
{
    public partial class User
    {
        public User()
        {
            Revervations = new HashSet<Revervation>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public int? RoleId { get; set; }
        public string? Avatar { get; set; }
        public bool? Status { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Revervation> Revervations { get; set; }
    }
}

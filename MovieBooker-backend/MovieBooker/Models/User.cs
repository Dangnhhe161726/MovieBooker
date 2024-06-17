using System;
using System.Collections.Generic;

namespace MovieBooker.Models
{
    public partial class User
    {
        public User()
        {
            
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public int? RoleId { get; set; }
    }
}

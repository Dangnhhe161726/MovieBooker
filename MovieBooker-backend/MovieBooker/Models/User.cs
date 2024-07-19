using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MovieBooker.Models
{
    public partial class User
    {
        public User()
        {
            From = "cgvcinema88@gmail.com";
            PasswordSendMail = "wiusmsuuotuekiyo";
        }
        public string From { get; set; }
        public string PasswordSendMail { get; set; }

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
        public string? RoleName { get; set; }
        public bool? Status { get; set; }
    }
}

using MovieBooker_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend.DTO
{
    public class UserDTO
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public bool? Status { get; set; }
        public Role? Role { get; set; }
        public int? RoleId { get; set; }
}
}

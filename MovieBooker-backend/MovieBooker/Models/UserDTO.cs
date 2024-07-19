using System.ComponentModel.DataAnnotations;

namespace MovieBooker.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public string? Avatar { get; set; }
        public bool? Status { get; set; }
    }
}

namespace MovieBooker.Models
{
    public class UpdateUserDTO
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public int? RoleId { get; set; }
    }
}

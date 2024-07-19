using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend
{
    public class SignUpModel
    {
        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public DateTime Dob { get; set; }

        public int Role = 3;
        public string? Avatar { get; set; }
        public bool Status = true;


    }
}

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
        public int Role = 3;
    }
}

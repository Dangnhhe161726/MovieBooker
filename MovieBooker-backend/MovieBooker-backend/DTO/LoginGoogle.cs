using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend.DTO
{
    public class LoginGoogle
    {
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public DateTime Dob { get; set; }

        public int Role = 3;
    }
}

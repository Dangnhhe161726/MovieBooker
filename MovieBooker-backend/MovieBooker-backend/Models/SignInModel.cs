using System.ComponentModel.DataAnnotations;

namespace MovieBooker_backend
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace JWT.DTO
{
    public class ApplicationUser : IdentityUser
    {
        public string? userName { get; set; } 
    }
}

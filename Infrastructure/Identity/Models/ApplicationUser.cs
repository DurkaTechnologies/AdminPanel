using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Infrastructure.Identity.Models
{
	public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; } = false;
    }
}

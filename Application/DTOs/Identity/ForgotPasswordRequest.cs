using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

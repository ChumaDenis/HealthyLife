using System.ComponentModel.DataAnnotations;

namespace HealthyLife.Models.Auth
{
    public class UserRegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; }= string.Empty;
        [Required, Compare("Password")]
        public string PasswordConfirmation { get; set; } = string.Empty;
    }
}

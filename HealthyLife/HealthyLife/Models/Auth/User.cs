using System.ComponentModel.DataAnnotations;

namespace HealthyLife.Models.Auth
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public string? VerifacationToken { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? PasswordRessetToken { get; set; }

        public DateTime? PasswordRessetExpires { get; set; }
    }
}

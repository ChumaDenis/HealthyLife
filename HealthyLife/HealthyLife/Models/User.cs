using System.ComponentModel.DataAnnotations;

namespace HealthyLife.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

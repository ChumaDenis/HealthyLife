namespace HealthyLife.Models
{
    public class Token
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Value { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string UserId { get; set; }
    }
}

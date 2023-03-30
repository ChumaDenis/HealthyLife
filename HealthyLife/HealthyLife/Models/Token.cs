namespace HealthyLife.Models
{
    public class Token
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }= DateTime.Now;
        public string UserId { get; set; }
    }
}

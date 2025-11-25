namespace StrivoLab.Model
{
    public class Services
    {
        public int Id { get; set; }
        public string ServiceId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Description { get; set; }
    }
}

namespace StrivoLab.Model
{
    public class TokenEntry
    {
        public int Id { get; set; }
        public string TokenId { get; set; } = null!;
        public string ServiceId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

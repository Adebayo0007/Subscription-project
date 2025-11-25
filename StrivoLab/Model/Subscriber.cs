namespace StrivoLab.Model
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string ServiceId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsSubscribed { get; set; }
        public DateTime? SubscribedAt { get; set; }
        public DateTime? UnsubscribedAt { get; set; }

    }
}

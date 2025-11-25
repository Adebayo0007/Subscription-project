namespace StrivoLab.DTOs
{
    public record SubscribeRequest(string service_id, string token_id, string phone_number);
    public record SubscribeResponse(string subscriptionId, string message);
    public record StatusResponse(string phone_number, bool isSubscribed, string? subscribedAt, string? unsubscribedAt);
}

namespace StrivoLab.DTOs
{
    public record LoginRequest(string service_id, string password);
    public record LoginResponse(string token_id, string expiresAt);
}

using StrivoLab.DTOs;

namespace StrivoLab.Service.Interfaces
{
    public interface ISubscriptionService
    {
        Task<(bool created, string message, string? subscriptionId)> SubscribeAsync(string serviceId, string phoneNumber);
        Task<(bool ok, string message)> UnsubscribeAsync(string serviceId, string phoneNumber);
        Task<StatusResponse> CheckStatusAsync(string serviceId, string phoneNumber);
    }
}

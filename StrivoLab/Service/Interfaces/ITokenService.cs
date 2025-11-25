using StrivoLab.DTOs;

namespace StrivoLab.Service.Interfaces
{
    public interface ITokenService
    {
        Task<LoginResponse?> LoginAsync(string serviceId, string password);
        Task<(bool ok, string reason)> ValidateTokenAsync(string serviceId, string tokenId);

    }
}

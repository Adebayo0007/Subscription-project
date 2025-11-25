using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StrivoLab.Data;
using StrivoLab.DTOs;
using StrivoLab.Model;
using StrivoLab.Service.Interfaces;
using System;

namespace StrivoLab.Service.Implementations
{
    public class TokenOptions
    {
        public int TokenValidityHours { get; set; } = 4;
    }
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _db;
        private readonly TokenOptions _options;

        public TokenService(AppDbContext db, IOptions<TokenOptions> options)
        {
            _db = db;
            _options = options.Value;
        }

        public async Task<LoginResponse?> LoginAsync(string serviceId, string password)
        {
            var svc = await _db.Services.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (svc == null) return null;
            if (svc.Password != password) return null;

            var now = DateTime.UtcNow;
            var existing = await _db.Tokens.FirstOrDefaultAsync(t => t.ServiceId == serviceId && t.ExpiresAt > now);
            if (existing != null)
            {
                return new LoginResponse(existing.TokenId, existing.ExpiresAt.ToString("o"));
            }

            var token = new TokenEntry
            {
                TokenId = Guid.NewGuid().ToString("N"),
                ServiceId = serviceId,
                CreatedAt = now,
                ExpiresAt = now.AddHours(_options.TokenValidityHours)
            };
            _db.Tokens.Add(token);
            await _db.SaveChangesAsync();

            return new LoginResponse(token.TokenId, token.ExpiresAt.ToString("o"));
        }

        public async Task<(bool ok, string reason)> ValidateTokenAsync(string serviceId, string tokenId)
        {
            var token = await _db.Tokens.FirstOrDefaultAsync(t => t.TokenId == tokenId && t.ServiceId == serviceId);
            if (token == null) return (false, "Wrong Token ID");
            if (token.ExpiresAt < DateTime.UtcNow) return (false, "Token ID expired");
            return (true, "OK");
        }
    }
}

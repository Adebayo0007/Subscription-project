using Microsoft.EntityFrameworkCore;
using StrivoLab.Data;
using StrivoLab.DTOs;
using StrivoLab.Model;
using StrivoLab.Service.Interfaces;
using System;

namespace StrivoLab.Service.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _db;

        public SubscriptionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(bool created, string message, string? subscriptionId)> SubscribeAsync(string serviceId, string phoneNumber)
        {
            var now = DateTime.UtcNow;
            var existing = await _db.Subscribers.FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.PhoneNumber == phoneNumber);

            if (existing != null && existing.IsSubscribed)
            {
                return (false, "User is already subscribed", null);
            }

            if (existing == null)
            {
                var sub = new Subscriber
                {
                    ServiceId = serviceId,
                    PhoneNumber = phoneNumber,
                    IsSubscribed = true,
                    SubscribedAt = now,
                    UnsubscribedAt = null
                };
                _db.Subscribers.Add(sub);
                await _db.SaveChangesAsync();
                return (true, "User subscribed", sub.Id.ToString());
            }
            else
            {
                existing.IsSubscribed = true;
                existing.SubscribedAt = now;
                existing.UnsubscribedAt = null;
                await _db.SaveChangesAsync();
                return (true, "User subscribed", existing.Id.ToString());
            }
        }

        public async Task<(bool ok, string message)> UnsubscribeAsync(string serviceId, string phoneNumber)
        {
            var existing = await _db.Subscribers.FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.PhoneNumber == phoneNumber);
            if (existing == null || !existing.IsSubscribed)
            {
                return (false, "User is not subscribed");
            }

            existing.IsSubscribed = false;
            existing.UnsubscribedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return (true, "User unsubscribed");
        }

        public async Task<StatusResponse> CheckStatusAsync(string serviceId, string phoneNumber)
        {
            var existing = await _db.Subscribers.FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.PhoneNumber == phoneNumber);
            if (existing == null)
            {
                return new StatusResponse(phoneNumber, false, null, null);
            }

            return new StatusResponse(
                phoneNumber,
                existing.IsSubscribed,
                existing.SubscribedAt?.ToString("o"),
                existing.UnsubscribedAt?.ToString("o"));
        }
    }
}

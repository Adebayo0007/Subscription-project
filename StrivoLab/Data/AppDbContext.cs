using Microsoft.EntityFrameworkCore;
using StrivoLab.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StrivoLab.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Services> Services { get; set; } = null!;
        public DbSet<TokenEntry> Tokens { get; set; } = null!;
        public DbSet<Subscriber> Subscribers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Services>().HasIndex(s => s.ServiceId).IsUnique();
            modelBuilder.Entity<TokenEntry>().HasIndex(t => t.TokenId).IsUnique();
            modelBuilder.Entity<Subscriber>().HasIndex(s => new { s.ServiceId, s.PhoneNumber }).IsUnique();

            // Seed example service (demo entry)
            modelBuilder.Entity<Services>().HasData(new Services
            {
                Id = 1,
                ServiceId = "demo-service",
                Password = "password123",
                Description = "Demo service created by seed data"
            });
        }
    }
}

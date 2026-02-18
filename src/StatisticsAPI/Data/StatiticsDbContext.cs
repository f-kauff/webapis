using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace StatisticsAPI.Data;

public class StatisticsDbContext : DbContext
{
    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options)
        : base(options) { }

    public DbSet<DeviceRegistration> DeviceRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceRegistration>().HasKey(r => new { r.UserKey, r.DeviceType });

        base.OnModelCreating(modelBuilder);
    }
}

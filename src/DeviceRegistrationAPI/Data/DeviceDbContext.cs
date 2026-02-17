using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DeviceRegistrationAPI.Data;

public class DeviceDbContext : DbContext
{
    public DeviceDbContext(DbContextOptions<DeviceDbContext> options)
        : base(options) { }

    public virtual DbSet<DeviceRegistration> DeviceRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceRegistration>().HasKey(r => new { r.UserKey, r.DeviceType });

        base.OnModelCreating(modelBuilder);
    }
}

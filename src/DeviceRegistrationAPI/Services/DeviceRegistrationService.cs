namespace DeviceRegistrationAPI.Services;

using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

public interface IDeviceRegistrationService
{
    Task<Result> RegisterDeviceAsync(string userKey, string deviceType);
}

public class DeviceRegistrationService : IDeviceRegistrationService
{
    private readonly DeviceDbContext _context;
    private readonly ILogger<DeviceRegistrationService> _logger;

    public DeviceRegistrationService(
        DeviceDbContext context,
        ILogger<DeviceRegistrationService> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> RegisterDeviceAsync(string userKey, string deviceType)
    {
        if (string.IsNullOrWhiteSpace(userKey) || string.IsNullOrWhiteSpace(deviceType))
        {
            return Result.Error("Missing required field: userKey and deviceType must be provided.");
        }

        if (
            await _context.DeviceRegistrations.AnyAsync(r =>
                r.UserKey == userKey && r.DeviceType == deviceType
            )
        )
        {
            _logger.LogWarning(
                "Duplicate registration attempt for userKey: {userKey}, deviceType: {deviceType}",
                userKey,
                deviceType
            );
            return Result.Successful(); // Idempotent response for duplicates
        }

        var registration = new DeviceRegistration
        {
            UserKey = userKey,
            DeviceType = deviceType,
            Timestamp = DateTime.UtcNow,
        };

        try
        {
            _context.DeviceRegistrations.Add(registration);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering device: {message}", ex.Message);
            return Result.Error("internal_error");
        }
        _logger.LogDebug(
            "Device registered for userKey: {userKey}, deviceType: {deviceType}",
            userKey,
            deviceType
        );

        return Result.Successful();
    }
}

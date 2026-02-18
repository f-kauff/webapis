using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using StatisticsAPI.Data;
using StatisticsAPI.Models;

namespace StatisticsAPI.Services;

public interface IStatisticsService
{
    Task<Result> LogAuthAsync(AuthLogInput input);
    Task<StatisticsOutput> GetStatisticsAsync(string deviceType);
}

public class StatisticsService : IStatisticsService
{
    private readonly StatisticsDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly ILogger<StatisticsService> _logger;
    private readonly IConfiguration _configuration;

    public StatisticsService(
        StatisticsDbContext context,
        HttpClient httpClient,
        ILogger<StatisticsService> logger,
        IConfiguration configuration
    )
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result> LogAuthAsync(AuthLogInput input)
    {
        if (string.IsNullOrWhiteSpace(input.UserKey) || string.IsNullOrWhiteSpace(input.DeviceType))
        {
            return Result.Error("Missing required field: userKey and deviceType must be provided.");
        }

        try
        {
            var DeviceRegistrationApiUrl = _configuration["DeviceRegistrationApiUrl"];
            if (string.IsNullOrEmpty(DeviceRegistrationApiUrl))
            {
                _logger.LogError("DeviceRegistrationApiUrl is not configured.");
                return Result.Error("internal_error");
            }

            var content = new StringContent(
                JsonSerializer.Serialize(input),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync(
                $"{DeviceRegistrationApiUrl}/Device/register",
                content
            );

            if (response.IsSuccessStatusCode)
            {
                return Result.Successful();
            }
            else
            {
                _logger.LogWarning(
                    "DeviceRegistrationAPI returned unsuccessful status code: {StatusCode}, {ReasonPhrase}",
                    response.StatusCode,
                    response.ReasonPhrase
                );
                return Result.Error("bad_request");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing auth log: {errorMessage}", ex.Message);
            return Result.Error("internal_error");
        }
    }

    public async Task<StatisticsOutput> GetStatisticsAsync(string deviceType)
    {
        if (string.IsNullOrWhiteSpace(deviceType))
        {
            return new StatisticsOutput { DeviceType = "", Count = -1 };
        }

        try
        {
            var count = await _context.DeviceRegistrations.CountAsync(r =>
                r.DeviceType == deviceType
            );

            return new StatisticsOutput
            {
                DeviceType = deviceType,
                Count = count == 0 ? -1 : count, // Return -1 if no device registration found
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving statistics {errorMessage}", ex.Message);
            return new StatisticsOutput { DeviceType = deviceType, Count = -1 };
        }
    }
}

using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using RichardSzalay.MockHttp;
using Shared.Models;
using StatisticsAPI.Data;
using StatisticsAPI.Models;
using StatisticsAPI.Services;
using Xunit;

namespace StatisticsAPI.UnitTests;

public class StatisticsServiceTests
{
    private readonly Mock<StatisticsDbContext> _contextMock;
    private readonly Mock<ILogger<StatisticsService>> _loggerMock;
    private readonly StatisticsService _service;
    private readonly MockHttpMessageHandler _httpMock;
    private readonly Mock<IConfiguration> _configurationMock;

    public StatisticsServiceTests()
    {
        //mock database context
        var options = new DbContextOptionsBuilder<StatisticsDbContext>().Options;
        _contextMock = new Mock<StatisticsDbContext>(options);
        //mock logger context
        _loggerMock = new Mock<ILogger<StatisticsService>>();
        //mock http client
        _httpMock = new MockHttpMessageHandler();

        //mock configuration
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["DeviceRegistrationApiUrl"]).Returns("http://dummy-url");

        //initialize service with mocked dependencies
        _service = new StatisticsService(
            _contextMock.Object,
            _httpMock.ToHttpClient(),
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [Fact]
    public async Task LogAuth_ReturnsBadRequest_WhenBothInputsAreEmpty()
    {
        var result = await _service.LogAuthAsync(
            new AuthLogInput { UserKey = "", DeviceType = "" }
        );
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task LogAuth_ReturnsBadRequest_WhenUserKeyIsEmpty()
    {
        var result = await _service.LogAuthAsync(
            new AuthLogInput { UserKey = "", DeviceType = "Android" }
        );
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task LogAuth_ReturnsBadRequest_WhenDeviceTypeIsEmpty()
    {
        var result = await _service.LogAuthAsync(
            new AuthLogInput { UserKey = "user1", DeviceType = "" }
        );
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task LogAuth_ReturnsSuccess_WhenInputIsValid()
    {
        // Arrange
        var input = new AuthLogInput { UserKey = "user1", DeviceType = "Android" };

        _httpMock
            .When(HttpMethod.Post, "/Device/register")
            .Respond(HttpStatusCode.OK, "application/json", "{'statusCode' : 'OK'}");

        // Act
        var result = await _service.LogAuthAsync(input);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task LogAuth_ReturnsBadRequest_WhenErrorOccurs()
    {
        // Arrange
        var input = new AuthLogInput { UserKey = "user1", DeviceType = "Android" };

        _httpMock
            .When(HttpMethod.Post, "/Device/register")
            .Respond(
                HttpStatusCode.BadRequest,
                "application/json",
                "{'statusCode' : 400, 'message' : 'Bad Request'}"
            );

        // Act
        var result = await _service.LogAuthAsync(input);

        // Assert
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public async Task GetStatistics_ReturnsCount_WhenDataExists()
    {
        // Arrange
        var registrations = new List<DeviceRegistration>
        {
            new DeviceRegistration { UserKey = "user1", DeviceType = "Android" },
            new DeviceRegistration { UserKey = "user2", DeviceType = "Android" },
            new DeviceRegistration { UserKey = "user3", DeviceType = "iOS" },
        };
        _contextMock.Setup(c => c.DeviceRegistrations).ReturnsDbSet(registrations);

        // Act
        var stats = await _service.GetStatisticsAsync("Android");

        // Assert
        Assert.Equal("Android", stats.DeviceType);
        Assert.Equal(2, stats.Count);
    }
}

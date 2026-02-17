using DeviceRegistrationAPI.Data;
using DeviceRegistrationAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Shared.Models;
using Xunit;

namespace DeviceRegistrationAPI.UnitTests;

public class DeviceRegistrationServiceTests
{
    private readonly Mock<DeviceDbContext> _contextMock;
    private readonly Mock<ILogger<DeviceRegistrationService>> _loggerMock;
    private readonly DeviceRegistrationService _service;

    public DeviceRegistrationServiceTests()
    {
        //mock database context
        var options = new DbContextOptionsBuilder<DeviceDbContext>().Options;
        _contextMock = new Mock<DeviceDbContext>(options);
        //mock logger context
        _loggerMock = new Mock<ILogger<DeviceRegistrationService>>();
        //initialize service with mocked dependencies
        _service = new DeviceRegistrationService(_contextMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenBothInputsAreEmpty()
    {
        var result = await _service.RegisterDeviceAsync("", "");
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUserKeyIsEmpty()
    {
        var result = await _service.RegisterDeviceAsync("", "Android");
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenDeviceTypeIsEmpty()
    {
        var result = await _service.RegisterDeviceAsync("user1", "");
        Assert.False(result.IsSuccessful);
        Assert.Equal(
            "Missing required field: userKey and deviceType must be provided.",
            result.ErrorMessage
        );
    }

    [Fact]
    public async Task Register_ReturnsSuccess_WhenInputIsValid()
    {
        // Arrange
        var registrations = new List<DeviceRegistration>();
        _contextMock.Setup(c => c.DeviceRegistrations).ReturnsDbSet(registrations);

        // Act
        var result = await _service.RegisterDeviceAsync("user1", "Android");

        // Assert
        Assert.True(result.IsSuccessful);
        _contextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task Register_ReturnsSuccess_WhenDuplicateExists()
    {
        // Arrange
        var registrations = new List<DeviceRegistration>
        {
            new DeviceRegistration { UserKey = "user1", DeviceType = "Android" },
        };
        _contextMock.Setup(c => c.DeviceRegistrations).ReturnsDbSet(registrations);

        // Act
        var result = await _service.RegisterDeviceAsync("user1", "Android");

        // Assert
        Assert.True(result.IsSuccessful);
        _contextMock.Verify(m => m.SaveChangesAsync(), Times.Never());
    }
}

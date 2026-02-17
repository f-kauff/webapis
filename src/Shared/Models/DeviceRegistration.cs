using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class DeviceRegistration
{
    [Required]
    public string UserKey { get; set; } = string.Empty;

    [Required]
    public string DeviceType { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

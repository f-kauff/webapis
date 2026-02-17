using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

class DeviceRegistration
{
    [Required]
    public string UserKey { get; set; }

    [Required]
    public string DeviceType { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

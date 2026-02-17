namespace DeviceRegistrationAPI.Models;

public class DeviceRegistrationInput
{
    public string UserKey { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
}

namespace XeGo.Services.Notifications.Functions.Models
{
    public class TwilioSmsRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
    }
}

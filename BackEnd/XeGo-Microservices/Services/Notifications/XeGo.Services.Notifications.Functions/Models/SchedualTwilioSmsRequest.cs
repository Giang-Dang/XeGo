namespace XeGo.Services.Notifications.Functions.Models
{
    public class SchedualTwilioSmsRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? SendTime { get; set; }
    }
}

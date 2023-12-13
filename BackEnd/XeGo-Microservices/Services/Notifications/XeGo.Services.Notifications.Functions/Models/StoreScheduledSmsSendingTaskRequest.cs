namespace XeGo.Services.Notifications.Functions.Models
{

    public class StoreScheduledSmsSendingTaskRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string? SendTime { get; set; }
    }
}

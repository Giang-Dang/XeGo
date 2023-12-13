using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Data;
using XeGo.Services.Notifications.Functions.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public class SendSmsTimeTrigger(ILoggerFactory loggerFactory, AppDbContext db)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<SendSmsTimeTrigger>();

        [Function(FuncConst.SendSmsTimeTrigger)]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            var utcNow = DateTime.UtcNow;
            _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} function executed at: {utcNow}");

            try
            {
                var scheduledTasks = await db.ScheduledTasks
                    .Where(t =>
                        t.ExecutionUtcYear == utcNow.Year &&
                        t.ExecutionUtcMonth == utcNow.Month &&
                        t.ExecutionUtcDay == utcNow.Day &&
                        t.ExecutionUtcHour == utcNow.Hour &&
                        t.ExecutionUtcMinute == utcNow.Minute)
                    .ToListAsync();

                _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} scheduledTasks.Count: {scheduledTasks.Count}");

                foreach (var task in scheduledTasks)
                {
                    _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} > task.Id={task.Id}: Executing.");

                    if (task.Json == null)
                    {
                        _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} > {task.Id}: task.Json null!");
                        continue;
                    }

                    var data = JsonConvert.DeserializeObject<StoreScheduledSmsSendingTaskRequest>(task.Json);
                    if (data == null)
                    {
                        _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} > {task.Id}: data null! (DeserializeObject failed!)");
                        continue;
                    }

                    if (data.Message == null || data.PhoneNumber == null)
                    {
                        _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} > {task.Id}: data.Message == null || data.PhoneNumber == null");
                        continue;
                    }
                    SendSms(data.Message, data.PhoneNumber);
                }
                _logger.LogInformation($"{FuncConst.SendSmsTimeTrigger} function started at {utcNow}. Completed at: {DateTime.UtcNow}");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void SendSms(string textMessage, string toPhoneNumber)
        {
            try
            {
                var accountSid = Environment.GetEnvironmentVariable("TwilioAccountSid");
                var authToken = Environment.GetEnvironmentVariable("TwilioAuthToken");
                var twilioPhoneNumber = Environment.GetEnvironmentVariable("TwilioPhoneNumber");

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: textMessage,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );

                _logger.LogInformation($"Message {message.Sid} sent: to {toPhoneNumber} message {textMessage}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Message sent: to {toPhoneNumber} message {textMessage} Failed! {e.Message}");
            }

        }
    }
}

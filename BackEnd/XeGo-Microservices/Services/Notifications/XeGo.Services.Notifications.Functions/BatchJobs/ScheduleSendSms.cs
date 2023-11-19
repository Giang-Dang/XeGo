using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Models;

namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public static class ScheduleSendSms
    {
        [Function(FuncNameConst.StartScheduleSendSms)]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [Microsoft.Azure.Functions.Worker.DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            Microsoft.Extensions.Logging.ILogger logger = executionContext.GetLogger("Function_HttpStart");

            // Function input comes from the request content.
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<SchedualTwilioSmsRequest>(requestBody);

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(FuncNameConst.ScheduleSendSms, data);

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return client.CreateCheckStatusResponse(req, instanceId);
        }



        [Function(FuncNameConst.ScheduleSendSms)]
        public static async Task RunOrchestrator(
            [Microsoft.Azure.Functions.Worker.OrchestrationTrigger] TaskOrchestrationContext context)
        {
            Microsoft.Extensions.Logging.ILogger logger = context.CreateReplaySafeLogger(nameof(ScheduleSendSms));

            try
            {
                var data = context.GetInput<SchedualTwilioSmsRequest>();
                if (data == null)
                {
                    logger.LogError("Missing request data.");
                    throw new ArgumentNullException("Request data is null.");
                }

                logger.LogInformation($"Executing Function {FuncNameConst.ScheduleSendSms}: Schedule sending sms on {data.SendTime}");

                if (string.IsNullOrEmpty(data.PhoneNumber) || string.IsNullOrEmpty(data.Message) || string.IsNullOrEmpty(data.SendTime))
                {
                    logger.LogError("Missing required parameters in the request body.");
                    throw new ArgumentNullException("Please pass a phoneNumber, message, and sendTime in the request body");
                }

                DateTime sendTime = DateTime.Parse(data.SendTime);

                await context.CreateTimer(sendTime, CancellationToken.None);

                var dataJson = JsonConvert.SerializeObject(data);

                await context.CallActivityAsync<string>(FuncNameConst.SendSmsFunction, dataJson);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while orchestrating the SMS sending: {ex.Message}");
                throw;
            }
        }


        [Function(FuncNameConst.SendSmsFunction)]
        public static async Task SendSmsFunction([Microsoft.Azure.Functions.Worker.ActivityTrigger] string dataJson, FunctionContext executionContext)
        {
            Microsoft.Extensions.Logging.ILogger logger = executionContext.GetLogger(FuncNameConst.SendSmsFunction);

            try
            {
                var accountSid = Environment.GetEnvironmentVariable("TwilioAccountSid");
                var authToken = Environment.GetEnvironmentVariable("TwilioAuthToken");
                var twilioPhoneNumber = Environment.GetEnvironmentVariable("TwilioPhoneNumber");

                var data = JsonConvert.DeserializeObject<SchedualTwilioSmsRequest>(dataJson);

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: data.Message,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(data.PhoneNumber)
                );

                logger.LogInformation($"Message sent: {message.Sid}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while sending the SMS: {ex.Message}");
                throw;
            }
        }
    }
}

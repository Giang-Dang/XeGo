using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Models;

namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public class SendSms
    {
        private readonly ILogger _logger;

        public SendSms(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SendSms>();
        }

        [Function(FuncConst.SendSms)]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing Function {FuncConst.SendSms} ...");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<TwilioSmsRequest>(requestBody);

                if (data?.PhoneNumber == null || data?.Message == null)
                {
                    return new BadRequestObjectResult("Please pass a phoneNumber and text in the request body");
                }

                var accountSid = Environment.GetEnvironmentVariable("TwilioAccountSid");
                var authToken = Environment.GetEnvironmentVariable("TwilioAuthToken");
                var twilioPhoneNumber = Environment.GetEnvironmentVariable("TwilioPhoneNumber");

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: data.Message,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(data.PhoneNumber)
                );

                _logger.LogInformation($"Message sent: {message.Sid}");

                return new OkObjectResult($"Message sent: {message.Sid}");
            }
            catch (Exception e)
            {
                _logger.LogError($"{FuncConst.SendSms}: {e.Message}");
                return new InternalServerErrorResult();
            }

        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Data;
using XeGo.Services.Notifications.Functions.Entities;
using XeGo.Services.Notifications.Functions.Models;
using XeGo.Shared.Lib.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public class StoreScheduledSmsSendingTask(ILoggerFactory loggerFactory, AppDbContext db)
    {
        private readonly ILogger<StoreScheduledSmsSendingTask> _logger = loggerFactory.CreateLogger<StoreScheduledSmsSendingTask>();
        [Function(FuncConst.StoreScheduledSmsSendingTask)]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"{FuncConst.StoreScheduledSmsSendingTask} HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                _logger.LogInformation($"{FuncConst.StoreScheduledSmsSendingTask} requestBody: {requestBody}");

                var requestDto = JsonConvert.DeserializeObject<StoreScheduledSmsSendingTaskRequest>(requestBody);

                if (requestDto == null
                    || requestDto.PhoneNumber == null
                    || requestDto.SendTime == null
                    || requestDto.Message == null)
                {
                    _logger.LogError($"{FuncConst.StoreScheduledSmsSendingTask}: request body can be null!");
                    return new InternalServerErrorResult();
                }

                DateTime sendTime = DateTime.Parse(requestDto.SendTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

                var createDto = new ScheduledTask()
                {
                    Type = FuncConst.FunctionType_SendSms,
                    ExecutionUtcYear = sendTime.Year,
                    ExecutionUtcMonth = sendTime.Month,
                    ExecutionUtcDay = sendTime.Day,
                    ExecutionUtcHour = sendTime.Hour,
                    ExecutionUtcMinute = sendTime.Minute,
                    Json = requestBody,
                    CreatedBy = RoleConstants.System,
                    LastModifiedBy = RoleConstants.System,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                };
                await db.ScheduledTasks.AddAsync(createDto);
                await db.SaveChangesAsync();

                return new OkObjectResult("ScheduledTasks Created!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{FuncConst.StoreScheduledSmsSendingTask}: {e.Message}");
                return new InternalServerErrorResult();
            }
        }
    }


}

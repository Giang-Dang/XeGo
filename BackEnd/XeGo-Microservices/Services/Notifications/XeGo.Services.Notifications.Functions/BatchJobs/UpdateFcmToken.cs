using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Data;
using XeGo.Services.Notifications.Functions.Entities;

namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public class UpdateFcmToken(ILoggerFactory loggerFactory, AppDbContext db)
    {
        private readonly ILogger<UpdateFcmToken> _logger = loggerFactory.CreateLogger<UpdateFcmToken>();


        [Function(FuncNameConst.UpdateFcmToken)]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"{FuncNameConst.UpdateFcmToken} HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var requestDto = JsonConvert.DeserializeObject<UpdateFcmTokenRequest>(requestBody);

                if (requestDto == null)
                {
                    _logger.LogError($"{nameof(UpdateFcmToken)}: Request body can not be null");
                    return new BadRequestObjectResult($"{nameof(UpdateFcmToken)}: Request body can not be null");
                }

                var cUserConnectionInfo =
                    await db.UserConnectionInfos.FirstOrDefaultAsync(u => u.UserId == requestDto.UserId);

                if (cUserConnectionInfo == null)
                {
                    var createDto = new UserConnectionInfo()
                    {
                        UserId = requestDto.UserId,
                        FcmToken = requestDto.FcmToken,
                        CreatedBy = requestDto.UserId,
                        LastModifiedBy = requestDto.UserId,
                    };

                    await db.UserConnectionInfos.AddAsync(createDto);
                    await db.SaveChangesAsync();

                    return new OkObjectResult("FcmToken Updated! (create new)");
                }

                cUserConnectionInfo.FcmToken = requestDto.FcmToken;
                cUserConnectionInfo.LastModifiedBy = requestDto.UserId;
                cUserConnectionInfo.LastModifiedDate = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return new OkObjectResult("FcmToken Updated! (update)"); ;
            }
            catch (Exception e)
            {
                _logger.LogError($"{FuncNameConst.UpdateFcmToken}: {e.Message}");
                return new InternalServerErrorResult();
            }
        }

        public class UpdateFcmTokenRequest
        {
            public string UserId { get; set; } = string.Empty!;
            public string FcmToken { get; set; } = string.Empty!;
        }
    }
}

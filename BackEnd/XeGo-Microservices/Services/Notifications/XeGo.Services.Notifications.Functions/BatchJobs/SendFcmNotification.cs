
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http;
using XeGo.Services.Notifications.Functions.Constants;
using XeGo.Services.Notifications.Functions.Data;


namespace XeGo.Services.Notifications.Functions.BatchJobs
{
    public class SendFcmNotification(ILoggerFactory loggerFactory, AppDbContext db)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<SendFcmNotification>();
        private static readonly HttpClient Client = new();

        [Function(FuncNameConst.SendFcmNotification)]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"{FuncNameConst.SendFcmNotification} HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var requestDto = JsonConvert.DeserializeObject<NotificationRequest>(requestBody);

                if (requestDto == null)
                {
                    _logger.LogError($"{nameof(SendFcmNotification)}: Request body can not be null");
                    return new BadRequestObjectResult($"{nameof(SendFcmNotification)}: Request body can not be null");
                }

                var cUserConnectionInfo =
                    await db.UserConnectionInfos.FirstOrDefaultAsync(u => u.UserId == requestDto.UserId);

                if (cUserConnectionInfo == null)
                {
                    _logger.LogError($"{nameof(SendFcmNotification)}: Not found!");
                    return new BadRequestObjectResult($"{nameof(SendFcmNotification)}: Not found!");
                }

                var payload = new
                {
                    message = new
                    {
                        token = cUserConnectionInfo.FcmToken,
                        notification = new
                        {
                            title = requestDto.Title,
                            body = requestDto.Message
                        }
                    }
                };
                var json = JsonConvert.SerializeObject(payload);

                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var accessToken = await GetAccessTokenAsync();

                Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", @$"Bearer {accessToken}");

                var response = await Client.PostAsync("https://fcm.googleapis.com/v1/projects/xego-404112/messages:send", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"{nameof(SendFcmNotification)}: {error}");
                    return new BadRequestObjectResult(error);
                }
                return new OkObjectResult("Notification sent");
            }
            catch (Exception e)
            {
                _logger.LogError($"{FuncNameConst.SendFcmNotification}: {e.Message}");
                return new InternalServerErrorResult();
            }

        }

        private async Task<string> GetAccessTokenAsync()
        {
            GoogleCredential credential;

            using (var stream = new FileStream("xego-404112-d3a460b45b76.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[] { "https://www.googleapis.com/auth/cloud-platform" });
            }

            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return token;
        }
    }

    public class NotificationRequest
    {
        public string UserId { get; set; } = string.Empty!;
        public string Title { get; set; } = string.Empty!;
        public string Message { get; set; } = string.Empty!;
    }
}

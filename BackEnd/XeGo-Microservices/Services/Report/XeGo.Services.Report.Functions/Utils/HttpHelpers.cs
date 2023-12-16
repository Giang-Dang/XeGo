using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace XeGo.Services.Report.Functions.Utils
{
    public class HttpHelpers
    {
        private readonly HttpClient _client;

        public HttpHelpers()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Get(string url, HttpRequestHeaders? headers)
        {
            try
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        _client.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                    }
                }

                var response = await _client.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                return new OkObjectResult(content);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"An error occurred: {ex.Message}");
            }
        }
    }
}

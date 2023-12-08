using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Location.API.Secrets;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Location.API.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationController(
        ILogger<LocationController> logger)
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet("directions")]
        public async Task<ResponseDto> GetDirectionApi(
            double startLat,
            double startLng,
            double endLat,
            double endLng)
        {
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={startLat},{startLng}&destination={endLat},{endLng}&key={ApiKey.GoogleMapApiKey}";

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var stringResponse = await response.Content.ReadAsStringAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = stringResponse;
                }
                catch (HttpRequestException httpRequestException)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = httpRequestException.Message;
                }
            }
            return ResponseDto;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Media.API.Services.IServices;

namespace XeGo.Services.Media.API.Controllers
{
    [Route("api/v1/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IBlobService _blobService;

        public MediaController(IImageService imageService, IBlobService blobService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        [HttpGet]
        public IActionResult Get(string userId, int imageTypeCode, int imageSizeCode)
        {
            return Ok();
        }

    }
}

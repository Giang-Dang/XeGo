using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Media.API.Data;
using XeGo.Services.Media.API.Entities;
using XeGo.Services.Media.API.Services.IServices;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Media.API.Controllers
{
    [Route("api/v1/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IBlobService _blobService;
        private readonly AppDbContext _db;
        private readonly ILogger<MediaController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public MediaController(IImageService imageService, IBlobService blobService, AppDbContext db, ILogger<MediaController> logger)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _db = db;
            _logger = logger;
            ResponseDto = new();
        }

        [HttpGet("images/avatar")]
        public async Task<ResponseDto> GetUserAvatarUri(string userId, string imageType, string imageSize)
        {
            _logger.LogInformation($"Executing {nameof(MediaController)} > {nameof(GetUserAvatarUri)}...");
            try
            {
                var userImage = _db.UserImages
                    .FirstOrDefault(e => e.UserId == userId && e.ImageType == imageType && e.ImageSize == imageSize);

                if (userImage == null)
                {
                    _logger.LogInformation($"{nameof(MediaController)} > {nameof(GetUserAvatarUri)} : Not found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not Found!";
                    return ResponseDto;
                }
                var uri = _blobService.GetBlobAbsoluteUriWithSas(userImage.BlobName, BlobConstants.ImageContainerName);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = uri;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(MediaController)} > {nameof(GetUserAvatarUri)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPost("images/avatar")]
        public async Task<ResponseDto> UploadAvatar(IFormFile imageFile, string userId)
        {
            _logger.LogInformation($"Executing {nameof(MediaController)} > {nameof(UploadAvatar)}...");

            try
            {
                var blobName = $"avatar/{imageFile.FileName}";
                var uploadSuccess = await _blobService.UploadBlob(blobName, imageFile, BlobConstants.ImageContainerName, null);
                if (!uploadSuccess)
                {
                    _logger.LogError($"{nameof(MediaController)} > {nameof(UploadAvatar)} : Cannot upload the image.");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Cannot upload the image";
                    return ResponseDto;
                }

                var userImage = new UserImage()
                {
                    Id = 0,
                    ImageName = imageFile.FileName,
                    BlobName = blobName,
                    ImageSize = ImageSizeConstants.Origin,
                    ImageType = ImageTypeConstants.Avatar,
                    UserId = userId,
                };

                await _db.AddAsync(userImage);
                await _db.SaveChangesAsync();

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = userImage;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(MediaController)} > {nameof(UploadAvatar)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

    }
}

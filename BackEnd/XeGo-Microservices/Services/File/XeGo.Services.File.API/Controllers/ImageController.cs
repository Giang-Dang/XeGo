using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.File.API.Data;
using XeGo.Services.File.API.Entities;
using XeGo.Services.File.API.Services.IServices;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.File.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IBlobService _blobService;
        private readonly AppDbContext _db;
        private readonly ILogger<ImageController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public ImageController(IImageService imageService, IBlobService blobService, AppDbContext db, ILogger<ImageController> logger)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _db = db;
            _logger = logger;
            ResponseDto = new();
        }

        [HttpGet("avatar")]
        public ResponseDto GetUserAvatarUri(string userId, string imageSize)
        {
            _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(GetUserAvatarUri)}...");
            try
            {
                imageSize = imageSize.ToUpper();

                var userImage = _db.UserImages
                    .FirstOrDefault(e => e.UserId == userId && e.ImageType == ImageTypeConstants.Avatar && e.ImageSize == imageSize);

                if (userImage == null)
                {
                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(GetUserAvatarUri)} : Not found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not Found!";
                    return ResponseDto;
                }
                var uri = _blobService.GetBlobAbsoluteUriWithSas(userImage.BlobName, BlobConstants.ImageContainerName);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = uri;
                _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(GetUserAvatarUri)}: Done.");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(ImageController)} > {nameof(GetUserAvatarUri)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpGet]
        public ResponseDto GetUserImageUri(string userId, string imageType, string imageSize)
        {
            _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(GetUserImageUri)}...");
            try
            {
                var userImage = _db.UserImages
                    .FirstOrDefault(e => e.UserId == userId && e.ImageType == imageType && e.ImageSize == imageSize);

                if (userImage == null)
                {
                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(GetUserImageUri)} : Not found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not Found!";
                    return ResponseDto;
                }
                var uri = _blobService.GetBlobAbsoluteUriWithSas(userImage.BlobName, BlobConstants.ImageContainerName);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = uri;

                _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(GetUserImageUri)} : Done.");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(ImageController)} > {nameof(GetUserImageUri)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> UploadImage(IFormFile imageFile, string folderName, string imageSize, string imageType,
            string userId)
        {
            _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(UploadImage)}...");

            try
            {
                folderName = folderName.ToLower();
                imageSize = imageSize.ToUpper();
                imageType = imageType.ToUpper();

                var cUserImage = await _db.UserImages
                    .FirstOrDefaultAsync(e => e.UserId == userId && e.ImageType == imageType && e.ImageSize == imageSize);

                var blobName = $"{folderName}/{Guid.NewGuid().ToString()}_{imageFile.FileName}";
                var uploadSuccess = await _blobService.UploadBlob(blobName, imageFile, BlobConstants.ImageContainerName, null);
                if (!uploadSuccess)
                {
                    _logger.LogError($"{nameof(ImageController)} > {nameof(UploadImage)} : Cannot upload the image.");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Cannot upload the image";
                    return ResponseDto;
                }

                if (cUserImage != null)
                {
                    //Delete old blob
                    var blobDeleted = await _blobService.DeleteBlob(cUserImage.BlobName, BlobConstants.ImageContainerName);
                    if (!blobDeleted)
                    {
                        throw new Exception("Cannot delete old blob!");
                    }
                    //Update db
                    cUserImage.BlobName = blobName;
                    cUserImage.LastModifiedBy = userId;
                    cUserImage.LastModifiedDate = DateTime.UtcNow;

                    _db.UserImages.Update(cUserImage);
                    await _db.SaveChangesAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = cUserImage;
                    ResponseDto.Message = "Updated!";

                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(UploadImage)} : Done (updated).");
                }
                else
                {
                    //create
                    var userImage = new UserImage()
                    {
                        Id = 0,
                        ImageName = imageFile.FileName,
                        BlobName = blobName,
                        ImageSize = imageSize,
                        ImageType = imageType,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedBy = userId,
                        LastModifiedDate = DateTime.UtcNow,
                    };

                    await _db.UserImages.AddAsync(userImage);
                    await _db.SaveChangesAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = userImage;
                    ResponseDto.Message = "Created!";

                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(UploadImage)} : Done (created).");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(ImageController)} > {nameof(UploadImage)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("avatar")]
        public async Task<ResponseDto> UploadAvatar(IFormFile imageFile, string userId)
        {
            _logger.LogInformation($"Executing {nameof(ImageController)} > {nameof(UploadAvatar)}...");

            try
            {
                var blobName = $"avatar/{Guid.NewGuid().ToString()}_{imageFile.FileName}";
                var uploadSuccess = await _blobService.UploadBlob(blobName, imageFile, BlobConstants.ImageContainerName, null);
                if (!uploadSuccess)
                {
                    _logger.LogError($"{nameof(ImageController)} > {nameof(UploadAvatar)} : Cannot upload the image.");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Cannot upload the image";
                    return ResponseDto;
                }

                var cUserImage = await _db.UserImages
                    .FirstOrDefaultAsync(e => e.UserId == userId && e.ImageType == ImageTypeConstants.Avatar && e.ImageSize == ImageSizeConstants.Origin);

                if (cUserImage != null)
                {
                    //Delete old blob
                    var blobDeleted = await _blobService.DeleteBlob(cUserImage.BlobName, BlobConstants.ImageContainerName);
                    if (!blobDeleted)
                    {
                        throw new Exception("Cannot delete old blob!");
                    }
                    //Update db
                    cUserImage.BlobName = blobName;
                    cUserImage.LastModifiedBy = userId;
                    cUserImage.LastModifiedDate = DateTime.UtcNow;

                    _db.UserImages.Update(cUserImage);
                    await _db.SaveChangesAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = cUserImage;
                    ResponseDto.Message = "Updated!";

                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(UploadAvatar)} : Done (updated).");
                }
                else
                {
                    //create
                    var userImage = new UserImage()
                    {
                        Id = 0,
                        ImageName = imageFile.FileName,
                        BlobName = blobName,
                        ImageSize = ImageSizeConstants.Origin,
                        ImageType = ImageTypeConstants.Avatar,
                        UserId = userId,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedBy = userId,
                        LastModifiedDate = DateTime.UtcNow,
                    };

                    await _db.AddAsync(userImage);
                    await _db.SaveChangesAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = userImage;
                    ResponseDto.Message = "Created!";

                    _logger.LogInformation($"{nameof(ImageController)} > {nameof(UploadAvatar)} : Done (created).");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(ImageController)} > {nameof(UploadAvatar)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }



    }
}

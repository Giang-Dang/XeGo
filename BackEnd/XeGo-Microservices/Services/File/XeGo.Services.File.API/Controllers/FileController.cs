using Microsoft.AspNetCore.Mvc;
using XeGo.Services.File.API.Data;
using XeGo.Services.File.API.Entities;
using XeGo.Services.File.API.Services.IServices;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Models;


namespace XeGo.Services.File.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly AppDbContext _db;
        private readonly ILogger<FileController> _logger;

        private ResponseDto ResponseDto { get; set; }

        public FileController(IBlobService blobService, AppDbContext db, ILogger<FileController> logger)
        {
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _db = db;
            _logger = logger;
            ResponseDto = new();
        }

        [HttpGet("uri")]
        public ResponseDto GetUserFileUri(string userId, string fileName, string type)
        {
            _logger.LogInformation($"Executing {nameof(FileController)} > {nameof(GetUserFileUri)}...");

            try
            {
                var userFile = _db.UserFiles.FirstOrDefault(f => f.UserId == userId && f.Name == fileName && f.Type == type);

                if (userFile == null)
                {
                    _logger.LogInformation($"{nameof(FileController)} > {nameof(GetUserFileUri)} : Not found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not Found!";
                    return ResponseDto;
                }

                var uri = _blobService.GetBlobAbsoluteUriWithSas(userFile.BlobName, BlobConstants.FileContainerName);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = uri;

                _logger.LogInformation($"Executing {nameof(FileController)} > {nameof(GetUserFileUri)} : Done.");

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(FileController)} > {nameof(GetUserFileUri)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> UploadFile(IFormFile file, string folderName, string type, string fromUserId)
        {
            _logger.LogInformation($"Executing {nameof(FileController)} > {nameof(UploadFile)}...");
            try
            {
                folderName = folderName.ToLower();
                type = type.ToUpper();

                var blobName = $"{folderName}/{Guid.NewGuid().ToString()}_{file.FileName}";
                var uploadSuccess = await _blobService.UploadBlob(blobName, file, BlobConstants.FileContainerName, null);
                if (!uploadSuccess)
                {
                    _logger.LogError($"{nameof(FileController)} > {nameof(UploadFile)} : Cannot upload the file.");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Cannot upload the image";
                    return ResponseDto;
                }

                var userFile = new UserFiles()
                {
                    Id = 0,
                    UserId = fromUserId,
                    Name = file.FileName,
                    Type = type,
                    BlobName = blobName,
                    CreatedBy = fromUserId,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedBy = fromUserId,
                    LastModifiedDate = DateTime.UtcNow,
                };

                await _db.UserFiles.AddAsync(userFile);
                await _db.SaveChangesAsync();

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = userFile;
                ResponseDto.Message = "Created!";

                _logger.LogInformation($"{nameof(FileController)} > {nameof(UploadFile)} : Done.");

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(FileController)} > {nameof(UploadFile)} : {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;

        }
    }
}

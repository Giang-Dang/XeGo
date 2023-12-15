using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Rating.API.Entities;
using XeGo.Services.Rating.API.Models;
using XeGo.Services.Rating.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Rating.API.Controllers
{
    [Route("api/rating")]
    public class UserRatingController(
        IUserRatingRepository userRatingRepo,
        ILogger<UserRatingController> logger) : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpPost]
        public async Task<ResponseDto> CreateRating(CreateRatingRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(UserRatingController)}>{nameof(CreateRating)}...");
            try
            {
                var cExistingRating = await userRatingRepo
                    .GetAllAsync(r =>
                        r.RideId == requestDto.RideId &&
                        r.FromUserId == requestDto.FromUserId &&
                        r.ToUserId == requestDto.ToUserId);

                if (cExistingRating != null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Already exists!";
                    return ResponseDto;
                }

                var createEntity = new UserRating()
                {
                    RideId = requestDto.RideId,
                    FromUserId = requestDto.FromUserId,
                    ToUserId = requestDto.ToUserId,
                    FromUserRole = requestDto.FromUserRole,
                    ToUserRole = requestDto.ToUserRole,
                    Rating = requestDto.Rating,
                    CreatedBy = requestDto.CreatedBy,
                    LastModifiedBy = requestDto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                };

                createEntity = await userRatingRepo.CreateAsync(createEntity);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createEntity;

            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(UserRatingController)}>{nameof(CreateRating)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}

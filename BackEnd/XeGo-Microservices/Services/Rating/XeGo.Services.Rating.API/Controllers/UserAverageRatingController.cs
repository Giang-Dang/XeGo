using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Rating.API.Entities;
using XeGo.Services.Rating.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Rating.API.Controllers
{
    [Route("api/average-rating")]
    public class UserAverageRatingController(
        IUserAverageRatingRepository userAverageRatingRepo,
        ILogger<UserAverageRatingController> logger) : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllAverageRatings(
            string? userId,
            string? userRole,
            int pageNumber = 0,
            int pageSize = 0)
        {
            logger.LogInformation($"Executing {nameof(UserAverageRatingController)}>{nameof(GetAllAverageRatings)}...");
            try
            {
                Expression<Func<UserAverageRating, bool>> filters = r =>
                    (userId == null || r.UserId == userId) &&
                    (userRole == null || r.UserRole == userRole);

                var userAverageRatings = await userAverageRatingRepo.GetAllAsync(
                    filter: filters,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                logger.LogInformation($"GetAllAverageRatings : {userAverageRatings.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = userAverageRatings;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(UserAverageRatingController)}>{nameof(GetAllAverageRatings)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}

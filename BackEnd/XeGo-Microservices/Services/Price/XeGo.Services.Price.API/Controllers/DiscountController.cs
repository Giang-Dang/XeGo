using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Price.API.Entities;
using XeGo.Services.Price.API.Models;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Price.API.Controllers
{
    [Route("api/discount")]
    [ApiController]
    public class DiscountController(IDiscountRepository discountRepo, ILogger<DiscountController> logger)
        : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllDiscount(
            int? id,
            string? name,
            double? percentStart,
            double? percentEnd,
            int? quantityStart,
            int? quantityEnd,
            DateTime? fromDayStart,
            DateTime? fromDayEnd,
            DateTime? toDayStart,
            DateTime? toDayEnd,
            string? searchString,
            int pageNumber = 0,
            int pageSize = 0)
        {
            logger.LogInformation($"Getting discounts...");

            try
            {
                Expression<Func<Entities.Discount, bool>> filters = d =>
                    (id == null || d.Id == id) &&
                    (name == null || d.Name.Equals(name)) &&
                    (percentStart == null || d.Percent >= percentStart) &&
                    (percentEnd == null || d.Percent <= percentEnd) &&
                    (quantityStart == null || d.Quantity >= quantityStart) &&
                    (quantityEnd == null || d.Quantity <= quantityEnd) &&
                    (fromDayStart == null || d.FromDay >= fromDayStart) &&
                    (fromDayEnd == null || d.FromDay <= fromDayEnd) &&
                    (toDayStart == null || d.ToDay >= toDayStart) &&
                    (toDayEnd == null || d.ToDay <= toDayEnd) &&
                    (searchString == null || d.Name.Contains(searchString));

                var discounts = await discountRepo.GetAllAsync(
                    filter: filters,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                logger.LogInformation($"Get discounts : {discounts.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = discounts;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(DiscountController)}>{nameof(GetAllDiscount)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateDiscount(CreateDiscountRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(PriceController)}>{nameof(CreateDiscount)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var createDto = new Discount()
                {
                    Id = requestDto.Id,
                    Name = requestDto.Name,
                    Percent = requestDto.Percent,
                    Quantity = requestDto.Quantity,
                    FromDay = requestDto.FromDay,
                    ToDay = requestDto.ToDay,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };

                await discountRepo.CreateAsync(createDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createDto;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(PriceController)}>{nameof(CreateDiscount)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditDiscount(EditDiscountRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(PriceController)}>{nameof(EditDiscount)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var cEntity = await discountRepo.GetAsync(d => d.Id == requestDto.Id);
                if (cEntity == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not found!";
                    return ResponseDto;
                }

                cEntity.Name = requestDto.Name ?? cEntity.Name;
                cEntity.Percent = requestDto.Percent ?? cEntity.Percent;
                cEntity.Quantity = requestDto.Quantity ?? cEntity.Quantity;
                cEntity.FromDay = requestDto.FromDay ?? cEntity.FromDay;
                cEntity.ToDay = requestDto.ToDay ?? cEntity.ToDay;
                cEntity.LastModifiedBy = requestDto.ModifiedBy;
                cEntity.LastModifiedDate = DateTime.UtcNow;

                await discountRepo.UpdateAsync(cEntity);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = cEntity;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(PriceController)}>{nameof(EditDiscount)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}

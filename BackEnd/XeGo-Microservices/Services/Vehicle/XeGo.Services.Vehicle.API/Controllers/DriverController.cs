using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Vehicle.API.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriverController(
        IDriverRepository driverRepo,
        ILogger<DriverController> logger)
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllDrivers(
            string? userName,
            string? firstName,
            string? lastName,
            string? phoneNumber,
            string? email,
            string? address,
            bool? isAssigned,
            int pageNumber = 0,
            int pageSize = 0)
        {
            logger.LogInformation("{class}>{function}: Received", nameof(DriverController), nameof(GetAllDrivers));
            try
            {
                Expression<Func<Driver, bool>> filters = u =>
                    (userName == null || u.UserName.ToUpper().Contains(userName.ToUpper())) &&
                    (email == null || u.Email.ToUpper().Contains(email.ToUpper())) &&
                    (firstName == null || u.FirstName.ToUpper().Contains(firstName.ToUpper())) &&
                    (lastName == null || u.LastName.ToUpper().Contains(lastName.ToUpper())) &&
                    (phoneNumber == null || u.PhoneNumber.ToUpper().Contains(phoneNumber.ToUpper())) &&
                    (address == null || u.Address.ToUpper().Contains(address.ToUpper())) &&
                    (isAssigned == null || u.IsAssigned == isAssigned.Value);

                var drivers = await driverRepo.GetAllAsync(
                    filter: filters,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = drivers;

                logger.LogError($"{nameof(DriverController)}>{nameof(GetAllDrivers)}: Done.");

            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(DriverController)}>{nameof(GetAllDrivers)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpGet("{userId}")]
        public async Task<ResponseDto> GetDriverById(string userId)
        {
            logger.LogInformation("{class}>{function}: Received", nameof(DriverController), nameof(GetDriverById));
            try
            {
                var driver = await driverRepo.GetAsync(d => d.UserId == userId);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = driver;

                logger.LogError($"{nameof(DriverController)}>{nameof(GetDriverById)}: Done.");

            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(DriverController)}>{nameof(GetDriverById)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}

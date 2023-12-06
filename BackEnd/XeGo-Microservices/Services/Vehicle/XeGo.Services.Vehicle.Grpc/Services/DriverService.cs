using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Vehicle.Grpc.Data;
using XeGo.Services.Vehicle.Grpc.Entities;
using XeGo.Services.Vehicle.Grpc.Protos;

namespace XeGo.Services.Vehicle.Grpc.Services
{
    public class DriverService(
        AppDbContext db,
        ILogger<DriverService> logger
        ) : DriverProtoService.DriverProtoServiceBase
    {
        private DriverResponse Response { get; set; } = new();

        public override async Task<DriverResponse> CreateDriver(CreateDriverRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(DriverService)}>{nameof(CreateDriver)}: Begin.");

            try
            {
                var cDriver = await db.Drivers.FirstOrDefaultAsync(d => d.UserId == request.UserId);
                if (cDriver != null)
                {
                    Response.IsSuccess = false;
                    Response.Message = "Already Existed!";
                    return Response;
                }

                var driver = new Driver()
                {
                    UserId = request.UserId,
                    Address = request.Address,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.UserName,
                    IsAssigned = false,
                    CreatedBy = request.ModifiedBy,
                    LastModifiedBy = request.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                };

                await db.Drivers.AddAsync(driver);
                await db.SaveChangesAsync();

                Response.IsSuccess = true;
                Response.Message = JsonConvert.SerializeObject(driver);
                logger.LogInformation($"{nameof(DriverService)}>{nameof(CreateDriver)}: Done.");
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message;
                logger.LogError($"{nameof(DriverService)}>{nameof(CreateDriver)}: {e.Message}");
            }
            return Response;
        }

        public override async Task<DriverResponse> EditDriver(EditDriverRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(DriverService)}>{nameof(EditDriver)}: Begin.");

            try
            {
                var cDriver = await db.Drivers.FirstOrDefaultAsync(d => d.UserId == request.UserId);
                if (cDriver == null)
                {
                    Response.IsSuccess = false;
                    Response.Message = "Not Found!";
                    return Response;
                }

                cDriver.UserName = request.UserName ?? cDriver.UserName;
                cDriver.FirstName = request.FirstName ?? cDriver.FirstName;
                cDriver.LastName = request.LastName ?? cDriver.LastName;
                cDriver.Email = request.Email ?? cDriver.Email;
                cDriver.PhoneNumber = request.PhoneNumber ?? cDriver.PhoneNumber;
                cDriver.Address = request.Address ?? cDriver.Address;
                cDriver.IsAssigned = request.IsAssigned ?? cDriver.IsAssigned;
                cDriver.LastModifiedBy = request.ModifiedBy;
                cDriver.LastModifiedDate = DateTime.UtcNow;

                db.Drivers.Update(cDriver);
                await db.SaveChangesAsync();

                Response.IsSuccess = true;
                Response.Message = JsonConvert.SerializeObject(cDriver);
                logger.LogInformation($"{nameof(DriverService)}>{nameof(EditDriver)}: Done.");
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message;
                logger.LogError($"{nameof(DriverService)}>{nameof(EditDriver)}: {e.Message}");
            }
            return Response;
        }
    }
}

using XeGo.Services.Vehicle.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class DriverGrpcService(DriverProtoService.DriverProtoServiceClient service)
    {
        public async Task<DriverResponse> CreateDriver(
            string userId,
            string userName,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string address,
            string modifiedBy)
        {
            var request = new CreateDriverRequest()
            {
                UserId = userId,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                ModifiedBy = modifiedBy
            };

            return await service.CreateDriverAsync(request);
        }

        public async Task<DriverResponse> EditDriver(
            string userId,
            string? userName,
            string? firstName,
            string? lastName,
            string? phoneNumber,
            string? email,
            string? address,
            bool? isAssigned,
            string modifiedBy
        )
        {
            var request = new EditDriverRequest()
            {
                UserId = userId,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                IsAssigned = isAssigned,
                ModifiedBy = modifiedBy
            };

            return await service.EditDriverAsync(request);
        }
    }
}

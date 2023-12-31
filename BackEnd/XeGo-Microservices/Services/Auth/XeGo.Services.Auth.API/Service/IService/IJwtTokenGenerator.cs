﻿using XeGo.Services.Auth.API.Entities;

namespace XeGo.Services.Auth.API.Service.IService
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser applicationUser, string loginApp);
        string GenerateRefreshToken(int size = 256);
    }
}

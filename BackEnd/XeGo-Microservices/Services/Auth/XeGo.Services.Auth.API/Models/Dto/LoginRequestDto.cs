﻿namespace XeGo.Services.Auth.API.Models.Dto
{
    public class LoginRequestDto
    {
        public string PhoneNumber { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromApp { get; set; } = "";
    }
}

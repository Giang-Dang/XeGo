﻿namespace XeGo.Services.Driver.API.Models
{
    public class GetDriverInfoRequestDto
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedStartDate { get; set; }
        public DateTime? CreatedEndDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedStartDate { get; set; }
        public DateTime? LastModifiedEndDate { get; set; }
        public string? SearchString { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }
}

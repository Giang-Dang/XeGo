using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using XeGo.Services.Report.Functions.Data;
using XeGo.Services.Report.Functions.Models;
using XeGo.Services.Report.Functions.Models.Dto;
using XeGo.Services.Report.Functions.Utils;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Report.Functions.Reports
{
    public class RideReportGenerator
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _db;
        public RideReportGenerator(ILoggerFactory loggerFactory, AppDbContext db)
        {
            _logger = loggerFactory.CreateLogger<RideReportGenerator>();
            _db = db;
        }

        public async Task Generate(int reportId, ParamsReportRequest req)
        {
            _logger.LogInformation($"Executing Function {nameof(RideReportGenerator)} > {nameof(Generate)} ...");
            try
            {
                if (req.StartDate == null || req.EndDate == null)
                {
                    throw new Exception($"req.StartDate == null || req.EndDate == null");
                }
                var reportInfo = await _db.ReportInfos.FirstOrDefaultAsync(r => r.Id == reportId);
                if (reportInfo == null)
                {
                    throw new Exception($"Cannot find report (id: {reportId}).");
                }

                reportInfo.Status = ReportStatusConstants.Processing;
                reportInfo.LastModifiedBy = RoleConstants.System;
                reportInfo.LastModifiedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();

                var allRides = await CallApiHelper.GetListFromUrl<RideDto>("http://xego.services.ride.api:8080/api/rides", _logger);
                var allPrices = await CallApiHelper.GetListFromUrl<PriceDto>("http://xego.services.price.api:8080/api/price", _logger);
                var allUsers = await CallApiHelper.GetListFromUrl<UserDto>("http://xego.services.auth.api:8080/api/auth/user", _logger);

                DateTime startDate = DateTime.Parse(req.StartDate, null, System.Globalization.DateTimeStyles.RoundtripKind);
                DateTime endDate = DateTime.Parse(req.EndDate, null, System.Globalization.DateTimeStyles.RoundtripKind);

                List<RideReportQueryResponse> queryResponse = new();
                foreach (var ride in allRides)
                {
                    if (ride.CreatedDate < startDate || ride.CreatedDate > endDate)
                    {
                        continue;
                    }

                    var price = allPrices.First(p => p.RideId == ride.Id);
                    var rider = allUsers.FirstOrDefault(u => u.UserId == ride.RiderId);
                    var driver = allUsers.FirstOrDefault(u => (ride.DriverId != null && u.UserId == ride.DriverId));

                    RideReportQueryResponse query = new(
                        ride.Id,
                        rider == null ? "N/A" : (rider.FirstName + ", " + rider.LastName),
                        driver == null ? "N/A" : (driver.FirstName + ", " + driver.LastName),
                        ride.DiscountId,
                        ride.Status,
                        ride.StartAddress,
                        ride.DestinationAddress,
                        ride.PickupTime.ToLongDateString(),
                        ride.IsScheduleRide,
                        price.VehicleTypeId,
                        price.DistanceInMeters,
                        price.TotalPrice,
                        ride.CreatedBy,
                        ride.CreatedDate,
                        ride.LastModifiedBy,
                        ride.LastModifiedDate);
                }

                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("sheet1");
                    List<string[]> headers = new List<string[]>
                    {
                        new[]
                        {
                            "RIDE_ID",
                            "RIDER_NAME",
                            "DRIVER_NAME",
                            "DISCOUNT_ID",
                            "STATUS",
                            "PICKUP_ADDRESS",
                            "DROPOFF_ADDRESS",
                            "PICKUP_TIME",
                            "IS_SCHEDULE_RIDE",
                            "VEHICLE_TYPE_ID",
                            "DISTANCE_IN_METERS",
                            "TOTAL_PRICE",
                            "CREATED_BY",
                            "CREATED_DATE",
                            "LAST_MODIFIED_BY",
                            "LAST_MODIFIED_DATE"
                        }
                    };

                    sheet.Cell(1, 6).InsertData(new List<string[]> { new[] { "RIDE REPORT" } });
                    sheet.Cell(2, 1).InsertData(new List<string[]> { new[] {
                        "RIDE CREATED FROM: ",
                        startDate.ToLongDateString()
                    }});
                    sheet.Cell(3, 1).InsertData(new List<string[]> { new[] {
                        "RIDE CREATED TO: ",
                        endDate.ToLongDateString()
                    }});
                    sheet.Cell(4, 1).InsertData(new List<string[]> { new[]
                    {
                        "REPORT GENERATION DATE: ",
                        DateTime.Now.ToLongDateString(),
                    }});

                    sheet.Cell(6, 1).InsertData(headers);
                    sheet.Cell(7, 1).InsertData(queryResponse.ToList());

                    //style
                    sheet.Cell(1, 6).Style.Font.Bold = true;
                    sheet.Cell(2, 1).Style.Font.Bold = true;
                    sheet.Cell(3, 1).Style.Font.Bold = true;
                    sheet.Cell(4, 6).Style.Font.Bold = true;
                    var headerRange = sheet.Range(sheet.Cell(6, 1), sheet.Cell(6, headers[0].Length));
                    headerRange.Style.Font.Bold = true;

                    sheet.SheetView.FreezeRows(6);
                    sheet.SheetView.FreezeColumns(1);

                    sheet.ColumnsUsed().AdjustToContents();

                    //upload
                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    stream.Position = 0;

                    var byteContent = new ByteArrayContent(stream.ToArray());
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(byteContent, "file", $"{reportInfo.ReportName}.xlsx");

                    var httpClient = new HttpClient();

                    var folderName = "reports";

                    var response = await httpClient.PostAsync($"http://xego.services.file.api:8080/api/files?folderName={folderName}&type={FileTypeConstants.Report}&fromUserId={req.CreatedBy}", multipartContent);

                    var responseContent = await response.Content.ReadAsStringAsync();
                }

                reportInfo.Status = ReportStatusConstants.Completed;
                reportInfo.LastModifiedBy = RoleConstants.System;
                reportInfo.LastModifiedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();

                _logger.LogInformation($"{nameof(RideReportGenerator)} > {nameof(Generate)}: Done!");
            }
            catch (Exception e)
            {
                var reportInfo = await _db.ReportInfos.FirstOrDefaultAsync(r => r.Id == reportId);
                if (reportInfo != null)
                {
                    reportInfo.Status = ReportStatusConstants.Failed;
                    reportInfo.LastModifiedBy = RoleConstants.System;
                    reportInfo.LastModifiedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
                _logger.LogError($"{nameof(RideReportGenerator)} > {nameof(Generate)} Error: {e.ToString()}");
            }
        }

        private record RideReportQueryResponse(
            int RideId,
            string? RiderName,
            string? DriverName,
            int? DiscountId,
            string Status,
            string StartAddress,
            string EndAddress,
            string PickupTime,
            bool IsScheduleRide,
            int VehicleTypeId,
            double DistanceInMeters,
            double TotalPrice,
            string CreatedBy,
            DateTime CreatedDate,
            string LastModifiedBy,
            DateTime LastModifiedDate);
    }
}

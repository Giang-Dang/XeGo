using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using XeGo.Services.Report.Functions.Data;
using XeGo.Services.Report.Functions.Models;
using XeGo.Services.Report.Functions.Models.Dto;
using XeGo.Services.Report.Functions.Utils;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Report.Functions.Reports
{
    public class RevenueByMonthAndYearReportGenerator
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _db;
        public RevenueByMonthAndYearReportGenerator(ILoggerFactory loggerFactory, AppDbContext db)
        {
            _logger = loggerFactory.CreateLogger<RevenueByMonthAndYearReportGenerator>();
            _db = db;
        }

        public async Task Generate(int reportId, ParamsReportRequest req)
        {
            _logger.LogInformation($"Executing Function {nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} ...");
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
                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} allRides {JsonConvert.SerializeObject(allRides)} ");

                var allPrices = await CallApiHelper.GetListFromUrl<PriceDto>("http://xego.services.price.api:8080/api/price", _logger);
                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} allPrices {JsonConvert.SerializeObject(allPrices)} ");

                var allUsers = await CallApiHelper.GetListFromUrl<UserDto>("http://xego.services.auth.api:8080/api/auth/user", _logger);
                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} allUsers {JsonConvert.SerializeObject(allUsers)} ");

                List<RevenueByMonthYearReportQueryResponse> rows = new();

                foreach (var price in allPrices)
                {
                    var completedPrice = allRides.FirstOrDefault(r => r.Status == RideStatusConstants.Completed && r.Id == price.RideId);
                    if (completedPrice == null)
                    {
                        continue;
                    }

                    var cRow = rows.FirstOrDefault(r => r.Year == price.CreatedDate.Year && r.Month == price.CreatedDate.Month);

                    if (cRow == null)
                    {
                        var newRow = new RevenueByMonthYearReportQueryResponse(price.CreatedDate.Year, price.CreatedDate.Month, price.TotalPrice, price.DistanceInMeters);
                        rows.Add(newRow);
                    }
                    else
                    {
                        var cIndex = rows.IndexOf(cRow);
                        rows[cIndex].Revenue += price.TotalPrice;
                        rows[cIndex].TotalDistanceInMeter += price.DistanceInMeters;
                    }
                }


                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} RevenueByMonthAndYearReportGenerator_Generate_rows {JsonConvert.SerializeObject(rows)} ");


                var sortedQueryResponse = rows.OrderBy(x => x.Year).ThenBy(x => x.Month);

                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} sortedQueryResponse {JsonConvert.SerializeObject(sortedQueryResponse)} ");


                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("sheet1");
                    List<string[]> headers = new List<string[]>
                    {
                        new[]
                        {
                            "YEAR",
                            "MONTH",
                            "REVENUE",
                            "TOTAL_DISTANCE_IN_METERS",
                        }
                    };

                    sheet.Cell(1, 6).InsertData(new List<string[]> { new[] { "REVENUE BY MONTH AND YEAR REPORT" } });
                    sheet.Cell(4, 1).InsertData(new List<string[]> { new[]
                    {
                        "REPORT GENERATION DATE: ",
                        DateTime.Now.ToLongDateString(),
                    }});

                    sheet.Cell(6, 1).InsertData(headers);
                    sheet.Cell(7, 1).InsertData(sortedQueryResponse);

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

                _logger.LogInformation($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)}: Done!");
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
                _logger.LogError($"{nameof(RevenueByMonthAndYearReportGenerator)} > {nameof(Generate)} Error: {e.ToString()}");
            }
        }

        public class RevenueByMonthYearReportQueryResponse
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public double Revenue { get; set; }
            public double TotalDistanceInMeter { get; set; }

            public RevenueByMonthYearReportQueryResponse(int year, int month, double revenue, double totalDistanceInMeter)
            {
                Year = year;
                Month = month;
                Revenue = revenue;
                TotalDistanceInMeter = totalDistanceInMeter;
            }
        }
    }
}

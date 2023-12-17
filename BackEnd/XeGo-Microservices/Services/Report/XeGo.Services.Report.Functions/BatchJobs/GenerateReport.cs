using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XeGo.Services.Report.Functions.Constants;
using XeGo.Services.Report.Functions.Data;
using XeGo.Services.Report.Functions.Entities;
using XeGo.Services.Report.Functions.Models;
using XeGo.Services.Report.Functions.Reports;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Report.Functions.BatchJobs
{
    public class GenerateReport
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _db;
        private readonly RideReportGenerator _rideReportGenerator;
        private readonly RevenueByMonthAndYearReportGenerator _venueByMonthAndYearReportGenerator;

        public GenerateReport(ILoggerFactory loggerFactory, AppDbContext db)
        {
            _logger = loggerFactory.CreateLogger<GenerateReport>();
            _db = db;
            _rideReportGenerator = new RideReportGenerator(loggerFactory, db);
            _venueByMonthAndYearReportGenerator = new RevenueByMonthAndYearReportGenerator(loggerFactory, db);
        }

        [Function(FuncConst.GenerateReport)]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing Function {FuncConst.GenerateReport} ...");
            var utcNow = DateTime.UtcNow;
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var body = JsonConvert.DeserializeObject<ParamsReportRequest>(requestBody);

                var reportInfo = new ReportInfo()
                {
                    ReportType = body.ReportType,
                    ReportName = body.CreatedBy + "_" + body.ReportType,
                    SubmissionDateTime = utcNow,
                    CompletionDateTime = null,
                    Status = ReportStatusConstants.Submitted,
                    CreatedBy = body.CreatedBy,
                    LastModifiedBy = body.CreatedBy,
                    CreatedDate = utcNow,
                    LastModifiedDate = utcNow,
                };
                await _db.ReportInfos.AddAsync(reportInfo);
                await _db.SaveChangesAsync();

                //update report name
                reportInfo.ReportName = body.CreatedBy + "_" + body.ReportType + "_" + reportInfo.Id;
                _db.ReportInfos.Update(reportInfo);
                await _db.SaveChangesAsync();

                switch (body.ReportType)
                {
                    case ReportTypeConstants.rideReport:
                        await _rideReportGenerator.Generate(reportInfo.Id, body);
                        break;
                    case ReportTypeConstants.revenueByMonthAndYearReport:
                        await _venueByMonthAndYearReportGenerator.Generate(reportInfo.Id, body);
                        break;
                    default:
                        reportInfo.Status = ReportStatusConstants.DoesNotSupport;
                        reportInfo.LastModifiedDate = DateTime.UtcNow;
                        reportInfo.LastModifiedBy = RoleConstants.System;
                        await _db.SaveChangesAsync();
                        throw new Exception("Invalid Report Type!");
                }

                return new OkObjectResult(reportInfo);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

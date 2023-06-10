using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PBL5.Employees;
using PBL5.Enum;
using PBL5.Localization;
using PBL5.Mobiles;
using PBL5.Permissions;
using PBL5.Reports;
using PBL5.ResponseInfos;
using PBL5.TimeSheets;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using static PBL5.Mobiles.AppleNotification;

namespace PBL5.Reports
{
    [Authorize(PBL5Permissions.Report.Default)]
    public class ReportAppService :
    CrudAppService<
        Report,
        ReportDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateReportDto,
        UpdateReportDto>,
    IReportAppService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStringLocalizer<PBL5Resource> _localizer;
        private readonly IMobileRepository _mobileRepository;
        
        public ReportAppService(
            IReportRepository reportRepository,
            IStringLocalizer<PBL5Resource> localizer,
            ITimeSheetRepository timeSheetRepository,
            IEmployeeRepository employeeRepository,
            IMobileRepository mobileRepository
            ) : base(reportRepository)
        {
            _mobileRepository = mobileRepository;
            _reportRepository = reportRepository;
            _timeSheetRepository = timeSheetRepository;
            _employeeRepository = employeeRepository;
            _localizer = localizer;
        }

        public async Task<PagedResultDto<ReportDto>> SearchListAsync(ConditionSearchDto input)
        {
            var (total, result) = await _reportRepository.SearchListAsync(
                input.KeySearch,
                input.DateReport,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting);
            var finalResult = ObjectMapper.Map<List<Report>, List<ReportDto>>(result);
            return new(total, finalResult);
        }

        public async Task ReportTimeSheetAsync(CreateReportDto input)
        {         
            var timeSheet = await _timeSheetRepository.FindAsync(x => x.Employee.Id == input.EmployeeId && x.DateCheckIn.Date == input.ReportDate.Date);
            if (timeSheet != null)
            {
                var report = new Report();
                report.EmployeeId = input.EmployeeId;
                report.ReportDate = timeSheet.DateCheckIn;
                report.Content = input.Content;
                report.ReportStatus = ReportStatus.PENDING;
                await _reportRepository.InsertAsync(report);
            }
            else
            {
                var report = new Report();
                report.EmployeeId = input.EmployeeId;
                report.ReportDate = input.ReportDate;
                report.Content = input.Content;
                report.ReportStatus = ReportStatus.PENDING;
                await _reportRepository.InsertAsync(report);
            };
        }

        public async Task<List<ReportDto>> GetHistoryReportAsync(Guid employeeId)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            var employee = await _employeeRepository.FindAsync(x => x.Id == employeeId);
            var listReport = await  _reportRepository.GetListAsync(x => x.EmployeeId == employeeId); 
            return ObjectMapper.Map<List<Report>, List<ReportDto>>(listReport);
        }

        public async Task ChangeStatusReportAsync(Guid reportId, string status)
        {
            var report = await _reportRepository.FindAsync(x => x.Id == reportId);
            var newStatus = status;
            if (report.ReportStatus == ReportStatus.PENDING) { newStatus =  ContentNotification.CONTENT_REPORT_PENDING; }
            if (report.ReportStatus == ReportStatus.ACCEPT) { newStatus =  ContentNotification.CONTENT_REPORT_ACCEPT; }
            if (report.ReportStatus == ReportStatus.DENY) { newStatus =  ContentNotification.CONTENT_REPORT_DENIED; }
            report.ReportStatus = status;
            await _reportRepository.UpdateAsync(report, true);
            var deviceId = await _mobileRepository.FirstOrDefaultAsync(x => x.EmployeeId == report.EmployeeId);
            var body =  $"Your {report.TypeOfReport} - {report.ReportDate.ToShortDateString()} has been {newStatus} by Admin";

            var noti = new Notification();
            var appleMessage = new AppleMessage();

            noti.title = Enum.ContentNotification.TITLE_REPORT;
            noti.body = body;

            appleMessage.to = deviceId.DeviceId;
            appleMessage.notification = noti;

            HttpClient httpNoti = new HttpClient();
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + FCMKey.ServerKey);
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
            var content = new StringContent(JsonConvert.SerializeObject(appleMessage), System.Text.Encoding.UTF8, "application/json");

            var response = await httpNoti.PostAsync("https://fcm.googleapis.com/fcm/send", content);
        }

        public async Task<ReportDto> GetReportByIdAsync(Guid reportId)
        {
            var report = await _reportRepository.FindAsync(x => x.Id == reportId);
            var reportDto =  ObjectMapper.Map<Report, ReportDto>(report);
            return reportDto.TypeOfReport.IsNullOrWhiteSpace() ? null : reportDto ;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using PBL5.Common;
using PBL5.Employees;
using PBL5.Permissions;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PBL5.Localization;
using Volo.Abp.Domain.Repositories;
using PBL5.ResponseInfos;
using PBL5.TimeSheets;
using PBL5.Reports;
using PBL5.Enum;
using PushSharp.Apple;
using CorePush.Apple;
using System.Net.Http;
using CorePush.Serialization;
using System.IO;
using CorePush.Firebase;
using static PBL5.Mobiles.AppleNotification;
using Newtonsoft.Json;

namespace PBL5.Mobiles
{
    [Authorize(PBL5Permissions.Mobile.Default)]
    public class MobileAppService : ApplicationService, IMobileAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly EmployeeManager _employeeManager;
        private readonly ITimeSheetAppService _timeSheetAppService;
        private readonly IReportRepository _reportRepository;
        private readonly IStringLocalizer<PBL5Resource> _localizer;
        private readonly IMobileRepository _mobileRepository;

        public MobileAppService(
            IEmployeeRepository employeeRepository,
            IStringLocalizer<PBL5Resource> localizer,
            EmployeeManager employeeManager,
            ITimeSheetAppService timeSheetAppService,
            ITimeSheetRepository timeSheetRepository,
            IReportRepository reportRepository,
            IMobileRepository mobileRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _timeSheetAppService = timeSheetAppService;
            _timeSheetRepository = timeSheetRepository;
            _reportRepository = reportRepository;
            _localizer = localizer;
            _mobileRepository = mobileRepository;
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        public async Task<IActionResult> GetEmployeeMobileByEmployeeIdAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.FindAsync(p => p.Id == employeeId);
            return employee is null ?
                new BadRequestObjectResult(_localizer["PBL5:Mobile:0001"]) : new OkObjectResult(ObjectMapper.Map<Employee, EmployeeMobileDto>(employee));
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.ChangePassword)]
        public async Task<IActionResult> ChangePasswordEmployeeMobileAsync(Guid employeeId, string oldPassword, string newPassword)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            var employee = await _employeeRepository.FindAsync(p => p.Id.Equals(employeeId));

            if (employee == null)
            {
                return new BadRequestObjectResult(_localizer["PBL5:Mobile:0001"]);
            }

            if (Security.GetMD5(oldPassword) == employee.Password)
            {
                employee.Password = Security.GetMD5(newPassword);
            }
            else
            {
                responseInfo.Code = Common.CodeResponse.NOT_VALIDATE;
                responseInfo.Message = _localizer["Response:Message:200"];
                return new OkObjectResult(responseInfo);
            }
            await _employeeRepository.UpdateAsync(employee);
            
            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        [Route("/api/app/mobile/sign-in-mobile/{email}/{password}")]
        public async Task<IActionResult> SignInMobileAsync(string email, string password)
        {
            if (email.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
            {
                return new OkObjectResult(_localizer["PBL5:Mobile:0003"]);
            }

            var confirmPassword = Security.GetMD5(password);
            var employee = await _employeeRepository.FindAsync(p => p.Email.Contains(email) && p.Password.Contains(confirmPassword));
            if (employee == null)
            {
                return new OkObjectResult(_localizer["PBL5:Mobile:0003"]);
            }

            return new OkObjectResult(employee.Id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEmployeeMobileInfoAsync(EmployeeMobileDto employeeMobileDto)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            var employee = await _employeeRepository.FindAsync(p => p.Id == employeeMobileDto.Id);
            if (employee == null)
            {
                //Check null Employee Code
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"] + " " + _localizer["PBL5:Mobile:0001"];
                return new OkObjectResult(responseInfo);
            }

            //Valid email
            var inValid = await _employeeRepository.AnyAsync(p => p.Id != employee.Id && p.Email == employeeMobileDto.Email);
            if (inValid)
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:001"] + " " + _localizer["PBL5:0001"];
                return new OkObjectResult(responseInfo);
            }

            //Valid employee code
            inValid = await _employeeRepository.AnyAsync(p => p.Id != employee.Id && p.EmployeeCode == employeeMobileDto.EmployeeCode);
            if (inValid)
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:001"] + " " + _localizer["PBL5:Mobile:0002"];
                return new OkObjectResult(responseInfo);
            }

            //Valid Identity card
            inValid = await _employeeRepository.AnyAsync(p => p.Id != employee.Id && p.IdentityCard == employeeMobileDto.IdentityCard);
            if (inValid)
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:001"] + " " + _localizer["PBL5:0004"];
                return new OkObjectResult(responseInfo);
            }

            var employeeUpdate = ObjectMapper.Map<EmployeeMobileDto, Employee>(employeeMobileDto);
            _employeeManager.UpdateEmployee(employeeUpdate, employee);
            await _employeeRepository.UpdateAsync(employee);
            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        public async Task<IActionResult> GetTimeSheetByTimeMobileAsync(Guid employeeId, DateTime? timeSheetByMonth)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            TimeSheetSearchDto searchDto = new TimeSheetSearchDto();
            searchDto.EmployeeId = employeeId;
            searchDto.DateSearch = timeSheetByMonth;
            if (employeeId != null)
            {
                var result = await _timeSheetAppService.GetTimeSheetByDateAsync(searchDto);
                return new OkObjectResult(result);
            }
            else
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:001"] + " " + _localizer["PBL5:Mobile:0002"];
                return new OkObjectResult(responseInfo);
            }
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        public async Task<IActionResult> GetTotalTimeWorkInADay(Guid id)
        {
            return new OkObjectResult(await _timeSheetAppService.CountTimeWorkInDay(id));
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        public async Task<IActionResult> GetTotalTimeWorkInAMonth(Guid employeeId, DateTime? timeSheetByMonth)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            TimeSheetSearchDto searchDto = new TimeSheetSearchDto();
            searchDto.EmployeeId = employeeId;
            searchDto.DateSearch = timeSheetByMonth;
            if (employeeId != null)
            {
                return new OkObjectResult(await _timeSheetAppService.CountTimeWorkInMonth(searchDto));
            }
            else
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:001"] + " " + _localizer["PBL5:Mobile:0002"];
                return new OkObjectResult(responseInfo);
            }
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        public async Task<IActionResult> GetHistoryReportMobileAsync(Guid employeeId)
        {
            var listReport = await _reportRepository.GetListAsync(x => x.EmployeeId == employeeId);
            var repo = ObjectMapper.Map<List<Report>, List<ReportDto>>(listReport);
            return new OkObjectResult(repo);
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.ReportTimeSheet)]
        [HttpPost]
        public async Task<IActionResult> ReportTimeSheetMobileAsync(CreateReportMobileDto input)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            if (input == null)
            {
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }

            var timeSheet = await _timeSheetRepository.FindAsync(x => x.Employee.Id == input.EmployeeId && x.DateCheckIn.Date == input.ReportDate.Date);
            if (timeSheet != null)
            {
                var report = new Report();
                report.EmployeeId = input.EmployeeId;
                report.ReportDate = timeSheet.DateCheckIn;
                report.TypeOfReport = input.TypeOfReport.IsNullOrWhiteSpace() ? ReportingReason.OTHER_ERROR : input.TypeOfReport;
                report.Content = input.Content;
                report.ReportStatus = ReportStatus.PENDING;
                await _reportRepository.InsertAsync(report);
            }
            else
            {
                var report = new Report();
                report.EmployeeId = input.EmployeeId;
                report.ReportDate = input.ReportDate;
                report.TypeOfReport = input.TypeOfReport.IsNullOrWhiteSpace() ? ReportingReason.OTHER_ERROR : input.TypeOfReport;
                report.Content = input.Content;
                report.ReportStatus = ReportStatus.PENDING;
                await _reportRepository.InsertAsync(report);
            }

            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpGet]
        public async Task<IActionResult> StatisticByYear(Guid employeeId, int year)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            var employee = await _employeeRepository.FindAsync(p => p.Id.Equals(employeeId));

            if (employee == null)
            {
                return new BadRequestObjectResult(_localizer["PBL5:Mobile:0001"]);
            }

            var listTimeSheetByMonth =  new List<StatisticTimeSheetByMonthMobileDto>();
            for (int i = 1; i <= 12; i++)
            {
                var timeSheetMonth = new StatisticTimeSheetByMonthMobileDto();
                timeSheetMonth.Month = i;
                (timeSheetMonth.DayOff, timeSheetMonth.DayWork, timeSheetMonth.DayLate) = await _timeSheetRepository.CountStatusTimeSheetByMonth(employeeId, i, year);
                listTimeSheetByMonth.Add(timeSheetMonth);
            }
            var statisticByYear = new StatisticTimeSheetByYearMobileDto();
            statisticByYear.StatisticYear = year;
            statisticByYear.ListTimeSheetMonths = listTimeSheetByMonth;

            return new OkObjectResult(statisticByYear);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SendEmailRestPasswordAsync(string employeeCode)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            var employee = await _employeeRepository.FindAsync(p => p.EmployeeCode.Equals(employeeCode));
            if (employee == null)
            {
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }
            var randomPassword = Security.GenerateRandomPassword();
            employee.Password = Security.GetMD5(randomPassword);

            await _employeeRepository.UpdateAsync(employee);
            
            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        [AllowAnonymous]

        public async Task<IActionResult> CreateDeviceIdFromAccount(Guid employeeId, string deviceId)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            if (await _employeeRepository.AnyAsync(x => x.Id == employeeId))
            {
                if (await _mobileRepository.AnyAsync(x => x.DeviceId == deviceId))
                {
                    var device = await _mobileRepository.FindAsync(x => x.DeviceId == deviceId);
                    device.EmployeeId = employeeId;
                }
                else
                {
                    var newDevice =  new DeviceNotification();
                    newDevice.EmployeeId = employeeId;
                    newDevice.DeviceId = deviceId;

                    await _mobileRepository.InsertAsync(newDevice);
                }
            }
            else
            {
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }

            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        public async Task<IActionResult> SendNotification(Guid employeeId)
        {
            var deviceId = await _mobileRepository.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            string apnDeviceToken = deviceId.DeviceId;

            HttpClient http = new();

            var serverKey = "AAAAUDtw9Tk:APA91bGRWs4df1aDHBLRO922WwEcFM_SEuFdchBpDt2TNlns10ZI-8wkgfV2WggH5R1w5DcTWJz5LWdrbt6t32J9EM1HW7yRzfeV89eKV0DcdQ0DZYVp4kgdh4qWLVX9TxVZzaSndOgJ";
            var noti = new Notification();
            var appleMessage = new AppleMessage();

            noti.title = "TimeSheet";
            noti.body = "Check-in successfully";

            appleMessage.to = deviceId.DeviceId;
            appleMessage.notification = noti;

            HttpClient httpNoti = new HttpClient();
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + serverKey);
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
            var content = new StringContent(JsonConvert.SerializeObject(appleMessage), System.Text.Encoding.UTF8, "application/json");

            var response = await httpNoti.PostAsync("https://fcm.googleapis.com/fcm/send", content);
            ResponseInfo responseInfo = new ResponseInfo();
            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }

        [AllowAnonymous]
        [Authorize(PBL5Permissions.Mobile.GetInfo)]
        [HttpPost]
        public async Task<IActionResult> UpdateReportMobileAsync(UpdateReportMobileDto input)
        {
            ResponseInfo responseInfo = new ResponseInfo(); 
            if (input == null)
            {
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }

            var report = await _reportRepository.FindAsync(input.Id);
            if (report == null)
            {
                responseInfo.Code = Common.CodeResponse.NOT_FOUND;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }
            else
            {
                if (report.ReportStatus == ReportStatus.PENDING)
                {
                    report.Content = input.Content;
                    report.TypeOfReport = input.TypeOfReport.IsNullOrWhiteSpace() ? ReportingReason.OTHER_ERROR : input.TypeOfReport;
                    report.ReportDate = input.ReportDate;
                }
                else
                {
                    responseInfo.Code = Common.CodeResponse.HAVE_ERROR;
                    responseInfo.Message = _localizer["PBL5:Mobile:0004"];
                    return new OkObjectResult(responseInfo);
                }
            }
            
            responseInfo.Code = Common.CodeResponse.OK;
            responseInfo.Message = _localizer["Response:Message:200"];
            return new OkObjectResult(responseInfo);
        }
    }
}
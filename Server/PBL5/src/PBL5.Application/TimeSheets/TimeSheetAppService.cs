using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBL5.TimeSheets;
using System.Linq;
using PBL5.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using PBL5.Employees;
using PBL5.Enum;
using Microsoft.AspNetCore.Mvc;
using PBL5.ResponseInfos;
using Microsoft.Extensions.Localization;
using PBL5.Localization;
using System.Net.Http;
using static PBL5.Mobiles.AppleNotification;
using Newtonsoft.Json;
using PBL5.Mobiles;
using Microsoft.AspNetCore.Http;
using System.IO;
using PBL5.IdentificationImages;
using PBL5.Common;
using Volo.Abp;
using Volo.Abp.Guids;
using Microsoft.AspNetCore.Http.Internal;

namespace PBL5.TimeSheets
{
    [Authorize(PBL5Permissions.TimeSheet.Default)]
    public class TimeSheetAppService : CrudAppService<
    TimeSheet,
    TimeSheetDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateTimeSheetDto,
    CreateUpdateTimeSheetDto>,
    ITimeSheetAppService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly TimeSheetManager _timeSheetManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMobileRepository _mobileRepository;
        private readonly IStringLocalizer<PBL5Resource> _localizer;
        private readonly MobileManager _mobileManager;
        private readonly IGuidGenerator _guidGenerator;
        public TimeSheetAppService(
            ITimeSheetRepository timeSheetRepository,
            TimeSheetManager timeSheetManager,
            IEmployeeRepository employeeRepository,
            IMobileRepository mobileRepository,
            MobileManager mobileManager,
            IGuidGenerator guidGenerator,
            IStringLocalizer<PBL5Resource> localizer
            ) : base(timeSheetRepository)
        {
            GetPolicyName = PBL5Permissions.TimeSheet.Default;
            GetListPolicyName = PBL5Permissions.TimeSheet.Default;
            CreatePolicyName = PBL5Permissions.TimeSheet.Create;
            UpdatePolicyName = PBL5Permissions.TimeSheet.Update;
            DeletePolicyName = PBL5Permissions.TimeSheet.Delete;
            _timeSheetRepository = timeSheetRepository;
            _timeSheetManager = timeSheetManager;
            _employeeRepository = employeeRepository;
            _localizer = localizer;
            _mobileRepository = mobileRepository;
            _mobileManager = mobileManager;
            _guidGenerator = guidGenerator;
        }

        public async Task<PagedResultDto<TimeSheetDto>> SearchListAsync(ConditionSearchDto input)
        {
            var (total, result) = await _timeSheetRepository.SearchListAsync(
                input.KeySearch,
                input.DayWork,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting);

            var finalResult = ObjectMapper.Map<List<TimeSheet>, List<TimeSheetDto>>(result);
            return new(total, finalResult);
        }

        [AllowAnonymous]
        public async Task<IActionResult> TimeSheetForEmployee(string employeeCode, IFormFile fileData)
        {
            ResponseInfo responseInfo = new ResponseInfo();
            DateTime checkInTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9,0,0);
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            var utcN = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone).AddHours(Constants.TimeZoneVietNam);
           
            if (employeeCode.IsNullOrWhiteSpace())
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }
            var employee = await _employeeRepository.FindAsync(p => p.EmployeeCode.Equals(employeeCode));

            if (employee == null)
            {
                responseInfo.Code = Common.CodeResponse.INFO;
                responseInfo.Message = _localizer["Response:Message:404"];
                return new OkObjectResult(responseInfo);
            }

            var isExistRecordCheckIn = await _timeSheetRepository.AnyAsync(p => p.EmployeeId == employee.Id && p.DateCheckIn.Date == utcN.Date );
            if (isExistRecordCheckIn)
            {
                //Sửa timeSheet
                var timeSheet = await _timeSheetRepository.FirstOrDefaultAsync(p => p.EmployeeId == employee.Id && p.DateCheckIn.Date == utcN.Date);
                timeSheet.IdentificationImageCheckOut = await SaveFileAsync(fileData, timeSheet.Id, false);
                await _timeSheetRepository.UpdateAsync(await _timeSheetManager.CheckOut(timeSheet), false);

                responseInfo.Code = Common.CodeResponse.OK;
                responseInfo.Message = _localizer["Response:Message:200"];

                var deviceId = await _mobileRepository.FirstOrDefaultAsync(x => x.EmployeeId == employee.Id);
                if (deviceId != null)
                {
                    var body = $"{employee.Name}-{employee.EmployeeCode} {timeSheet.CheckOutTime} {Enum.ContentNotification.CONTENT_CHECK_OUT}";
                    var result = await _mobileManager.SendNotificationAPN(deviceId.DeviceId, Enum.ContentNotification.TITLE, body);
                }
                responseInfo.Code = Common.CodeResponse.OK;
                responseInfo.Message = _localizer["Response:Message:200"];
            }
            else
            { 
                //Tạo TimeSheet
                var timeSheet = new TimeSheet();
                timeSheet.EmployeeId = employee.Id;
                timeSheet.CheckInTime = utcN;
                timeSheet.CheckOutTime = utcN;
                // timeSheet.CheckInTime = DateTime.Now;
                // timeSheet.CheckOutTime = DateTime.Now;
                timeSheet.DateCheckIn = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone).AddHours(Constants.TimeZoneVietNam).Date;
                timeSheet.IsAbsent = false;
                timeSheet.WorkStatus = timeSheet.CheckInTime > checkInTime ? WorkStatus.LATE : WorkStatus.ON_TIME;
                timeSheet.IdentificationImageCheckIn = null;
                timeSheet.Description = null;
                await _timeSheetRepository.InsertAsync(timeSheet, true);

                var timeSheetInserted = await _timeSheetRepository.FirstOrDefaultAsync(p => p.EmployeeId == employee.Id && p.DateCheckIn.Date == utcN.Date);
                timeSheetInserted.IdentificationImageCheckIn = await SaveFileAsync(fileData, timeSheet.Id, true);

                await _timeSheetRepository.UpdateAsync(timeSheetInserted, true);
                var deviceIds = await _mobileRepository.GetListAsync(x => x.EmployeeId == employee.Id);
                if(deviceIds != null)
                {
                    foreach(var deviceId in deviceIds)
                    {   
                        var body = $"{employee.Name}-{employee.EmployeeCode} {timeSheet.CheckInTime} {Enum.ContentNotification.CONTENT_CHECK_IN}";
                        var result = await _mobileManager.SendNotificationAPN(deviceId.DeviceId, Enum.ContentNotification.TITLE, body);
                    }
                }

                responseInfo.Code = Common.CodeResponse.OK;
                responseInfo.Message = _localizer["Response:Message:200"];
            }

            return new OkObjectResult(responseInfo);
        }

        public async Task<(int, double)> CountEmployeesHaveCheckInAsync(DateTime timeToCount)
        {
            var totalEmployeeCheckIn = await _timeSheetRepository.CountAsync(p => p.DateCheckIn.Date.Equals(DateTime.Now.Date));
            var listEmployee = await _employeeRepository.GetListAsync();
            var totalTimeWork = 0.0;
            foreach (var employee in listEmployee)
            {
                var (count, oneEmployeeTimeSheet) = await _timeSheetRepository.GetTimeSheetByDateAsync(employee.EmployeeCode, timeToCount);
                var oneEmployeeTime = 0.0;
                foreach (var time in oneEmployeeTimeSheet)
                {
                    oneEmployeeTime = (time.CheckOutTime - time.CheckInTime).TotalHours;
                }
                totalTimeWork += oneEmployeeTime;
            }

            return (totalEmployeeCheckIn, totalTimeWork);
        }

        [AllowAnonymous]
        public async Task<PagedResultDto<StatisticDto>> GetTimeSheetByDateAsync(TimeSheetSearchDto timeSheetSearchDto)
        {
            var emp = await _employeeRepository.FindAsync(p => p.Id == timeSheetSearchDto.EmployeeId);
            if (emp != null)
            {
                var (total, listTimeSheet) = await _timeSheetRepository.GetTimeSheetByDateAsync(emp.EmployeeCode, timeSheetSearchDto.DateSearch);
                var result = ObjectMapper.Map<List<TimeSheet>, List<StatisticDto>>(listTimeSheet);
                foreach (var item in result)
                {
                    item.TotalTimeDateWork = Math.Round((item.CheckOutTime - item.CheckInTime).TotalHours, 2);
                }
                return new PagedResultDto<StatisticDto>(total, result);
            }
            return null;
        }

        [AllowAnonymous]
        public async Task<TimeSheetDto> GetDetailTimeSheetAsync(Guid id)
        {
            var result = ObjectMapper.Map<TimeSheet, TimeSheetDto>(await _timeSheetRepository.FindAsync(id));
            return  result;
        }

        [AllowAnonymous]
        public async Task<double> CountTimeWorkInDay(Guid id)
        {
            var result = await _timeSheetRepository.GetAsync(id);
            return Math.Round((result.CheckOutTime - result.CheckInTime).TotalHours, 2) - 2;
        }

        [AllowAnonymous]
        public async Task<double> CountTimeWorkInMonth(TimeSheetSearchDto timeSheetSearchDto)
        {
                var emp = await _employeeRepository.FindAsync(p => p.Id == timeSheetSearchDto.EmployeeId);
                if (emp != null)
                {
                    var (total, listTimeSheet) = await _timeSheetRepository.GetTimeSheetByDateAsync(emp.EmployeeCode, timeSheetSearchDto.DateSearch);
                    var result = ObjectMapper.Map<List<TimeSheet>, List<StatisticDto>>(listTimeSheet);
                    var totalTime = 0.0;
                    foreach (var item in result)
                    {
                        totalTime += Math.Round((item.CheckOutTime - item.CheckInTime).TotalHours, 2) - 2;
                    }
                    return totalTime;
                }
            return 0;
        }

        public async Task<string> SaveFileAsync(IFormFile fileData, Guid timeSheetId,bool isCheckIn)
        {
            try
            {
                // đổi file name
                string fileExtension = Path.GetExtension(fileData.FileName);
                string filePath = "";
                if (isCheckIn)
                {
                    filePath = Path.Combine($"./wwwroot/UploadFile/", $"1-{timeSheetId}{fileExtension}");
                }
                else
                {
                    filePath = Path.Combine($"./wwwroot/UploadFile/", $"0-{timeSheetId}{fileExtension}");
                }
                // Tạo đường dẫn mới cho file

                // Đọc dữ liệu từ IFormFile và lưu vào thư mục đích
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileData.CopyToAsync(fileStream);
                }
                // var newUrl = $"/PBL5.Web/UploadFile/{timeSheetId}.{fileType}";
                return filePath;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error {ex}");
            }
        }

        public async Task<string> GetPathFileFromTimeSheetAsync(Guid timeSheetId, bool isCheckIn)
        {
            var timeSheet = await _timeSheetRepository.FindAsync(timeSheetId);
            if (isCheckIn)
            {
                return timeSheet != null ? timeSheet.IdentificationImageCheckIn : $"./wwwroot/UploadFile/profile.jpg";
            }
            else
            {
                return timeSheet != null ? timeSheet.IdentificationImageCheckOut : $"./wwwroot/UploadFile/profile.jpg";
            }
        }

        public async Task<DetailEmployeeTimeSheet> GetDetailEmployeeTimeSheetAsync(Guid employeeId, DateTime month)
        {
            var employee = await _employeeRepository.FindAsync(p => p.Id.Equals(employeeId));
            var detailEmployeeTimeSheet =  new DetailEmployeeTimeSheet();
            detailEmployeeTimeSheet.EmployeeCode = employee.EmployeeCode;
            detailEmployeeTimeSheet.Name = employee.Name;
            (detailEmployeeTimeSheet.DayOff, 
            detailEmployeeTimeSheet.DayWork, 
            detailEmployeeTimeSheet.DayLate) = await _timeSheetRepository.CountStatusTimeSheetByMonth(employeeId, month.Date.Month,  month.Date.Year);
            var timeSheetSearchDto = new TimeSheetSearchDto();
            timeSheetSearchDto.EmployeeId = employeeId;
            timeSheetSearchDto.DateSearch = month;
            detailEmployeeTimeSheet.TotalTimeWorkInAMonth = (float)await CountTimeWorkInMonth(timeSheetSearchDto);

            // var dateSearch = searchDto.DateSearch.HasValue ? searchDto.DateSearch.Value : DateTime.Now;
            // var employee = await _employeeRepository.FindAsync(p => p.Id.Equals(searchDto.EmployeeId));
            // var detailEmployeeTimeSheet =  new DetailEmployeeTimeSheet();
            // detailEmployeeTimeSheet.EmployeeCode = employee.EmployeeCode;
            // detailEmployeeTimeSheet.Name = employee.Name;
            // (detailEmployeeTimeSheet.DayOff, 
            // detailEmployeeTimeSheet.DayWork, 
            // detailEmployeeTimeSheet.DayLate) = await _timeSheetRepository.CountStatusTimeSheetByMonth(searchDto.EmployeeId, dateSearch.Date.Month,  dateSearch.Date.Year);
            // detailEmployeeTimeSheet.TotalTimeWorkInAMonth = (float)await CountTimeWorkInMonth(searchDto);
            
            return detailEmployeeTimeSheet;
        }
    }
}
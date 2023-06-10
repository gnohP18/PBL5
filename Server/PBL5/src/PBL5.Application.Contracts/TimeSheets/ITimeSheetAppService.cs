using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL5.TimeSheets;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

public interface ITimeSheetAppService : ICrudAppService<
    TimeSheetDto, 
    Guid, 
    PagedAndSortedResultRequestDto, 
    CreateUpdateTimeSheetDto, 
    CreateUpdateTimeSheetDto>, 
    IApplicationService
{
    Task<PagedResultDto<TimeSheetDto>> SearchListAsync(ConditionSearchDto input);
    Task<IActionResult> TimeSheetForEmployee(string employeeCode, IFormFile FileData );
    Task<(int, double)> CountEmployeesHaveCheckInAsync(DateTime timeToCount);
    Task<PagedResultDto<StatisticDto>> GetTimeSheetByDateAsync(TimeSheetSearchDto timeSheetSearchDto);
    Task<TimeSheetDto> GetDetailTimeSheetAsync(Guid id);
    Task<double> CountTimeWorkInDay(Guid id);
    Task<double> CountTimeWorkInMonth(TimeSheetSearchDto timeSheetSearchDto);
    Task<string> SaveFileAsync(IFormFile fileData, Guid timeSheetId, bool isCheckIn);
    Task<string> GetPathFileFromTimeSheetAsync(Guid identificationImages, bool isCheckIn);
    Task<DetailEmployeeTimeSheet> GetDetailEmployeeTimeSheetAsync(Guid employeeId, DateTime month);
}
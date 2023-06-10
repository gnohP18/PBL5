using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PBL5.Reports
{
    public interface IReportAppService :
    ICrudAppService<
        ReportDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateReportDto, 
        UpdateReportDto>
    {
        Task<PagedResultDto<ReportDto>> SearchListAsync(ConditionSearchDto input);
        Task ReportTimeSheetAsync(CreateReportDto input);
        Task<List<ReportDto>> GetHistoryReportAsync(Guid employeeId);
        Task ChangeStatusReportAsync(Guid reportId, string status);
        Task<ReportDto> GetReportByIdAsync(Guid reportId);
    }
}
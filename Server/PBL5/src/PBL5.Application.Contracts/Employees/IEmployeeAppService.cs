using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PBL5.Employees
{
    public interface IEmployeeAppService : ICrudAppService<
        EmployeeDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateEmployeeDto, 
        CreateUpdateEmployeeDto>
    {
        Task<bool> ValidInfoEmployeeAsync(CreateUpdateEmployeeDto input);
        Task UpdateEmployeeAsync(CreateUpdateEmployeeDto input);
        Task<PagedResultDto<EmployeeDto>> SearchListAsync(ConditionSearchDto input);
        Task<EmployeeDto> GetEmployeeByEmployeeCodeAsync(string employeeCode);
        Task ChangePasswordAsync(Guid id, string newPassword);
        Task<List<EmployeeDto>> GetListEmployeeAsync();
    }
}
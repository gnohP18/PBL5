using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PBL5.Common;
using PBL5.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PBL5.Employees
{
    [Authorize(PBL5Permissions.Employee.Default)]
    public class EmployeeAppService : CrudAppService<
        Employee,
        EmployeeDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateEmployeeDto,
        CreateUpdateEmployeeDto>,
        IEmployeeAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly EmployeeManager _employeeManager;

        public EmployeeAppService(IEmployeeRepository employeeRepository, EmployeeManager employeeManager) : base(employeeRepository)
        {
            GetPolicyName = PBL5Permissions.Employee.Default;
            GetListPolicyName = PBL5Permissions.Employee.Default;
            CreatePolicyName = PBL5Permissions.Employee.Create;
            UpdatePolicyName = PBL5Permissions.Employee.Update;
            DeletePolicyName = PBL5Permissions.Employee.Delete;
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
        }

        public async Task<PagedResultDto<EmployeeDto>> SearchListAsync(ConditionSearchDto input)
        {
            var (total, result) = await _employeeRepository.SearchListAsync(
                input.KeySearch,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting);
            var finalResult = ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(result);
            return new(total, finalResult);
        }

        public async Task<EmployeeDto> GetEmployeeByEmployeeCodeAsync(string employeeCode)
        {
            if (employeeCode.IsNullOrWhiteSpace())
            {
                return null;
            }
            var employee = await _employeeRepository.FindAsync(p => p.EmployeeCode == employeeCode);
            return employee is null ? null : ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }

        public async Task<bool> ValidInfoEmployeeAsync(CreateUpdateEmployeeDto input)
        {
            var employee = ObjectMapper.Map<CreateUpdateEmployeeDto, Employee>(input);
            return await _employeeManager.ValidInfoEmployeeAsync(employee);
        }


        public async Task UpdateEmployeeAsync(CreateUpdateEmployeeDto input)
        {
            var employeeUpdate = ObjectMapper.Map<CreateUpdateEmployeeDto, Employee>(input);
            var employee = await _employeeRepository.GetAsync(input.Id);
            if (employee != null)
            {
                _employeeManager.UpdateEmployee(employeeUpdate, employee);
                await _employeeRepository.UpdateAsync(employee);
            }
        }

        public async Task ChangePasswordAsync(Guid id, string newPassword)
        {
            if (newPassword.IsNullOrWhiteSpace())
            {
                throw new WrongNewPasswordOfEmployee();
            }
            var employee = await _employeeRepository.GetAsync(p => p.Id == id);
            
            employee.Password = Security.GetMD5(newPassword);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<List<EmployeeDto>> GetListEmployeeAsync()
        {
            return ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(await _employeeRepository.GetListAsync());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace PBL5.Employees
{
    public class EmployeeManager : DomainService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeManager(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> ValidInfoEmployeeAsync(Employee input)
        {
            if (input.Id == null)
            {
                var employee = await _employeeRepository.AnyAsync(v => v.Email == input.Email);
                if (employee)
                {
                    throw new ExistEmailOfEmployee();
                }

                employee = await _employeeRepository.AnyAsync(v => v.IdentityCard == input.IdentityCard);
                if (employee)
                {
                    throw new ExistIdentityCardOfEmployee();
                }

                employee = await _employeeRepository.AnyAsync(v => v.EmployeeCode == input.EmployeeCode);
                if (employee)
                {
                    throw new ExistEmployeeCodeOfEmployee();
                }
            }
            else
            {
                var employee = await _employeeRepository.AnyAsync(v => v.Email == input.Email && v.Id != input.Id);
                if (employee)
                {
                    throw new ExistEmailOfEmployee();
                }

                employee = await _employeeRepository.AnyAsync(v => v.IdentityCard == input.IdentityCard && v.Id != input.Id);
                if (employee)
                {
                    throw new ExistIdentityCardOfEmployee();
                }

                employee = await _employeeRepository.AnyAsync(v => v.EmployeeCode == input.EmployeeCode && v.Id != input.Id);
                if (employee)
                {
                    throw new ExistEmployeeCodeOfEmployee();
                }
            }
                
            return true;
        }

        public Employee UpdateEmployee(Employee employeeUpdate, Employee employee)
        {
            employee.Name = employeeUpdate.Name;
            employee.Gender = employeeUpdate.Gender;
            employee.DateOfBirth = employeeUpdate.DateOfBirth;
            employee.Address = employeeUpdate.Address;
            employee.IdentityCard = employeeUpdate.IdentityCard;
            employee.Email = employeeUpdate.Email;
            employee.Phone = employeeUpdate.Phone;
            employee.EmployeeCode = employeeUpdate.EmployeeCode;

            return employee;
        }
    }
}
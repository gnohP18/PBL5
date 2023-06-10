using System;
using System.Threading.Tasks;
using PBL5.Employees;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace PBL5
{
    public class PBL5DataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IEmployeeRepository _employeeRepository;
        public PBL5DataSeederContributor(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedEmployee();
        }

        private async Task SeedEmployee()
        {
            if (await _employeeRepository.AnyAsync())
            {
                return;
            }

            await _employeeRepository.InsertAsync(
                new Employee
                {
                    Name = "Nguyen Hoang Phong",
                    Gender = Enum.Gender.Male,
                    DateOfBirth = new DateTime(2002, 04, 18),
                    Address = "Tam Ky, Quang Nam",
                    IdentityCard = "101",
                    Email = "user1@gmail.com",
                    Phone = "0912345671",
                    EmployeeCode = "10001",
                    Password = "21232f297a57a5a743894a0e4a801fc3"
                });
            await _employeeRepository.InsertAsync(
                new Employee
                {
                    Name = "Ho Dac Nguyen Minh",
                    Gender = Enum.Gender.Female,
                    DateOfBirth = new DateTime(2002, 01, 01),
                    Address = "Hue, Hue",
                    IdentityCard = "102",
                    Email = "user2@gmail.com",
                    Phone = "0912345672",
                    EmployeeCode = "10002",
                    Password = "21232f297a57a5a743894a0e4a801fc3"
                });
            await _employeeRepository.InsertAsync(
                new Employee
                {
                    Name = "Tăng Lê Thanh Tú",
                    Gender = Enum.Gender.Female,
                    DateOfBirth = new DateTime(2002, 01, 01),
                    Address = "Sơn Tra, Da Nang",
                    IdentityCard = "103",
                    Email = "user3@gmail.com",
                    Phone = "0912345673",
                    EmployeeCode = "10003",
                    Password = "21232f297a57a5a743894a0e4a801fc3"
                });
            await _employeeRepository.InsertAsync(
                new Employee
                {
                    Name = "Dinh Quang Hiep",
                    Gender = Enum.Gender.Male,
                    DateOfBirth = new DateTime(2002, 01, 01),
                    Address = "Sơn Tra, Da Nang",
                    IdentityCard = "104",
                    Email = "user4@gmail.com",
                    Phone = "0912345674",
                    EmployeeCode = "10004",
                    Password = "21232f297a57a5a743894a0e4a801fc3"
                });
        }
    }
}
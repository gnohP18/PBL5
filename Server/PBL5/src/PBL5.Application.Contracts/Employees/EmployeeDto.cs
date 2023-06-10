using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Enum;
using Volo.Abp.Application.Dtos;

namespace PBL5.Employees
{
    public class EmployeeDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string GenderName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string IdentityCard { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string EmployeeCode { get; set; }
        
        public string Password { get; set; }
    }
}
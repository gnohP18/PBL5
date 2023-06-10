using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Enum;
using Volo.Abp.Application.Dtos;

namespace PBL5.Employees
{
    public class CreateUpdateEmployeeDto :  EntityDto<Guid>
    {
        /// <summary>
        /// Họ nhân viên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>

        public Gender Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// CMND/CCCD
        /// </summary>
        public string IdentityCard { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Password { get; set; }
    }
}
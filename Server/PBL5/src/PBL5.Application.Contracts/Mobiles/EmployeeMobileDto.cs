using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PBL5.Enum;
using Volo.Abp.Application.Dtos;

namespace PBL5.Mobiles
{
    public class EmployeeMobileDto :   EntityDto<Guid>
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [Required]
        public Gender Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// CMND/CCCD
        /// </summary>
        [Required]
        public string IdentityCard { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required]
        public string EmployeeCode { get; set; }
    }
}
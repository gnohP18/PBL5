using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PBL5.TimeSheets
{
    public class TimeSheetDto : EntityDto<Guid>
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Thời gian Checkin
        /// </summary>
        public DateTime CheckInTime { get; set; }

        /// <summary>
        /// Thời gian Checkout
        /// </summary>
        public DateTime CheckOutTime { get; set; }

        /// <summary>
        /// Ngày Checkin
        /// </summary>
        public DateTime DateCheckIn { get; set; }

        /// <summary>
        /// Vắng mặt
        /// </summary>
        public bool IsAbsent { get; set; }

        /// <summary>
        /// Trạng thái 
        /// </summary>
        public string WorkStatus { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
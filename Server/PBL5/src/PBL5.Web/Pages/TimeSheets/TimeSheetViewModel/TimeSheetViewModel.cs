using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Web.Pages.TimeSheets
{
    public class TimeSheetViewModel
    {
        /// <summary>
        /// Họ và tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CMND/CCCD
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
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
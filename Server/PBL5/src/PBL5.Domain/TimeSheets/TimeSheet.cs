using System;
using System.ComponentModel.DataAnnotations;
using PBL5.IdentificationImages;
using PBL5.Employees;
using Volo.Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using PBL5.Reports;

namespace PBL5.TimeSheets
{
    public class TimeSheet :  FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Thời gian Checkin
        /// </summary>
        [Required]
        public DateTime CheckInTime { get; set; }

        /// <summary>
        /// Thời gian Checkout
        /// </summary>
        [Required]
        public DateTime CheckOutTime { get; set; }

        /// <summary>
        /// Ngày Checkin
        /// </summary>
        [Required]
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

        /// <summary>
        /// Link ảnh nhận diện lúc check-in
        /// </summary>
        public string IdentificationImageCheckIn { get; set; }
        /// <summary>
        /// Link ảnh nhận diện lúc check-out
        /// </summary>
        public string IdentificationImageCheckOut { get; set; }

        public Guid EmployeeId { get; set; }


        public Guid ReportId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
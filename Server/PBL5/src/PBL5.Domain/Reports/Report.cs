using System;
using PBL5.Employees;
using Volo.Abp.Domain.Entities.Auditing;

namespace PBL5.Reports
{
    public class Report :  FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Ngày report
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Trạng thái report
        /// </summary>
        public string ReportStatus { get; set; }

        /// <summary>
        /// Loại Report
        /// </summary>
        public string TypeOfReport { get; set; }


        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PBL5.Reports
{
    public class CreateReportDto
    {

        /// <summary>
        /// TimeSheetId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Trạng thái report
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Loại Report
        /// </summary>
        public string TypeOfReport { get; set; }

    }
}
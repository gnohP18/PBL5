using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Mobiles
{
    public class CreateReportMobileDto
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
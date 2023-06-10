using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PBL5.Web.Pages.Reports
{
    public class SolveReportViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        /// <summary>
        /// Loại Report
        /// </summary>
        [Display(Name = "Type of report")]
        [HiddenInput]
        public string TypeOfReport { get; set; }

        /// <summary>
        /// Trạng thái report
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Report Date")]
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Trạng thái report
        /// </summary>
        [Required]
        [Display(Name = "Report Status")]
        public string ReportStatus { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        [Display(Name = "Report Content")]
        public string Content { get; set; }

        [HiddenInput]
        public Guid EmployeeId { get; set; }
    }
}
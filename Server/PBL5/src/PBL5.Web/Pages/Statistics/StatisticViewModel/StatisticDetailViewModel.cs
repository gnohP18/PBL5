using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Web.Pages.Statistics.StatisticViewModel
{
    public class StatisticDetailViewModel
    {
        [Display(Name = "Employee:EmployeeCode")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee:Name")]
        public string Name { get; set; }

        [Display(Name = "Employee:TotalTimeWork")]
        public float TotalTimeMonthWork { get; set; } 
    }
}
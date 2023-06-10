using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Web.Pages.Statistics.StatisticViewModel
{
    public class DetailEmployeeViewModel
    {
        [Display(Name = "Employee:EmployeeCode")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee:Name")]
        public string Name { get; set; }

        [Display(Name = "Total time work in a month")]
        public float TotalTimeWorkInAMonth { get; set; }

        [Display(Name = "Days off")]
        public int DayOff { get; set; }

        [Display(Name = "Days late")]
        public int DayLate { get; set; }

        [Display(Name = "Days late")]
        public int DayWork { get; set; }
    }
}
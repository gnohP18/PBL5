using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL5.Web.Pages.TimeSheets
{
    public class CreateUpdateTimeSheetViewModel
    {
        public string EmployeeCode { get; set; }
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public DateTime DateCheckIn { get; set; } = DateTime.Now;
        public bool IsAbsent { get; set; } = false;
        public string Description { get; set; }
    }
}
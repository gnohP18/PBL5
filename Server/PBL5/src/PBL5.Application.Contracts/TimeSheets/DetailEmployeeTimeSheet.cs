using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PBL5.TimeSheets
{
    public class DetailEmployeeTimeSheet :  EntityDto<Guid>
    {
        public string EmployeeCode { get; set; }

        public string Name { get; set; }

        public float TotalTimeWorkInAMonth { get; set; }

        public float DayOff { get; set; }

        public float DayLate { get; set; }

        public float DayWork { get; set; }
    }
}
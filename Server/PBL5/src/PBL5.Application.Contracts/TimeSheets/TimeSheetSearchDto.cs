using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFilterer.Types;
using Volo.Abp.Application.Dtos;

namespace PBL5.TimeSheets
{
    public class TimeSheetSearchDto
    {
        public Guid EmployeeId { get; set; }
        public DateTime? DateSearch { get; set; }
    }
}
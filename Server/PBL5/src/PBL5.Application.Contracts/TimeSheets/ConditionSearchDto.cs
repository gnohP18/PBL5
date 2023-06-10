using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFilterer.Types;
using Volo.Abp.Application.Dtos;

namespace PBL5.TimeSheets
{
    public class ConditionSearchDto : FilterBase, IPagedAndSortedResultRequest
    {
        public string KeySearch { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime DayWork { get; set; }
        public int SkipCount {get; set; }
        public int TotalCount {get; set; }
        public string Sorting {get; set; }
        public int MaxResultCount { get ;set; }
    }
}
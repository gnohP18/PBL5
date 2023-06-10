using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using Volo.Abp.Application.Dtos;

namespace PBL5.Employees
{
    public class ConditionSearchDto : FilterBase, IPagedAndSortedResultRequest
    {
        [CompareTo(typeof(ToLowerContainsComparisonAttribute),
            nameof(EmployeeDto.Name),
            nameof(EmployeeDto.Phone))]
        [StringFilterOptions(StringFilterOption.Contains)]
        public string KeySearch { get; set; }
        public Guid? IdentificationImageId { get; set; }
        public Guid? TimeSheetId { get; set; }
        public int SkipCount {get; set; }
        public int MaxResultCount {get; set; }
        public string Sorting {get; set; }
    }
}
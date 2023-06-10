using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Common;
using PBL5.TimeSheets;
using PBL5.Web.Pages.Statistics.StatisticViewModel;

namespace PBL5.Web.Pages.Statistics
{
    public class DetailEmployeeStatisticModal : PBL5PageModel
    {
        [BindProperty]
        public DetailEmployeeViewModel ViewModel { get; set; }

        private readonly ITimeSheetAppService _timeSheetAppService;

        public DetailEmployeeStatisticModal(ITimeSheetAppService timeSheetAppService)
        {
            _timeSheetAppService = timeSheetAppService;
        }

        public virtual async Task OnGetAsync(Guid employeeId, DateTime month)
        {
            var timeSheetDto = await _timeSheetAppService.GetDetailEmployeeTimeSheetAsync(employeeId, month);
            ViewModel = ObjectMapper.Map<DetailEmployeeTimeSheet, DetailEmployeeViewModel>(timeSheetDto);
        }
    }
}
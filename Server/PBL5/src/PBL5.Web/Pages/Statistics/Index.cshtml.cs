using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL5.Employees;
using PBL5.Web.Pages.Statistics.StatisticViewModel;

namespace PBL5.Web.Pages.Statistics
{
    public class IndexModel : PBL5PageModel
    {
        [BindProperty]
        public StatisticTotalViewModel TotalViewModel{ get; set; }

        [BindProperty]
        public StatisticDetailViewModel DetailViewModel{ get; set; }

        [BindProperty]
        public SearchEmployeeViewModel SearchViewModel{ get; set; }

        public List<SelectListItem> ListEmployees{ get; set; }

        private readonly IEmployeeAppService _employeeAppService;
        private readonly ITimeSheetAppService _timeSheetAppService;

        public IndexModel(
            IEmployeeAppService employeeAppService, 
            ITimeSheetAppService timeSheetAppService)
        {
            _employeeAppService = employeeAppService;
            _timeSheetAppService = timeSheetAppService;
        }

        public async Task OnGetAsync()
        {
            TotalViewModel =  new StatisticTotalViewModel();
            SearchViewModel = new SearchEmployeeViewModel();
            DetailViewModel =  new StatisticDetailViewModel();

            (TotalViewModel.TotalEmployeeCheckIn, TotalViewModel.TotalTimeEmployeeWork) = await _timeSheetAppService.CountEmployeesHaveCheckInAsync(DateTime.Now);
            TotalViewModel.TotalTimeEmployeeWork = Math.Round(TotalViewModel.TotalTimeEmployeeWork, 2);
            var listEm =  await _employeeAppService.GetListEmployeeAsync();
            ListEmployees = listEm?.Select(x => 
                new SelectListItem() 
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToList();
        }
    }

    public class StatisticTotalViewModel
    {
        [Display(Name = "Employee:TotalEmployeeCheckIn")]
        public int TotalEmployeeCheckIn { get; set; }

        [Display(Name = "Employee:TotalTimeEmployeeWork")]
        public double TotalTimeEmployeeWork { get; set; }
    }

    public class SearchEmployeeViewModel
    {
        [Display(Name = "Employee:Name")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Statistic:DateSearch")]
        [DataType(DataType.Date)]
        public DateTime DateSearch { get; set; } = DateTime.Now.Date;
    }
}
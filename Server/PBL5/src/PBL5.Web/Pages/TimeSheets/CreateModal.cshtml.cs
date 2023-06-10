using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Employees;

namespace PBL5.Web.Pages.TimeSheets
{
    public class CreateModalModel : PBL5PageModel
    {
        [BindProperty]
        public CreateUpdateTimeSheetViewModel ViewModel { get; set; }
        
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ITimeSheetAppService _timeSheetAppService;

        public CreateModalModel(
            ITimeSheetAppService timeSheetAppService, 
            IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
            _timeSheetAppService = timeSheetAppService;
        }

        public void OnGet()
        {
            ViewModel = new CreateUpdateTimeSheetViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var employee = await _employeeAppService.GetEmployeeByEmployeeCodeAsync(ViewModel.EmployeeCode);
            // await _timeSheetAppService.TimeSheetForEmployee(employee.EmployeeCode);
            return NoContent();
        }
    }
}
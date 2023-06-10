using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PBL5.Employees;

namespace PBL5.Web.Pages.Employees
{
    public class ChangePasswordModalModel : PBL5PageModel
    {
        [BindProperty]
        public ChangePasswordViewModel ViewModel { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public ChangePasswordModalModel(EmployeeAppService employeeAppService){
            _employeeAppService = employeeAppService;
        }

        public void OnGet(Guid id)
        {
            ViewModel = new ChangePasswordViewModel();
            ViewModel.Id = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ViewModel.Password.IsNullOrWhiteSpace())
            {
                await _employeeAppService.ChangePasswordAsync(ViewModel.Id, ViewModel.Password);
            }
            return NoContent();
        }
    }
}
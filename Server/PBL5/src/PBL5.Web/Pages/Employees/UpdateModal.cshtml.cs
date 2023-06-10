using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Common;
using PBL5.Employees;

namespace PBL5.Web.Pages.Employees
{
    public class UpdateModalModel : PBL5PageModel
    {
        [BindProperty]
        public UpdateEmployeeViewModel ViewModel { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public UpdateModalModel(EmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        public virtual async Task OnGetAsync(Guid id)
        {
            var employee = await _employeeAppService.GetAsync(id);
            ViewModel = ObjectMapper.Map<EmployeeDto, UpdateEmployeeViewModel>(employee);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = ObjectMapper.Map<UpdateEmployeeViewModel, CreateUpdateEmployeeDto>(ViewModel);
            if (await _employeeAppService.ValidInfoEmployeeAsync(result))
            {
                await _employeeAppService.UpdateEmployeeAsync(result);
            }
            return NoContent();
        }
    }
}
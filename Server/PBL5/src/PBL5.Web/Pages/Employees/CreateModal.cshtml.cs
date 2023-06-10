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
    public class CreateModalModel : PBL5PageModel
    {
        [BindProperty]
        public CreateEmployeeViewModel ViewModel { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public CreateModalModel(EmployeeAppService employeeAppService){
            _employeeAppService = employeeAppService;
        }

        public void OnGet()
        {
            ViewModel = new CreateEmployeeViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = ObjectMapper.Map<CreateEmployeeViewModel, CreateUpdateEmployeeDto>(ViewModel);
            if (await _employeeAppService.ValidInfoEmployeeAsync(result))
            {
                result.Password = Security.GetMD5(result.Password);
                await _employeeAppService.CreateAsync(result);
            }
            return NoContent();
        }
    }
}
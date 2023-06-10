using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Employees;
using PBL5.Web.Pages;

namespace PBL5.Web.Pages.Employees
{
    public class IndexModel : PBL5PageModel
    {
        [BindProperty]
        public EmployeeSearchViewModel SearchViewModel { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public IndexModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        public void OnGet()
        {

        }

        public class EmployeeSearchViewModel
        {
            public string KeySearch { get; set; }
        }
    }
}
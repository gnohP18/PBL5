using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Employees;
using PBL5.Reports;
using PBL5.Web.Pages;

namespace PBL5.Web.Pages.Reports
{
    public class IndexModel : PBL5PageModel
    {
        [BindProperty]
        public EmployeeSearchViewModel SearchViewModel { get; set; }

        private readonly IReportAppService _reportAppService;

        public IndexModel(IReportAppService reportAppService)
        {
            _reportAppService = reportAppService;
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
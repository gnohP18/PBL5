using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Web.Pages;

namespace PBL5.Web.Pages.TimeSheets
{
    public class IndexModel : PBL5PageModel
    {
        [BindProperty]
        public EmployeeSearchViewModel SearchViewModel { get; set; }

        private readonly ITimeSheetAppService _timeSheetAppService;

        public IndexModel(ITimeSheetAppService timeSheetAppService)
        {
            _timeSheetAppService = timeSheetAppService;
        }

        public void OnGet()
        {

        }

        public class EmployeeSearchViewModel
        {
            public string KeySearch { get; set; }

            [DataType(DataType.Date)]
            public DateTime DayWork { get; set; }
        }
    }
}
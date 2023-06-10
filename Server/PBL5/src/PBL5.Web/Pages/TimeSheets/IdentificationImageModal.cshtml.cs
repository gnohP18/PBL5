using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Employees;
using PBL5.IdentificationImages;
using PBL5.TimeSheets;

namespace PBL5.Web.Pages.TimeSheets
{
    public class IdentificationImageModal : PBL5PageModel
    {
        [BindProperty]
        public IdentificationImageViewModel ViewModel { get; set; }

        [BindProperty]
        public bool IsCheckIn { get; set; }
        private readonly ITimeSheetAppService _timeSheetAppService;
        private readonly ITimeSheetRepository _timeSheetRepository;

        public IdentificationImageModal(
            ITimeSheetAppService timeSheetAppService,
            ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetAppService = timeSheetAppService;
            _timeSheetRepository = timeSheetRepository;
        }

        public void OnGet(Guid id, bool type)
        { 
            IsCheckIn = type;
            ViewModel = new IdentificationImageViewModel();
            ViewModel.Id = id;
        }
    }

    public class IdentificationImageViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string PathCheckIn { get; set; }
        public string PathCheckOut { get; set; }
    }
}
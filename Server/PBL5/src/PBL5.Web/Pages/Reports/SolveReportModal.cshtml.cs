using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL5.Enum;
using PBL5.Reports;

namespace PBL5.Web.Pages.Reports
{
    public class SolveReportModal : PBL5PageModel
    {
        [BindProperty]
        public SolveReportViewModel ViewModel { get; set; }

        public List<SelectListItem> ListStatusReports{ get; set; }

        private readonly IReportAppService _reportAppService;

        public SolveReportModal(IReportAppService reportAppService){
            _reportAppService = reportAppService;
        }

        public virtual async Task OnGetAsync(Guid id)
        {
            var report = await _reportAppService.GetReportByIdAsync(id);
            ViewModel = ObjectMapper.Map<ReportDto, SolveReportViewModel>(report);
            ListStatusReports =  new List<SelectListItem>(){
                new SelectListItem { Value = ReportStatus.PENDING, Text = ReportStatus.PENDING},
                new SelectListItem { Value = ReportStatus.ACCEPT, Text = ReportStatus.ACCEPT},
                new SelectListItem { Value = ReportStatus.DENY, Text = ReportStatus.DENY}
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = ObjectMapper.Map<SolveReportViewModel, UpdateReportDto>(ViewModel);
            await _reportAppService.ChangeStatusReportAsync(ViewModel.Id, ViewModel.ReportStatus);
            return NoContent();
        }
    }
}
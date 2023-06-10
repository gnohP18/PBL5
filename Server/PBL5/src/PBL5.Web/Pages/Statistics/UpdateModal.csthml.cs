using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PBL5.Common;
using PBL5.TimeSheets;

namespace PBL5.Web.Pages.Statistics
{
    public class UpdateModalModel : PBL5PageModel
    {
        [BindProperty]
        public UpdateTimeSheetViewModel ViewModel { get; set; }

        private readonly ITimeSheetAppService _timeSheetAppService;

        public UpdateModalModel(ITimeSheetAppService timeSheetAppService)
        {
            _timeSheetAppService = timeSheetAppService;
        }

        public virtual async Task OnGetAsync(Guid id)
        {
            var timeSheetDto = await _timeSheetAppService.GetDetailTimeSheetAsync(id);
            ViewModel = ObjectMapper.Map<TimeSheetDto, UpdateTimeSheetViewModel>(timeSheetDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = ObjectMapper.Map<UpdateTimeSheetViewModel, CreateUpdateTimeSheetDto>(ViewModel);
            await _timeSheetAppService.UpdateAsync(result.Id, result);
            return NoContent();
        }
    }
}
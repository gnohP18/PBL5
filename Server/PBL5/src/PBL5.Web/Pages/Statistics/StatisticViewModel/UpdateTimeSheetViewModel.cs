using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PBL5.Web.Pages.Statistics
{
    public class UpdateTimeSheetViewModel
    {
        [HiddenInput]
        [DisplayOrder(10001)]
        public Guid Id { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; } 
        public DateTime DateCheckIn { get; set; }
        public string WorkStatus { get; set; }
        public bool IsAbsent { get; set; }
        public string Description { get; set; }
    }
}
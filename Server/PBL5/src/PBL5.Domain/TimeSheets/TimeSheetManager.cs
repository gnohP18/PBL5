using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Enum;
using Volo.Abp.Domain.Services;

namespace PBL5.TimeSheets
{
    public class TimeSheetManager :  DomainService
    {
        public async Task<TimeSheet> CheckOut (TimeSheet timeSheet)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timeSheet.CheckOutTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone).AddHours(Constants.TimeZoneVietNam);
            // timeSheet.CheckOutTime = DateTime.Now;
            await Task.CompletedTask;
            return timeSheet;
        }
    }
}
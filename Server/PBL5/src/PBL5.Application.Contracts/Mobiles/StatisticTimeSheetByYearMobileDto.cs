using System.Collections.Generic;

namespace PBL5.Mobiles
{
    public class StatisticTimeSheetByYearMobileDto
    {
        public int StatisticYear { get; set;}
        public List<StatisticTimeSheetByMonthMobileDto> ListTimeSheetMonths { get; set; }
    }
}
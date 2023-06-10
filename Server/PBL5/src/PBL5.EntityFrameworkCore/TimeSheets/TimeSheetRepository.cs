using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PBL5.TimeSheets;
using PBL5.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PBL5.Enum;
using PBL5.Extensions;

public class TimeSheetRepository : EfCoreRepository<PBL5DbContext, TimeSheet, Guid>, ITimeSheetRepository
{
    public TimeSheetRepository(IDbContextProvider<PBL5DbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<(long, List<TimeSheet>)> GetTimeSheetByDateAsync(string employeeCode, DateTime? date)
    {
        DateTime utcNow = DateTime.UtcNow;
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        var utcN = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone).AddHours(Constants.TimeZoneVietNam);
        if (!date.HasValue) date = utcN;
        var queryable = await GetQueryableAsync();
        if (date.Value > DateTime.Now)
        {
            var nullListTimeSheet = new List<TimeSheet>();
            return (0, nullListTimeSheet);
        }
        var query = queryable
            .WhereIf(date.Value.Date <= DateTime.Now.Date && !employeeCode.IsNullOrWhiteSpace() && date.Value.Month == DateTime.Now.Month,
                p => p.Employee.EmployeeCode.Equals(employeeCode)
                && p.DateCheckIn.Date >= new DateTime(date.Value.Year, date.Value.Month, 1)
                && p.DateCheckIn.Date <= DateTime.Now)
            .WhereIf(date.Value.Date <= DateTime.Now.Date && !employeeCode.IsNullOrWhiteSpace() && date.Value.Month != DateTime.Now.Month,
                p => p.Employee.EmployeeCode.Equals(employeeCode)
                && p.DateCheckIn.Date >= new DateTime(date.Value.Year, date.Value.Month, 1)
                && p.DateCheckIn.Date <= date.Value.AddMonths(1).AddDays(-1).Date);
        var total = await query.CountAsync();
        var result = await query.OrderByDescending(p => p.DateCheckIn)
                                .ToListAsync();
        return (total, result);
    }

    public async Task<(long, List<TimeSheet>)> SearchListAsync(
        string keySearch,
        DateTime dayWork,
        int maxResultCount,
        int skipCount,
        string sorting)
    {
        var queryable = await GetQueryableAsync();
        var query = queryable
            .WhereIf(!keySearch.IsNullOrWhiteSpace(),
                p => p.Employee.IdentityCard.Contains(keySearch))
            .Where(
                p => p.DateCheckIn.Year == dayWork.Year
                && p.DateCheckIn.Month == dayWork.Month
                && p.DateCheckIn.Day == dayWork.Day)
            .IncludeDetails(true);
        var total = await query.CountAsync();
        var result = await query.Skip(skipCount)
                                .Take(maxResultCount)
                                .AsNoTracking()
                                .ToListAsync();
        return (total, result);
    }

    public async Task<(int, int, int)> CountStatusTimeSheetByMonth(
        Guid employeeId,
        int month,
        int year)
    {
        var queryable = await GetQueryableAsync();
        var query = queryable
            .WhereIf(month != 0 && year != 0,
                p => p.EmployeeId == employeeId && p.DateCheckIn.Month == month && p.DateCheckIn.Year == year)
            .IncludeDetails(true);

        var listTimeSheet = await query.ToListAsync();
        var firstDayOfMonth = new DateTime(year, month, 1);
        var totalDays = firstDayOfMonth.AddMonths(1).AddDays(-1).Day; // Tổng số ngày của tháng đó

        int dayWork = listTimeSheet.Count;
        int dayLate = 0;
        int dayOff = 0;
        var dateNow = DateTime.Now;
        //Tính số ngày đi trễ
        dayLate =  listTimeSheet.Count(p => p.WorkStatus == WorkStatus.LATE);

        //Tính và trừ ra số ngày làm viêc vào thứ 7 chủ nhật
        dayWork = dayWork - listTimeSheet.Count(p => p.DateCheckIn.Date.DayOfWeek == DayOfWeek.Saturday || p.DateCheckIn.Date.DayOfWeek == DayOfWeek.Sunday);

        //Trường hợp tháng đang tính là tháng sau tháng hiện tại
        if (dateNow.Year >= year && dateNow.Month < month)
        {
            return (0,0,0);
        }

        //Trường hợp tháng đang xét là tháng trước tháng hiện tại
        if (dateNow.Year <= year && dateNow.Month > month)
        {
            var totalDaysExceptWeekend = totalDays - EnumExtensions.CountWeekendDaysInAMonth(new DateTime(year,month,1));// Tổng số ngày ngoại trừ t7 cn của tháng trước tháng hiện tại
            dayOff = totalDaysExceptWeekend - dayWork;
            return (dayOff, dayWork, dayLate);
        }

        // Trường hợp tháng đang tính là tháng hiện tại
        if (dateNow.Year == year && dateNow.Month == month)
        {
            var totalDaysExceptWeekend = dateNow.Day - EnumExtensions.CountWeekendDaysToPresentDay(new DateTime(year,month,1), dateNow);//Tống số ngày ngoại trừ ngày chủ nhật là tháng hiện tại
            dayOff = totalDaysExceptWeekend - dayWork;
            return (dayOff, dayWork, dayLate);
        }   

        return (0,0,0);     
    }
}
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PBL5.TimeSheets;

public static class TimeSheetQueryableExtensions
{
    public static IQueryable<TimeSheet> IncludeDetails(this IQueryable<TimeSheet> queryable, bool include = true)
    {
        if (!include) return queryable;
        return queryable.Include(x => x.Employee);
    }
}
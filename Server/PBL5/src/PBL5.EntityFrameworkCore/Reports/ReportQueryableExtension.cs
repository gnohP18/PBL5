using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PBL5.Reports
{
    public static class ReportGetQueryableExtensions
    {
        public static IQueryable<Report> IncludeDetails(this IQueryable<Report> queryable, bool include = true)
        {
            if (!include) return queryable;
            return queryable.Include(x => x.Employee);
        }
    }
}
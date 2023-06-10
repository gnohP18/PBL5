using System.Linq;
using Microsoft.EntityFrameworkCore;
using PBL5.Employees;

namespace PBL5.Employees
{
    public static class EmployeeQueryableExtensions
    {
        public static IQueryable<Employee> IncludeDetails(this IQueryable<Employee> queryable, bool include = true)
        {
            if (!include) return queryable;
            return queryable
                .Include(v => v.TimeSheets);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PBL5.Employees;
using PBL5.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PBL5.Employees
{
    public class EmployeeRepository : EfCoreRepository<PBL5DbContext, Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(IDbContextProvider<PBL5DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<(long, List<Employee>)> SearchListAsync(
            string keySearch, 
            int maxResultCount, 
            int skipCount, 
            string sorting)
        {
            var queryable = await GetQueryableAsync();
            var query = queryable
                .WhereIf(!keySearch.IsNullOrWhiteSpace(), p => p.Name.Contains(keySearch) || p.EmployeeCode.Contains(keySearch));
            var total = await query.CountAsync();
            var result = await query.Skip(skipCount)
                                    .Take(maxResultCount)
                                    .AsNoTracking()
                                    .ToListAsync();
            return (total, result);
        }
    }
}
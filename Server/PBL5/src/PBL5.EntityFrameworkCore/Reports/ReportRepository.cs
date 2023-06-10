using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PBL5.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PBL5.Reports
{
    public class ReportRepository : EfCoreRepository<PBL5DbContext, Report, Guid>, IReportRepository
    {
        public ReportRepository(IDbContextProvider<PBL5DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<(long, List<Report>)> SearchListAsync(
            string keySearch, 
            DateTime? dateSearch,
            int maxResultCount, 
            int skipCount, 
            string sorting)
        {
            var queryable = await GetQueryableAsync();
            var query = queryable
                .WhereIf(!keySearch.IsNullOrWhiteSpace(), 
                p => p.Employee.Name.Contains(keySearch) || p.Employee.EmployeeCode.Contains(keySearch))
                .IncludeDetails(true);
            var total = await query.CountAsync();
            var result = await query.OrderByDescending(x => x.CreationTime)
                                    .Skip(skipCount)
                                    .Take(maxResultCount)
                                    .AsNoTracking()
                                    .ToListAsync();
            return (total, result);
        }
    }
}
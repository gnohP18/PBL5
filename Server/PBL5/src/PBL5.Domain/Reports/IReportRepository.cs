using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PBL5.Reports
{
    public interface IReportRepository : IRepository<Report, Guid>
    {
        Task<(long, List<Report>)> SearchListAsync(
            string keySearch, 
            DateTime? dateSearch,
            int maxResultCount, 
            int skipCount, 
            string sorting);
    }
}
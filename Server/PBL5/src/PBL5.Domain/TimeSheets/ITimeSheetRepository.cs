using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PBL5.TimeSheets
{
    public interface ITimeSheetRepository : IRepository<TimeSheet, Guid>
    {
        Task<(long, List<TimeSheet>)> SearchListAsync(
            string keySearch, 
            DateTime dayWork, 
            int maxResultCount, 
            int skipCount, 
            string sorting);

        Task<(long, List<TimeSheet>)> GetTimeSheetByDateAsync(string employeeCode, DateTime? date);

        Task<(int, int, int)> CountStatusTimeSheetByMonth(Guid employeeId, int month, int year);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Employees;
using Volo.Abp.Domain.Repositories;

namespace PBL5.Employees
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    { 
        Task<(long, List<Employee>)> SearchListAsync(
            string keySearch,  
            int maxResultCount, 
            int skipCount, 
            string sorting);
    }
}
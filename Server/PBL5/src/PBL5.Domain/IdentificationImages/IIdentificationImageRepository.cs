using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Employees;
using Volo.Abp.Domain.Repositories;

namespace PBL5.IdentificationImages
{
    public interface IIdentificationImageRepository : IRepository<IdentificationImage, Guid>
    { 
    }
}
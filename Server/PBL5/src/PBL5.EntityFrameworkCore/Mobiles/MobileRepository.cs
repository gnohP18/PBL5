using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PBL5.Mobiles
{
    public class MobileRepository : EfCoreRepository<PBL5DbContext, DeviceNotification, Guid>, IMobileRepository
    {
        public MobileRepository(IDbContextProvider<PBL5DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
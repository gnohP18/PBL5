using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PBL5.Mobiles
{
    public interface IMobileRepository : IRepository<DeviceNotification, Guid>
    {
        
    }
}
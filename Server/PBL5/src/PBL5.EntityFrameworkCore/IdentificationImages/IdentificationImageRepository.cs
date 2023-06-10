using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PBL5.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PBL5.IdentificationImages
{
    public class IdentificationImageRepository : EfCoreRepository<PBL5DbContext, IdentificationImage, Guid>, IIdentificationImageRepository
    {
        public IdentificationImageRepository(IDbContextProvider<PBL5DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
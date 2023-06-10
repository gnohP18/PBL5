using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PBL5.IdentificationImages
{
    public interface IIdentificationImageAppService : ICrudAppService<
        IdentificationImageDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        IdentificationImageDto, 
        IdentificationImageDto>
    {
    }
}
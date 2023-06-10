using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PBL5.IdentificationImages
{
    public class IdentificationImageAppService : 
    CrudAppService<
        IdentificationImage,
        IdentificationImageDto,
        Guid,
        PagedAndSortedResultRequestDto,
        IdentificationImageDto,
        IdentificationImageDto>,
    IIdentificationImageAppService
    {
        private readonly IIdentificationImageRepository _identificationImageRepository;
        public IdentificationImageAppService(IIdentificationImageRepository identityImageRepository) : base(identityImageRepository)
        {
            _identificationImageRepository = identityImageRepository;
        }
    }

}
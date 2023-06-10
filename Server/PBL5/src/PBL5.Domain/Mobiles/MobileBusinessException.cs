using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace PBL5.Mobiles
{
    public class MobileBusinessException : BusinessException
    {
        public MobileBusinessException() : base(PBL5DomainErrorCodes.EmployeeMobileIsNotExist)
        {
        }
    }
    public class WrongEmployeeCodeException : BusinessException
    {
        public WrongEmployeeCodeException() : base(PBL5DomainErrorCodes.EmployeeCodeMobileIsWrong)
        {
        }
    }

    public class WrongEmailOrPasswordException : BusinessException
    {
        public WrongEmailOrPasswordException() : base(PBL5DomainErrorCodes.EmployeeCodeMobileIsWrong)
        {
        }
    }
}
using Volo.Abp;

namespace PBL5.Employees
{
    public class EmployeeBusinessException : BusinessException
    {
        public EmployeeBusinessException()
            : base(PBL5DomainErrorCodes.ExistEmployee)
        {
        }
    }

    public class ExistEmailOfEmployee : BusinessException
    {
        public ExistEmailOfEmployee() 
            : base(PBL5DomainErrorCodes.ExistEmailEmployee)
        {

        }
    }

    public class ExistIdentityCardOfEmployee : BusinessException
    {
        public ExistIdentityCardOfEmployee() 
            : base(PBL5DomainErrorCodes.ExistIdentityCardEmployee)
        {

        }
    }

    public class ExistEmployeeCodeOfEmployee : BusinessException
    {
        public ExistEmployeeCodeOfEmployee() 
            : base(PBL5DomainErrorCodes.ExistEmployeeCode)
        {

        }
    }

    public class WrongNewPasswordOfEmployee : BusinessException
    {
        public WrongNewPasswordOfEmployee() 
            : base(PBL5DomainErrorCodes.WrongNewPasswordOfEmployee)
        {

        }
    }
}
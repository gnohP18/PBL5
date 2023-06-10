namespace PBL5;

public static class PBL5DomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */

    public const string ExistEmailEmployee = "PBL5:0001";
    public const string ExistEmployeeCode = "PBL5:0002";
    public const string ExistEmployee = "PBL5:0003";
    public const string ExistIdentityCardEmployee = "PBL5:0004";
    public const string WrongNewPasswordOfEmployee = "PBL5:0005";

    public const string EmployeeMobileIsNotExist = "PBL5:Mobile:0001";
    public const string EmployeeCodeMobileIsWrong = "PBL5:Mobile:0002";
    public const string WrongEmailOrPasswordException = "PBL5:Mobile:0003";
}

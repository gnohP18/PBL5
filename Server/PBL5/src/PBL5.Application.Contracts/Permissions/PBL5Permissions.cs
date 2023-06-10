namespace PBL5.Permissions;

public static class PBL5Permissions
{
    public const string GroupName = "PBL5";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public class Employee
    {
        public const string Default = GroupName + ".Employee";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public class TimeSheet
    {
        public const string Default = GroupName + ".TimeSheet";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public class Mobile
    {
        public const string Default = GroupName + ".Mobile";
        public const string GetInfo = Default + ".GetInfo";
        public const string ChangePassword = Default + ".ChangePassword";
        public const string ReportTimeSheet = Default + ".ReportTimeSheet";
    }

    public class Report
    {
        public const string Default = GroupName + ".Report";
        public const string ReportTimeSheet = Default + ".ReportTimeSheet";        
    }
}

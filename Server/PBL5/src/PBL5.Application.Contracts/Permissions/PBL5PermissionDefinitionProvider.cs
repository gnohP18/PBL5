using PBL5.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PBL5.Permissions;

public class PBL5PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PBL5Permissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(PBL5Permissions.MyPermission1, L("Permission:MyPermission1"));

        var employeePermission = myGroup.AddPermission(PBL5Permissions.Employee.Default, L("Permission:Employee"));
        employeePermission.AddChild(PBL5Permissions.Employee.Create, L("Permission:Create"));
        employeePermission.AddChild(PBL5Permissions.Employee.Update, L("Permission:Update"));
        employeePermission.AddChild(PBL5Permissions.Employee.Delete, L("Permission:Delete"));

        var timeSheetPermission = myGroup.AddPermission(PBL5Permissions.TimeSheet.Default, L("Permission:TimeSheet"));
        timeSheetPermission.AddChild(PBL5Permissions.TimeSheet.Create, L("Permission:Create"));
        timeSheetPermission.AddChild(PBL5Permissions.TimeSheet.Update, L("Permission:Update"));
        timeSheetPermission.AddChild(PBL5Permissions.TimeSheet.Delete, L("Permission:Delete"));

        var mobilePermission =  myGroup.AddPermission(PBL5Permissions.Mobile.Default, L("Permission:Mobile"));
        mobilePermission.AddChild(PBL5Permissions.Mobile.GetInfo, L("Permission:GetInfo"));
        mobilePermission.AddChild(PBL5Permissions.Mobile.ChangePassword, L("Permission:ChangePassword"));
        mobilePermission.AddChild(PBL5Permissions.Mobile.ReportTimeSheet, L("Permission:ReportTimeSheet"));

        var reportPermission = myGroup.AddPermission(PBL5Permissions.Report.Default, L("Permission:Report"));
        reportPermission.AddChild(PBL5Permissions.Report.ReportTimeSheet, L("Permission:ReportTimeSheet"));
    } 

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PBL5Resource>(name);
    }
}

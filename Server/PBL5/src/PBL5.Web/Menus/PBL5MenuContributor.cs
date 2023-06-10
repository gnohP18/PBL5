using System.Threading.Tasks;
using PBL5.Localization;
using PBL5.MultiTenancy;
using PBL5.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace PBL5.Web.Menus;

public class PBL5MenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<PBL5Resource>();

        // context.Menu.Items.Insert(
        //     0,
        //     new ApplicationMenuItem(
        //         PBL5Menus.Home,
        //         l["Menu:Home"],
        //         "~/",
        //         icon: "fas fa-home",
        //         order: 0
        //     )
        // );

        administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        if (!MultiTenancyConsts.IsEnabled)
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        context.Menu.AddItem(new ApplicationMenuItem(PBL5Menus.Employee, l["Employee"], url: "/Employees", icon: "fa fa-id-card", order: 0).RequirePermissions(PBL5Permissions.Employee.Default));
        context.Menu.AddItem(new ApplicationMenuItem(PBL5Menus.TImeSheet, l["TimeSheet"], url: "/TimeSheets", icon: "fa fa-calendar-o", order: 1).RequirePermissions(PBL5Permissions.TimeSheet.Default));
        context.Menu.AddItem(new ApplicationMenuItem(PBL5Menus.Statistic, l["Statistic"], url: "/Statistics", icon: "fa fa-line-chart", order: 2).RequirePermissions(PBL5Permissions.TimeSheet.Default));
        context.Menu.AddItem(new ApplicationMenuItem(PBL5Menus.Report, l["Report"], url: "/Reports", icon: "fa fa-clipboard", order: 3).RequirePermissions(PBL5Permissions.Report.Default));

        await Task.CompletedTask;
    }
}

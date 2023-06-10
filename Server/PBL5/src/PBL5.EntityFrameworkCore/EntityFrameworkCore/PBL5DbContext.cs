using Microsoft.EntityFrameworkCore;
using PBL5.IdentificationImages;
using PBL5.Employees;
using PBL5.TimeSheets;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using PBL5.Reports;
using PBL5.Mobiles;

namespace PBL5.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class PBL5DbContext :
    AbpDbContext<PBL5DbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    //Entity
    public DbSet<Employee> Employees { get; set; }
    public DbSet<IdentificationImage> IdentificationImages { get; set; }
    public DbSet<TimeSheet> TimeSheets { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<DeviceNotification> DeviceNotifications { get; set; }

    #endregion

    public PBL5DbContext(DbContextOptions<PBL5DbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(PBL5Consts.DbTablePrefix + "YourEntities", PBL5Consts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Employee>(p =>
        {
            p.ToTable(PBL5Consts.DbTablePrefix + "Employees", PBL5Consts.DbSchema);
            p.ConfigureByConvention();
        });

        builder.Entity<IdentificationImage>(p =>
        {
            p.ToTable(PBL5Consts.DbTablePrefix + "IdentificationImages", PBL5Consts.DbSchema);
            p.ConfigureByConvention();
        });

        builder.Entity<TimeSheet>(p =>
        {
            p.ToTable(PBL5Consts.DbTablePrefix + "TimeSheets", PBL5Consts.DbSchema);
            p.HasOne(v => v.Employee).WithMany(n => n.TimeSheets).HasForeignKey(d => d.EmployeeId).IsRequired();
            p.ConfigureByConvention();
        });

        builder.Entity<Report>(b =>
        {
            b.ToTable(PBL5Consts.DbTablePrefix + "Reports", PBL5Consts.DbSchema);
            b.HasOne(v => v.Employee).WithMany(n => n.Reports).HasForeignKey(d => d.EmployeeId).IsRequired();
            b.ConfigureByConvention();
        });

        builder.Entity<DeviceNotification>(p =>
        {
            p.ToTable(PBL5Consts.DbTablePrefix + "DeviceNotifications", PBL5Consts.DbSchema);
            p.ConfigureByConvention();
        });
    }
}

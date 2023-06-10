using PBL5.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PBL5.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PBL5EntityFrameworkCoreModule),
    typeof(PBL5ApplicationContractsModule)
    )]
public class PBL5DbMigratorModule : AbpModule
{

}

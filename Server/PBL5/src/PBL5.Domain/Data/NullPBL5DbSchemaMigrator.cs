using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PBL5.Data;

/* This is used if database provider does't define
 * IPBL5DbSchemaMigrator implementation.
 */
public class NullPBL5DbSchemaMigrator : IPBL5DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

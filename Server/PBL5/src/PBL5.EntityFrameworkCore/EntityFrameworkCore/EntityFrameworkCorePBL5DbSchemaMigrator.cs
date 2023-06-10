using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PBL5.Data;
using Volo.Abp.DependencyInjection;

namespace PBL5.EntityFrameworkCore;

public class EntityFrameworkCorePBL5DbSchemaMigrator
    : IPBL5DbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorePBL5DbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the PBL5DbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<PBL5DbContext>()
            .Database
            .MigrateAsync();
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZenCrm.Data;
using Volo.Abp.DependencyInjection;

namespace ZenCrm.EntityFrameworkCore;

public class EntityFrameworkCoreZenCrmDbSchemaMigrator
    : IZenCrmDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreZenCrmDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ZenCrmDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ZenCrmDbContext>()
            .Database
            .MigrateAsync();
    }
}

using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ZenCrm.Data;

/* This is used if database provider does't define
 * IZenCrmDbSchemaMigrator implementation.
 */
public class NullZenCrmDbSchemaMigrator : IZenCrmDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

using ZenCrm.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ZenCrm.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ZenCrmEntityFrameworkCoreModule),
    typeof(ZenCrmApplicationContractsModule)
)]
public class ZenCrmDbMigratorModule : AbpModule
{
}

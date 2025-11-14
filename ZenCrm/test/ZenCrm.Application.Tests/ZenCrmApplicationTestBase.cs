using Volo.Abp.Modularity;

namespace ZenCrm;

public abstract class ZenCrmApplicationTestBase<TStartupModule> : ZenCrmTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

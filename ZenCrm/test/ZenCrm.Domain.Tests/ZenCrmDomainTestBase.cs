using Volo.Abp.Modularity;

namespace ZenCrm;

/* Inherit from this class for your domain layer tests. */
public abstract class ZenCrmDomainTestBase<TStartupModule> : ZenCrmTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

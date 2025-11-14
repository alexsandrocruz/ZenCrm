using Volo.Abp.Modularity;

namespace ZenCrm;

[DependsOn(
    typeof(ZenCrmDomainModule),
    typeof(ZenCrmTestBaseModule)
)]
public class ZenCrmDomainTestModule : AbpModule
{

}

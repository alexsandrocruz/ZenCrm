using Volo.Abp.Modularity;

namespace ZenCrm;

[DependsOn(
    typeof(ZenCrmApplicationModule),
    typeof(ZenCrmDomainTestModule)
)]
public class ZenCrmApplicationTestModule : AbpModule
{

}

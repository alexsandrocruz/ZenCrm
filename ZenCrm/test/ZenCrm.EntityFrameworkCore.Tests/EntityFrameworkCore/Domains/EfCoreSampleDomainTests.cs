using ZenCrm.Samples;
using Xunit;

namespace ZenCrm.EntityFrameworkCore.Domains;

[Collection(ZenCrmTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ZenCrmEntityFrameworkCoreTestModule>
{

}

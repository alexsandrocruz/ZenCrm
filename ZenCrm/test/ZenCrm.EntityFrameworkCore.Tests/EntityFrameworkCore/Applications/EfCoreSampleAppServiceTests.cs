using ZenCrm.Samples;
using Xunit;

namespace ZenCrm.EntityFrameworkCore.Applications;

[Collection(ZenCrmTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ZenCrmEntityFrameworkCoreTestModule>
{

}

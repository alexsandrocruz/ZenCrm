using Xunit;

namespace ZenCrm.EntityFrameworkCore;

[CollectionDefinition(ZenCrmTestConsts.CollectionDefinitionName)]
public class ZenCrmEntityFrameworkCoreCollection : ICollectionFixture<ZenCrmEntityFrameworkCoreFixture>
{

}

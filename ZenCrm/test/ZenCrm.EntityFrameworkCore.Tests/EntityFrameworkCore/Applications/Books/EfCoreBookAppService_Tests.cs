using ZenCrm.Books;
using Xunit;

namespace ZenCrm.EntityFrameworkCore.Applications.Books;

[Collection(ZenCrmTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<ZenCrmEntityFrameworkCoreTestModule>
{

}
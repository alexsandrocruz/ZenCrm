using ZenCrm.Finance.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ZenCrm.Finance.csproj"); 
await builder.RunAbpModuleAsync<FinanceTestsModule>(applicationName: "ZenCrm.Finance");

public partial class TestProgram
{
}

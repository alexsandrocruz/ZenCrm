using ZenCrm.Juris.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ZenCrm.Juris.csproj"); 
await builder.RunAbpModuleAsync<JurisTestsModule>(applicationName: "ZenCrm.Juris");

public partial class TestProgram
{
}

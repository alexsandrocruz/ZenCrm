using ZenCrm.EventFlow.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ZenCrm.EventFlow.csproj"); 
await builder.RunAbpModuleAsync<EventFlowTestsModule>(applicationName: "ZenCrm.EventFlow");

public partial class TestProgram
{
}

using ZenCrm.Catalog.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ZenCrm.Catalog.csproj"); 
await builder.RunAbpModuleAsync<CatalogTestsModule>(applicationName: "ZenCrm.Catalog");

public partial class TestProgram
{
}

using Microsoft.AspNetCore.Builder;
using ZenCrm;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ZenCrm.Web.csproj"); 
await builder.RunAbpModuleAsync<ZenCrmWebTestModule>(applicationName: "ZenCrm.Web");

public partial class Program
{
}

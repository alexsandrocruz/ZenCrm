using System.Threading.Tasks;
using Shouldly;
using Xunit;
using ZenCrm.Juris.Samples;

namespace ZenCrm.Juris.Tests.Controllers;

/* This is a demo test class to show how to test HTTP API controllers.
 * You can delete this class freely.
 *
 * See https://docs.abp.io/en/abp/latest/Testing for more about automated tests.
 */

public class ExampleController_Tests : JurisIntegrationTestBase
{
    [Fact]
    public async Task GetAsync()
    {
        var response = await GetResponseAsObjectAsync<SampleDto>("api/Juris/example");
        response.Value.ShouldBe(42);
    }
}
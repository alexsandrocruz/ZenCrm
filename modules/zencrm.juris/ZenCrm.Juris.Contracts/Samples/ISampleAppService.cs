using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ZenCrm.Juris.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}

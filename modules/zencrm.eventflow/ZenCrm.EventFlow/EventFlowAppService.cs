using ZenCrm.EventFlow.Localization;
using Volo.Abp.Application.Services;

namespace ZenCrm.EventFlow;

public abstract class EventFlowAppService : ApplicationService
{
    protected EventFlowAppService()
    {
        LocalizationResource = typeof(EventFlowResource);
        ObjectMapperContext = typeof(EventFlowModule);
    }
}

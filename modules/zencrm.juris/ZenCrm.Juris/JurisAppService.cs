using ZenCrm.Juris.Localization;
using Volo.Abp.Application.Services;

namespace ZenCrm.Juris;

public abstract class JurisAppService : ApplicationService
{
    protected JurisAppService()
    {
        LocalizationResource = typeof(JurisResource);
        ObjectMapperContext = typeof(JurisModule);
    }
}

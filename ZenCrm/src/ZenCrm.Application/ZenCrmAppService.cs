using ZenCrm.Localization;
using Volo.Abp.Application.Services;

namespace ZenCrm;

/* Inherit your application services from this class.
 */
public abstract class ZenCrmAppService : ApplicationService
{
    protected ZenCrmAppService()
    {
        LocalizationResource = typeof(ZenCrmResource);
    }
}

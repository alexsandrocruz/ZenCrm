using ZenCrm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ZenCrm.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ZenCrmController : AbpControllerBase
{
    protected ZenCrmController()
    {
        LocalizationResource = typeof(ZenCrmResource);
    }
}

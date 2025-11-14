using ZenCrm.EventFlow.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ZenCrm.EventFlow.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class EventFlowPageModel : AbpPageModel
{
    protected EventFlowPageModel()
    {
        LocalizationResourceType = typeof(EventFlowResource);
        ObjectMapperContext = typeof(EventFlowWebModule);
    }
}

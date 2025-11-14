using ZenCrm.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ZenCrm.Web.Pages;

public abstract class ZenCrmPageModel : AbpPageModel
{
    protected ZenCrmPageModel()
    {
        LocalizationResourceType = typeof(ZenCrmResource);
    }
}

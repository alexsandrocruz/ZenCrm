using ZenCrm.Finance.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ZenCrm.Finance.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class FinancePageModel : AbpPageModel
{
    protected FinancePageModel()
    {
        LocalizationResourceType = typeof(FinanceResource);
        ObjectMapperContext = typeof(FinanceWebModule);
    }
}

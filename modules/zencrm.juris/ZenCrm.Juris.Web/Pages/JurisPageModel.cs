using ZenCrm.Juris.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ZenCrm.Juris.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class JurisPageModel : AbpPageModel
{
    protected JurisPageModel()
    {
        LocalizationResourceType = typeof(JurisResource);
        ObjectMapperContext = typeof(JurisWebModule);
    }
}

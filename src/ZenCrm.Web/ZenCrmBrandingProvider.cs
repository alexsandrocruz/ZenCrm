using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using ZenCrm.Localization;

namespace ZenCrm.Web;

[Dependency(ReplaceServices = true)]
public class ZenCrmBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ZenCrmResource> _localizer;

    public ZenCrmBrandingProvider(IStringLocalizer<ZenCrmResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}

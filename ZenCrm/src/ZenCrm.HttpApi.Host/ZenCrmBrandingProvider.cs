using Microsoft.Extensions.Localization;
using ZenCrm.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ZenCrm;

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

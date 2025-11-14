using Volo.Abp.Settings;

namespace ZenCrm.Settings;

public class ZenCrmSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ZenCrmSettings.MySetting1));
    }
}

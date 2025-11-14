using ZenCrm.EventFlow.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ZenCrm.EventFlow.Permissions;

public class EventFlowPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EventFlowPermissions.GroupName, L("Permission:EventFlow"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EventFlowResource>(name);
    }
}

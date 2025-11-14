using ZenCrm.Juris.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ZenCrm.Juris.Permissions;

public class JurisPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(JurisPermissions.GroupName, L("Permission:Juris"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<JurisResource>(name);
    }
}

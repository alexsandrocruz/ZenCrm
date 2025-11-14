using ZenCrm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ZenCrm.Permissions;

public class ZenCrmPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ZenCrmPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(ZenCrmPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ZenCrmPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ZenCrmResource>(name);
    }
}

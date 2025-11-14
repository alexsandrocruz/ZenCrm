using Volo.Abp.Reflection;

namespace ZenCrm.Finance.Permissions;

public class FinancePermissions
{
    public const string GroupName = "Finance";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FinancePermissions));
    }
}

using Volo.Abp.Reflection;

namespace ZenCrm.Juris.Permissions;

public class JurisPermissions
{
    public const string GroupName = "Juris";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(JurisPermissions));
    }
}

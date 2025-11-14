using Volo.Abp.Reflection;

namespace ZenCrm.EventFlow.Permissions;

public class EventFlowPermissions
{
    public const string GroupName = "EventFlow";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(EventFlowPermissions));
    }
}

namespace ZenCrm.EventFlow;

public static class EventFlowDbProperties
{
    public static string DbTablePrefix { get; set; } = "EventFlow";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "EventFlow";
}

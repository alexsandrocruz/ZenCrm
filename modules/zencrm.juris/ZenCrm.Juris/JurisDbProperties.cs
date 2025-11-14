namespace ZenCrm.Juris;

public static class JurisDbProperties
{
    public static string DbTablePrefix { get; set; } = "Juris";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Juris";
}

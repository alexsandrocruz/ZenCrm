namespace ZenCrm.Finance;

public static class FinanceDbProperties
{
    public static string DbTablePrefix { get; set; } = "Finance";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Finance";
}

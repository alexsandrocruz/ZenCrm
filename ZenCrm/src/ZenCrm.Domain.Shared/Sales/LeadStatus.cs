namespace ZenCrm.Sales;

public enum LeadStatus
{
    /// <summary>
    /// New lead captured
    /// </summary>
    New = 1,

    /// <summary>
    /// Lead is being qualified
    /// </summary>
    Qualifying = 2,

    /// <summary>
    /// Lead has been qualified as potential customer
    /// </summary>
    Qualified = 3,

    /// <summary>
    /// Lead converted to opportunity
    /// </summary>
    Converted = 4,

    /// <summary>
    /// Lead not qualified or lost
    /// </summary>
    Lost = 5,

    /// <summary>
    /// Lead recycled for future follow-up
    /// </summary>
    Recycled = 6
}
namespace ZenCrm.Communication;

/// <summary>
/// Priority level for message processing
/// </summary>
public enum MessagePriority
{
    /// <summary>
    /// Low priority - processed during off-peak hours
    /// </summary>
    Low = 1,

    /// <summary>
    /// Normal priority - standard processing
    /// </summary>
    Normal = 2,

    /// <summary>
    /// High priority - expedited processing
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical priority - immediate processing
    /// </summary>
    Critical = 4
}
namespace ZenCrm.Sales;

public enum InteractionStatus
{
    /// <summary>
    /// Interaction scheduled but not yet completed
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// Interaction currently in progress
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Interaction completed successfully
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Interaction cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Interaction postponed
    /// </summary>
    Postponed = 5,

    /// <summary>
    /// Interaction failed
    /// </summary>
    Failed = 6,

    /// <summary>
    /// Interaction is pending
    /// </summary>
    Pending = 7
}
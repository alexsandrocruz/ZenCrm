namespace ZenCrm.Communication;

/// <summary>
/// Type of message for categorization and routing
/// </summary>
public enum MessageType
{
    /// <summary>
    /// General notification message
    /// </summary>
    Notification = 1,

    /// <summary>
    /// Marketing or promotional message
    /// </summary>
    Marketing = 2,

    /// <summary>
    /// Transactional message (confirmations, receipts)
    /// </summary>
    Transactional = 3,

    /// <summary>
    /// Alert or warning message
    /// </summary>
    Alert = 4,

    /// <summary>
    /// Reminder message
    /// </summary>
    Reminder = 5
}
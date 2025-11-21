namespace ZenCrm.Communication;

/// <summary>
/// Status of a message in the communication system
/// </summary>
public enum MessageStatus
{
    /// <summary>
    /// Message is being drafted
    /// </summary>
    Draft = 1,

    /// <summary>
    /// Message is queued for sending
    /// </summary>
    Queued = 2,

    /// <summary>
    /// Message is currently being processed
    /// </summary>
    Processing = 3,

    /// <summary>
    /// Message has been sent to provider
    /// </summary>
    Sent = 4,

    /// <summary>
    /// Message has been delivered to recipient
    /// </summary>
    Delivered = 5,

    /// <summary>
    /// Message has been read by recipient
    /// </summary>
    Read = 6,

    /// <summary>
    /// Message sending failed
    /// </summary>
    Failed = 7,

    /// <summary>
    /// Message was cancelled before sending
    /// </summary>
    Cancelled = 8
}
namespace ZenCrm.Communication;

/// <summary>
/// Communication channels available in the system
/// </summary>
public enum CommunicationChannel
{
    /// <summary>
    /// Email communication
    /// </summary>
    Email = 1,

    /// <summary>
    /// SMS text message
    /// </summary>
    SMS = 2,

    /// <summary>
    /// WhatsApp message
    /// </summary>
    WhatsApp = 3,

    /// <summary>
    /// Push notification
    /// </summary>
    PushNotification = 4
}
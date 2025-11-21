namespace ZenCrm.Communication.Sms;

/// <summary>
/// SMS provider enumeration
/// </summary>
public enum SmsProvider
{
    Twilio = 1,
    Vonage = 2,
    Azure = 3,
    AmazonSNS = 4,
    ClickSend = 5,
    Plivo = 6,
    Sinch = 7,
    MessageBird = 8
}

/// <summary>
/// SMS category enumeration
/// </summary>
public enum SmsCategory
{
    Transactional = 0,
    Marketing = 1,
    Notification = 2,
    Authentication = 3,
    Support = 4
}

/// <summary>
/// Message priority enumeration
/// </summary>
public enum MessagePriority
{
    Low = 0,
    Normal = 1,
    High = 2
}

/// <summary>
/// Consent type enumeration
/// </summary>
public enum ConsentType
{
    None = 0,
    Explicit = 1,
    Implicit = 2,
    Required = 3
}

/// <summary>
/// Report type enumeration
/// </summary>
public enum ReportType
{
    Summary = 0,
    Detailed = 1,
    Timeline = 2,
    Statistics = 3
}
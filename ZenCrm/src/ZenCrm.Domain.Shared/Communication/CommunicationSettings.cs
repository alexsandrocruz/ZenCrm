namespace ZenCrm.Communication;

/// <summary>
/// Communication settings definition
/// </summary>
public static class CommunicationSettings
{
    public const string GroupName = "ZenCrm.Communication";

    // Email Settings
    public const string EmailEnabled = $"{GroupName}.Email.Enabled";
    public const string EmailDefaultFrom = $"{GroupName}.Email.DefaultFrom";
    public const string EmailDefaultFromName = $"{GroupName}.Email.DefaultFromName";

    // SMS Settings
    public const string SmsEnabled = $"{GroupName}.Sms.Enabled";
    public const string SmsProvider = $"{GroupName}.Sms.Provider";
    public const string SmsDefaultFrom = $"{GroupName}.Sms.DefaultFrom";

    // WhatsApp Settings
    public const string WhatsAppEnabled = $"{GroupName}.WhatsApp.Enabled";
    public const string WhatsAppProvider = $"{GroupName}.WhatsApp.Provider";
    public const string WhatsAppDefaultFrom = $"{GroupName}.WhatsApp.DefaultFrom";

    // General Settings
    public const string MaxRetries = $"{GroupName}.MaxRetries";
    public const string RetryDelay = $"{GroupName}.RetryDelay";
    public const string MaxProcessingTime = $"{GroupName}.MaxProcessingTime";
    public const string EnableBulkSending = $"{GroupName}.EnableBulkSending";
    public const string BulkBatchSize = $"{GroupName}.BulkBatchSize";

    // Template Settings
    public const string EnableTemplates = $"{GroupName}.Templates.Enabled";
    public const string TemplateCacheDuration = $"{GroupName}.Templates.CacheDuration";

    // Security Settings
    public const string EnableRateLimit = $"{GroupName}.Security.EnableRateLimit";
    public const string RateLimitPerMinute = $"{GroupName}.Security.RateLimitPerMinute";
    public const string EnableBlacklist = $"{GroupName}.Security.EnableBlacklist";
}
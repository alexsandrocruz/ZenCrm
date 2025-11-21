using System.Threading.Tasks;
using Volo.Abp.Emailing;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// Email service interface for sending emails through ABP Emailing
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send an email message
    /// </summary>
    Task<MessageDeliveryResult> SendAsync(Message message);

    /// <summary>
    /// Validate email address format
    /// </summary>
    Task<bool> ValidateEmailAddressAsync(string email);

    /// <summary>
    /// Check if email service is configured and available
    /// </summary>
    Task<bool> IsAvailableAsync();
}

/// <summary>
/// Result of message delivery attempt
/// </summary>
public class MessageDeliveryResult
{
    public bool Success { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ProviderResponse { get; set; }
    public string? ErrorMessage { get; set; }
    public bool ShouldRetry { get; set; }
    public System.DateTime? NextRetryDate { get; set; }
}
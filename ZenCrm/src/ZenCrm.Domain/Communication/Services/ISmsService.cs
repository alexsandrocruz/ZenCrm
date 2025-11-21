using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// SMS service interface for sending SMS messages
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// Send an SMS message
    /// </summary>
    Task<MessageDeliveryResult> SendAsync(Message message);

    /// <summary>
    /// Send an SMS message to multiple recipients
    /// </summary>
    Task<BatchMessageResult> SendBatchAsync(Message message, string[] recipients);

    /// <summary>
    /// Validate phone number format
    /// </summary>
    Task<bool> ValidatePhoneNumberAsync(string phoneNumber);

    /// <summary>
    /// Validate multiple phone numbers
    /// </summary>
    Task<PhoneValidationResult> ValidatePhoneNumbersAsync(string[] phoneNumbers);

    /// <summary>
    /// Format phone number to international format
    /// </summary>
    Task<string> FormatPhoneNumberAsync(string phoneNumber, string? countryCode = null);

    /// <summary>
    /// Get SMS delivery status
    /// </summary>
    Task<SmsDeliveryStatus> GetDeliveryStatusAsync(string messageId);

    /// <summary>
    /// Get SMS message details
    /// </summary>
    Task<SmsMessageDetails> GetMessageDetailsAsync(string messageId);

    /// <summary>
    /// Check if SMS service is configured and available
    /// </summary>
    Task<bool> IsAvailableAsync();

    /// <summary>
    /// Calculate SMS cost for a message
    /// </summary>
    Task<SmsCostResult> CalculateCostAsync(string phoneNumber, int messageLength, string? countryCode = null);
}

/// <summary>
/// Result of batch SMS sending
/// </summary>
public class BatchMessageResult
{
    public int TotalRecipients { get; set; }
    public int SuccessfulSends { get; set; }
    public int FailedSends { get; set; }
    public List<SmsSendResult> Results { get; set; } = new();
}

/// <summary>
/// Result of individual SMS send
/// </summary>
public class SmsSendResult
{
    public string PhoneNumber { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? MessageId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; set; }
}

/// <summary>
/// Phone number validation result
/// </summary>
public class PhoneValidationResult
{
    public List<ValidPhoneNumber> ValidNumbers { get; set; } = new();
    public List<InvalidPhoneNumber> InvalidNumbers { get; set; } = new();
    public int TotalNumbers => ValidNumbers.Count + InvalidNumbers.Count;
    public bool AllValid => InvalidNumbers.Count == 0;
}

/// <summary>
/// Valid phone number
/// </summary>
public class ValidPhoneNumber
{
    public string Original { get; set; } = string.Empty;
    public string Formatted { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // mobile, landline, etc.
}

/// <summary>
/// Invalid phone number
/// </summary>
public class InvalidPhoneNumber
{
    public string Original { get; set; } = string.Empty;
    public string ErrorReason { get; set; } = string.Empty;
}

/// <summary>
/// SMS message details
/// </summary>
public class SmsMessageDetails
{
    public string MessageId { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public SmsStatus Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public int Segments { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// SMS status enumeration (legacy, use SmsDeliveryStatus from entities)
/// </summary>
public enum SmsStatus
{
    Queued = 0,
    Sending = 1,
    Sent = 2,
    Delivered = 3,
    Read = 4,
    Failed = 5,
    Undelivered = 6,
    Rejected = 7,
    Canceled = 8
}

/// <summary>
/// SMS cost calculation result
/// </summary>
public class SmsCostResult
{
    public double Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int Segments { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public bool IsInternational { get; set; }
    public string PricePerSegment { get; set; } = string.Empty;
}
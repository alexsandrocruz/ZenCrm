using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ZenCrm.Communication.Entities;

/// <summary>
/// SMS delivery tracking record for monitoring message status
/// </summary>
/// <summary>
/// SMS delivery status enumeration
/// </summary>
public enum SmsDeliveryStatus
{
    /// <summary>
    /// Message queued for sending
    /// </summary>
    Queued = 0,

    /// <summary>
    /// Message is being sent
    /// </summary>
    Sending = 1,

    /// <summary>
    /// Message sent to network
    /// </summary>
    Sent = 2,

    /// <summary>
    /// Message delivered to recipient
    /// </summary>
    Delivered = 3,

    /// <summary>
    /// Message read by recipient
    /// </summary>
    Read = 4,

    /// <summary>
    /// Delivery failed
    /// </summary>
    Failed = 5,

    /// <summary>
    /// Could not deliver message
    /// </summary>
    Undelivered = 6,

    /// <summary>
    /// Message rejected by provider
    /// </summary>
    Rejected = 7,

    /// <summary>
    /// Message was canceled
    /// </summary>
    Canceled = 8
}

/// <summary>
/// SMS delivery tracking record for monitoring message status
/// </summary>
public class SmsDeliveryRecord : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// Associated message ID
    /// </summary>
    public Guid MessageId { get; private set; }

    /// <summary>
    /// External provider message ID (e.g., Twilio SID)
    /// </summary>
    public string ExternalMessageId { get; private set; }

    /// <summary>
    /// Recipient phone number
    /// </summary>
    public string ToPhoneNumber { get; private set; }

    /// <summary>
    /// Sender phone number
    /// </summary>
    public string FromPhoneNumber { get; private set; }

    /// <summary>
    /// Current delivery status
    /// </summary>
    public SmsDeliveryStatus Status { get; private set; }

    /// <summary>
    /// Previous delivery status
    /// </summary>
    public SmsDeliveryStatus? PreviousStatus { get; private set; }

    /// <summary>
    /// SMS provider used
    /// </summary>
    public SmsProvider Provider { get; private set; }

    /// <summary>
    /// SMS category (transactional, marketing, etc.)
    /// </summary>
    public SmsCategory Category { get; private set; }

    /// <summary>
    /// Message content (truncated for storage)
    /// </summary>
    public string MessageContent { get; private set; }

    /// <summary>
    /// Number of SMS segments
    /// </summary>
    public int Segments { get; private set; }

    /// <summary>
    /// Message cost
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// Cost currency
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// Date and time when message was sent
    /// </summary>
    public DateTime? SentAt { get; private set; }

    /// <summary>
    /// Date and time when message was delivered
    /// </summary>
    public DateTime? DeliveredAt { get; private set; }

    /// <summary>
    /// Date and time when message was read
    /// </summary>
    public DateTime? ReadAt { get; private set; }

    /// <summary>
    /// Date and time when delivery failed
    /// </summary>
    public DateTime? FailedAt { get; private set; }

    /// <summary>
    /// Error code from provider
    /// </summary>
    public string ErrorCode { get; private set; }

    /// <summary>
    /// Error message from provider
    /// </summary>
    public string ErrorMessage { get; private set; }

    /// <summary>
    /// Number of retry attempts
    /// </summary>
    public int RetryCount { get; private set; }

    /// <summary>
    /// Maximum retry attempts allowed
    /// </summary>
    public int MaxRetryAttempts { get; private set; }

    /// <summary>
    /// Date for next retry attempt
    /// </summary>
    public DateTime? NextRetryAt { get; private set; }

    /// <summary>
    /// Whether message delivery tracking is active
    /// </summary>
    public bool TrackingActive { get; private set; }

    /// <summary>
    /// Last status check timestamp
    /// </summary>
    public DateTime? LastStatusCheckAt { get; private set; }

    /// <summary>
    /// IP address or device identifier used for delivery
    /// </summary>
    public string DeliveryInfo { get; private set; }

    /// <summary>
    /// Country code of recipient
    /// </summary>
    public string RecipientCountry { get; private set; }

    /// <summary>
    /// Time zone of recipient (if available)
    /// </summary>
    public string RecipientTimeZone { get; private set; }

    /// <summary>
    /// Campaign or batch identifier
    /// </summary>
    public string CampaignId { get; private set; }

    /// <summary>
    /// Tags for categorization
    /// </summary>
    public string Tags { get; private set; }

    protected SmsDeliveryRecord()
    {
    }

    public SmsDeliveryRecord(
        Guid id,
        Guid messageId,
        string externalMessageId,
        string toPhoneNumber,
        string fromPhoneNumber,
        SmsProvider provider,
        SmsCategory category) : base(id)
    {
        MessageId = messageId;
        ExternalMessageId = externalMessageId;
        ToPhoneNumber = toPhoneNumber;
        FromPhoneNumber = fromPhoneNumber;
        Status = SmsDeliveryStatus.Queued;
        Provider = provider;
        Category = category;
        Currency = "USD";
        MaxRetryAttempts = 3;
        TrackingActive = true;
        RetryCount = 0;
        Segments = 1;
    }

    /// <summary>
    /// Update delivery status
    /// </summary>
    public void UpdateStatus(SmsDeliveryStatus newStatus, string? errorCode = null, string? errorMessage = null)
    {
        if (Status == newStatus)
            return; // No status change

        PreviousStatus = Status;
        Status = newStatus;

        // Update timestamps based on status
        var now = DateTime.UtcNow;

        switch (newStatus)
        {
            case SmsDeliveryStatus.Sent:
                SentAt = SentAt ?? now;
                break;
            case SmsDeliveryStatus.Delivered:
                DeliveredAt = now;
                break;
            case SmsDeliveryStatus.Read:
                ReadAt = ReadAt ?? now;
                break;
            case SmsDeliveryStatus.Failed:
            case SmsDeliveryStatus.Undelivered:
                FailedAt = now;
                break;
        }

        // Update error information
        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            ErrorCode = errorCode;
        }

        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            ErrorMessage = errorMessage;
        }

        // Handle retry logic
        if (newStatus == SmsDeliveryStatus.Failed && ShouldRetry())
        {
            ScheduleRetry();
        }
        else
        {
            TrackingActive = false;
        }
    }

    /// <summary>
    /// Set message content and details
    /// </summary>
    public void SetMessageDetails(string content, int segments, decimal cost, string currency = "USD")
    {
        MessageContent = content?.Length > 500 ? content.Substring(0, 497) + "..." : content;
        Segments = segments;
        Cost = cost;
        Currency = currency;
    }

    /// <summary>
    /// Set delivery information
    /// </summary>
    public void SetDeliveryInfo(string recipientCountry, string recipientTimeZone = null, string deliveryInfo = null)
    {
        RecipientCountry = recipientCountry;
        RecipientTimeZone = recipientTimeZone;
        DeliveryInfo = deliveryInfo;
    }

    /// <summary>
    /// Set campaign information
    /// </summary>
    public void SetCampaignInfo(string campaignId, string tags = null)
    {
        CampaignId = campaignId;
        Tags = tags;
    }

    /// <summary>
    /// Increment retry count
    /// </summary>
    public void IncrementRetry()
    {
        RetryCount++;
        if (RetryCount >= MaxRetryAttempts)
        {
            TrackingActive = false;
        }
    }

    /// <summary>
    /// Schedule next retry attempt
    /// </summary>
    public void ScheduleRetry()
    {
        if (RetryCount >= MaxRetryAttempts)
        {
            TrackingActive = false;
            return;
        }

        // Exponential backoff: 5 minutes, 15 minutes, 45 minutes
        var retryMinutes = 5 * (int)Math.Pow(3, RetryCount);
        NextRetryAt = DateTime.UtcNow.AddMinutes(retryMinutes);
    }

    /// <summary>
    /// Mark status check
    /// </summary>
    public void MarkStatusCheck()
    {
        LastStatusCheckAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Determine if message should be retried
    /// </summary>
    private bool ShouldRetry()
    {
        if (RetryCount >= MaxRetryAttempts)
            return false;

        // Don't retry certain permanent failure types
        var permanentFailures = new[]
        {
            SmsDeliveryStatus.Rejected,
            SmsDeliveryStatus.Canceled
        };

        return !permanentFailures.Contains(Status);
    }

    /// <summary>
    /// Get delivery duration
    /// </summary>
    public TimeSpan? GetDeliveryDuration()
    {
        if (!SentAt.HasValue || !DeliveredAt.HasValue)
            return null;

        return DeliveredAt.Value - SentAt.Value;
    }

    /// <summary>
    /// Check if delivery was successful
    /// </summary>
    public bool IsDeliveredSuccessfully()
    {
        return Status == SmsDeliveryStatus.Delivered || Status == SmsDeliveryStatus.Read;
    }

    /// <summary>
    /// Check if delivery failed permanently
    /// </summary>
    public bool IsFailedPermanently()
    {
        var permanentFailures = new[]
        {
            SmsDeliveryStatus.Failed,
            SmsDeliveryStatus.Undelivered,
            SmsDeliveryStatus.Rejected,
            SmsDeliveryStatus.Canceled
        };

        return permanentFailures.Contains(Status) && !TrackingActive;
    }

    /// <summary>
    /// Get human readable status description
    /// </summary>
    public string GetStatusDescription()
    {
        return Status switch
        {
            SmsDeliveryStatus.Queued => "Message queued for sending",
            SmsDeliveryStatus.Sending => "Message is being sent",
            SmsDeliveryStatus.Sent => "Message sent to network",
            SmsDeliveryStatus.Delivered => "Message delivered to recipient",
            SmsDeliveryStatus.Read => "Message read by recipient",
            SmsDeliveryStatus.Failed => "Delivery failed",
            SmsDeliveryStatus.Undelivered => "Could not deliver message",
            SmsDeliveryStatus.Rejected => "Message rejected by provider",
            SmsDeliveryStatus.Canceled => "Message was canceled",
            _ => "Unknown status"
        };
    }
}

/// <summary>
/// SMS provider enumeration
/// </summary>
public enum SmsProvider
{
    /// <summary>
    /// Twilio
    /// </summary>
    Twilio = 0,

    /// <summary>
    /// Vonage (formerly Nexmo)
    /// </summary>
    Vonage = 1,

    /// <summary>
    /// Amazon SNS
    /// </summary>
    AmazonSns = 2,

    /// <summary>
    /// Azure Communication Services
    /// </summary>
    Azure = 3,

    /// <summary>
    /// Custom provider
    /// </summary>
    Custom = 99
}
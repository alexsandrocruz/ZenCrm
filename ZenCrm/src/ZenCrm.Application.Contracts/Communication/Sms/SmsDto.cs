using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Communication.Sms;

/// <summary>
/// SMS message DTO
/// </summary>
public class SmsDto : EntityDto<Guid>
{
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string MessageType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public MessagePriority Priority { get; set; }
    public DateTime? ScheduledSendDate { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public Guid? SmsTemplateId { get; set; }
    public string? SmsTemplateName { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "USD";
    public int Segments { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? InteractionId { get; set; }
    public string? CampaignId { get; set; }
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// Create/update SMS DTO
/// </summary>
public class CreateUpdateSmsDto
{
    public string Content { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public DateTime? ScheduledSendDate { get; set; }
    public Guid? SmsTemplateId { get; set; }
    public Dictionary<string, object>? TemplateVariables { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? InteractionId { get; set; }
    public string? CampaignId { get; set; }
}

/// <summary>
/// Send SMS request DTO
/// </summary>
public class SendSmsRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid? SmsTemplateId { get; set; }
    public string? Content { get; set; }
    public Dictionary<string, object>? Variables { get; set; }
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public DateTime? ScheduledSendDate { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? InteractionId { get; set; }
    public string? CampaignId { get; set; }
    public SmsCategory Category { get; set; } = SmsCategory.Transactional;
}

/// <summary>
/// Send bulk SMS request DTO
/// </summary>
public class SendBulkSmsRequestDto
{
    public List<string> PhoneNumbers { get; set; } = new();
    public Guid? SmsTemplateId { get; set; }
    public string? Content { get; set; }
    public Dictionary<string, object>? Variables { get; set; }
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public DateTime? ScheduledSendDate { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public string? CampaignId { get; set; }
    public SmsCategory Category { get; set; } = SmsCategory.Transactional;
}

/// <summary>
/// SMS send result DTO
/// </summary>
public class SmsSendResultDto
{
    public Guid MessageId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ExternalMessageId { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "USD";
    public int Segments { get; set; }
    public DateTime? SentAt { get; set; }
}

/// <summary>
/// SMS validation result DTO
/// </summary>
public class PhoneValidationResultDto
{
    public List<ValidPhoneNumberDto> ValidNumbers { get; set; } = new();
    public List<InvalidPhoneNumberDto> InvalidNumbers { get; set; } = new();
    public int TotalNumbers => ValidNumbers.Count + InvalidNumbers.Count;
    public bool AllValid => InvalidNumbers.Count == 0;
}

/// <summary>
/// Valid phone number DTO
/// </summary>
public class ValidPhoneNumberDto
{
    public string Original { get; set; } = string.Empty;
    public string Formatted { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Invalid phone number DTO
/// </summary>
public class InvalidPhoneNumberDto
{
    public string Original { get; set; } = string.Empty;
    public string ErrorReason { get; set; } = string.Empty;
}

/// <summary>
/// SMS cost calculation DTO
/// </summary>
public class SmsCostResultDto
{
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int Segments { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public bool IsInternational { get; set; }
    public string PricePerSegment { get; set; } = string.Empty;
}

/// <summary>
/// SMS delivery status DTO
/// </summary>
public class SmsDeliveryStatusDto
{
    public string MessageId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
}

/// <summary>
/// SMS delivery statistics DTO
/// </summary>
public class SmsDeliveryStatisticsDto
{
    public int TotalMessages { get; set; }
    public int QueuedMessages { get; set; }
    public int SentMessages { get; set; }
    public int DeliveredMessages { get; set; }
    public int ReadMessages { get; set; }
    public int FailedMessages { get; set; }
    public int UndeliveredMessages { get; set; }
    public int RejectedMessages { get; set; }
    public int CanceledMessages { get; set; }
    public decimal TotalCost { get; set; }
    public decimal AverageCost { get; set; }
    public double DeliveryRate { get; set; }
    public double ReadRate { get; set; }
    public double FailureRate { get; set; }
    public TimeSpan? AverageDeliveryTime { get; set; }
    public int TotalSegments { get; set; }
    public int TotalRetries { get; set; }
    public DateTime? LastActivity { get; set; }
}

/// <summary>
/// SMS lookup parameters
/// </summary>
public class SmsLookupParameters
{
    public string? Filter { get; set; }
    public string? Status { get; set; }
    public string? MessageType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ToPhoneNumber { get; set; }
    public string? CampaignId { get; set; }
    public int MaxResultCount { get; set; } = 10;
    public int SkipCount { get; set; } = 0;
    public string? Sorting { get; set; }
}
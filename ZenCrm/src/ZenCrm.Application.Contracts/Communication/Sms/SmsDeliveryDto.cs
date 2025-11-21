using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Communication.Sms;

/// <summary>
/// SMS delivery record DTO
/// </summary>
public class SmsDeliveryRecordDto : EntityDto<Guid>
{
    public Guid MessageId { get; set; }
    public string ExternalMessageId { get; set; } = string.Empty;
    public string ToPhoneNumber { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? PreviousStatus { get; set; }
    public SmsProvider Provider { get; set; }
    public SmsCategory Category { get; set; }
    public string MessageContent { get; set; } = string.Empty;
    public int Segments { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetryAttempts { get; set; }
    public DateTime? NextRetryAt { get; set; }
    public bool TrackingActive { get; set; }
    public DateTime? LastStatusCheckAt { get; set; }
    public string? DeliveryInfo { get; set; }
    public string RecipientCountry { get; set; } = string.Empty;
    public string? RecipientTimeZone { get; set; }
    public string? CampaignId { get; set; }
    public string Tags { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
}

/// <summary>
/// SMS delivery search parameters DTO
/// </summary>
public class SmsDeliverySearchParametersDto
{
    public Guid? MessageId { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ToPhoneNumber { get; set; }
    public string? FromPhoneNumber { get; set; }
    public string? Status { get; set; }
    public SmsProvider? Provider { get; set; }
    public SmsCategory? Category { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? CampaignId { get; set; }
    public string? Tags { get; set; }
    public bool? DeliveredSuccessfully { get; set; }
    public int MaxResultCount { get; set; } = 50;
    public int SkipCount { get; set; } = 0;
    public string? Sorting { get; set; } = "CreationTime desc";
}

/// <summary>
/// SMS delivery event DTO
/// </summary>
public class SmsDeliveryEventDto
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Daily delivery statistics DTO
/// </summary>
public class DailyDeliveryStatsDto
{
    public DateTime Date { get; set; }
    public int TotalMessages { get; set; }
    public int DeliveredMessages { get; set; }
    public int FailedMessages { get; set; }
    public decimal TotalCost { get; set; }
    public double DeliveryRate { get; set; }
}

/// <summary>
/// Top destination statistics DTO
/// </summary>
public class TopDestinationStatsDto
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public int MessageCount { get; set; }
    public int DeliveredCount { get; set; }
    public decimal TotalCost { get; set; }
    public double DeliveryRate { get; set; }
}

/// <summary>
/// SMS delivery report parameters DTO
/// </summary>
public class SmsDeliveryReportParametersDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SmsProvider? Provider { get; set; }
    public SmsCategory? Category { get; set; }
    public string? CampaignId { get; set; }
    public ReportType ReportType { get; set; }
}

/// <summary>
/// SMS delivery report DTO
/// </summary>
public class SmsDeliveryReportDto
{
    public ReportType ReportType { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public SmsDeliveryStatisticsDto OverallStatistics { get; set; } = new();
    public List<DailyDeliveryStatsDto> DailyBreakdown { get; set; } = new();
    public List<TopDestinationStatsDto> TopDestinations { get; set; } = new();
    public Dictionary<SmsProvider, SmsDeliveryStatisticsDto> ProviderBreakdown { get; set; } = new();
    public Dictionary<SmsCategory, SmsDeliveryStatisticsDto> CategoryBreakdown { get; set; } = new();
    public byte[]? ExportData { get; set; }
    public string? ExportFormat { get; set; }
}

/// <summary>
/// SMS delivery timeline DTO
/// </summary>
public class SmsDeliveryTimelineDto
{
    public Guid RecordId { get; set; }
    public List<SmsDeliveryEventDto> Events { get; set; } = new();
}

/// <summary>
/// SMS delivery sync request DTO
/// </summary>
public class SmsDeliverySyncRequestDto
{
    public SmsProvider Provider { get; set; } = SmsProvider.Twilio;
    public int MaxRecords { get; set; } = 100;
}

/// <summary>
/// SMS delivery sync result DTO
/// </summary>
public class SmsDeliverySyncResultDto
{
    public int SyncedRecords { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SyncTime { get; set; }
}

/// <summary>
/// SMS delivery cleanup request DTO
/// </summary>
public class SmsDeliveryCleanupRequestDto
{
    public int DaysToKeep { get; set; } = 90;
}

/// <summary>
/// SMS delivery cleanup result DTO
/// </summary>
public class SmsDeliveryCleanupResultDto
{
    public int DeletedRecords { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CleanupTime { get; set; }
}
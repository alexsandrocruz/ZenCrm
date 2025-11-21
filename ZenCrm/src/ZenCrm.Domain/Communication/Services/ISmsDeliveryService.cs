using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// SMS delivery tracking service interface
/// </summary>
public interface ISmsDeliveryService
{
    /// <summary>
    /// Create a new SMS delivery record
    /// </summary>
    Task<SmsDeliveryRecord> CreateAsync(SmsDeliveryRecord record);

    /// <summary>
    /// Update SMS delivery status
    /// </summary>
    Task<SmsDeliveryRecord> UpdateStatusAsync(Guid recordId, SmsDeliveryStatus status, string errorCode = null, string errorMessage = null);

    /// <summary>
    /// Update SMS delivery status by external message ID
    /// </summary>
    Task<SmsDeliveryRecord> UpdateStatusByExternalIdAsync(string externalMessageId, SmsDeliveryStatus status, string errorCode = null, string errorMessage = null);

    /// <summary>
    /// Get SMS delivery record by ID
    /// </summary>
    Task<SmsDeliveryRecord> GetAsync(Guid id);

    /// <summary>
    /// Get SMS delivery record by external message ID
    /// </summary>
    Task<SmsDeliveryRecord> GetByExternalIdAsync(string externalMessageId);

    /// <summary>
    /// Get SMS delivery records by message ID
    /// </summary>
    Task<List<SmsDeliveryRecord>> GetByMessageIdAsync(Guid messageId);

    /// <summary>
    /// Get SMS delivery records by phone number
    /// </summary>
    Task<List<SmsDeliveryRecord>> GetByPhoneNumberAsync(string phoneNumber, int limit = 50);

    /// <summary>
    /// Get SMS delivery records by status
    /// </summary>
    Task<List<SmsDeliveryRecord>> GetByStatusAsync(SmsDeliveryStatus status);

    /// <summary>
    /// Get SMS delivery records requiring retry
    /// </summary>
    Task<List<SmsDeliveryRecord>> GetPendingRetriesAsync();

    /// <summary>
    /// Get SMS delivery records with active tracking
    /// </summary>
    Task<List<SmsDeliveryRecord>> GetActiveTrackingAsync();

    /// <summary>
    /// Search SMS delivery records
    /// </summary>
    Task<List<SmsDeliveryRecord>> SearchAsync(SmsDeliverySearchParameters parameters);

    /// <summary>
    /// Get SMS delivery statistics
    /// </summary>
    Task<SmsDeliveryStatistics> GetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Get SMS delivery statistics by category
    /// </summary>
    Task<Dictionary<SmsCategory, SmsDeliveryStatistics>> GetStatisticsByCategoryAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Get SMS delivery statistics by provider
    /// </summary>
    Task<Dictionary<SmsProvider, SmsDeliveryStatistics>> GetStatisticsByProviderAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Process delivery status updates from provider
    /// </summary>
    Task<int> ProcessProviderUpdatesAsync(SmsProvider provider, Dictionary<string, SmsDeliveryStatus> updates);

    /// <summary>
    /// Sync delivery status with provider
    /// </summary>
    Task<int> SyncWithProviderAsync(SmsProvider provider, int maxRecords = 100);

    /// <summary>
    /// Retry failed messages
    /// </summary>
    Task<int> RetryFailedMessagesAsync(int maxRetries = 10);

    /// <summary>
    /// Cancel pending messages
    /// </summary>
    Task<int> CancelPendingMessagesAsync(string campaignId = null);

    /// <summary>
    /// Get delivery timeline for a message
    /// </summary>
    Task<List<SmsDeliveryEvent>> GetDeliveryTimelineAsync(Guid recordId);

    /// <summary>
    /// Get daily delivery statistics
    /// </summary>
    Task<List<DailyDeliveryStats>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get top delivery destinations
    /// </summary>
    Task<List<TopDestinationStats>> GetTopDestinationsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Clean up old delivery records
    /// </summary>
    Task<int> CleanupOldRecordsAsync(int daysToKeep = 90);

    /// <summary>
    /// Export delivery records to CSV
    /// </summary>
    Task<string> ExportToCsvAsync(SmsDeliverySearchParameters parameters);

    /// <summary>
    /// Generate delivery report
    /// </summary>
    Task<SmsDeliveryReport> GenerateReportAsync(ReportParameters parameters);
}

/// <summary>
/// SMS delivery search parameters
/// </summary>
public class SmsDeliverySearchParameters
{
    public Guid? MessageId { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ToPhoneNumber { get; set; }
    public string? FromPhoneNumber { get; set; }
    public SmsDeliveryStatus? Status { get; set; }
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
/// SMS delivery statistics
/// </summary>
public class SmsDeliveryStatistics
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
/// SMS delivery event
/// </summary>
public class SmsDeliveryEvent
{
    public DateTime Timestamp { get; set; }
    public SmsDeliveryStatus Status { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Daily delivery statistics
/// </summary>
public class DailyDeliveryStats
{
    public DateTime Date { get; set; }
    public int TotalMessages { get; set; }
    public int DeliveredMessages { get; set; }
    public int FailedMessages { get; set; }
    public decimal TotalCost { get; set; }
    public double DeliveryRate { get; set; }
}

/// <summary>
/// Top destination statistics
/// </summary>
public class TopDestinationStats
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public int MessageCount { get; set; }
    public int DeliveredCount { get; set; }
    public decimal TotalCost { get; set; }
    public double DeliveryRate { get; set; }
}

/// <summary>
/// SMS delivery report parameters
/// </summary>
public class ReportParameters
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SmsProvider? Provider { get; set; }
    public SmsCategory? Category { get; set; }
    public string? CampaignId { get; set; }
    public ReportType ReportType { get; set; }
}

/// <summary>
/// SMS delivery report
/// </summary>
public class SmsDeliveryReport
{
    public ReportType ReportType { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public SmsDeliveryStatistics OverallStatistics { get; set; } = new();
    public List<DailyDeliveryStats> DailyBreakdown { get; set; } = new();
    public List<TopDestinationStats> TopDestinations { get; set; } = new();
    public Dictionary<SmsProvider, SmsDeliveryStatistics> ProviderBreakdown { get; set; } = new();
    public Dictionary<SmsCategory, SmsDeliveryStatistics> CategoryBreakdown { get; set; } = new();
    public byte[]? ExportData { get; set; }
    public string? ExportFormat { get; set; }
}

/// <summary>
/// Report type enumeration
/// </summary>
public enum ReportType
{
    Summary = 0,
    Detailed = 1,
    ByProvider = 2,
    ByCategory = 3,
    ByDestination = 4
}
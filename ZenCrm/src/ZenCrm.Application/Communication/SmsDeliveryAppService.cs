using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using ZenCrm.Communication.Entities;
using ZenCrm.Communication.Services;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Communication;

/// <summary>
/// SMS delivery tracking application service
/// </summary>
public class SmsDeliveryAppService : ApplicationService, ISmsDeliveryAppService
{
    private readonly ISmsDeliveryService _smsDeliveryService;

    public SmsDeliveryAppService(ISmsDeliveryService smsDeliveryService)
    {
        _smsDeliveryService = smsDeliveryService;
    }

    /// <summary>
    /// Get SMS delivery record by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<SmsDeliveryRecordDto> GetAsync(Guid id)
    {
        var record = await _smsDeliveryService.GetAsync(id);
        return ObjectMapper.Map<SmsDeliveryRecord, SmsDeliveryRecordDto>(record);
    }

    /// <summary>
    /// Get SMS delivery record by external message ID
    /// </summary>
    [HttpGet("by-external-id/{externalMessageId}")]
    public async Task<SmsDeliveryRecordDto> GetByExternalIdAsync(string externalMessageId)
    {
        var record = await _smsDeliveryService.GetByExternalIdAsync(externalMessageId);
        return ObjectMapper.Map<SmsDeliveryRecord, SmsDeliveryRecordDto>(record);
    }

    /// <summary>
    /// Get SMS delivery records by message ID
    /// </summary>
    [HttpGet("by-message-id/{messageId}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByMessageIdAsync(Guid messageId)
    {
        var records = await _smsDeliveryService.GetByMessageIdAsync(messageId);
        return ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records);
    }

    /// <summary>
    /// Get SMS delivery records by phone number
    /// </summary>
    [HttpGet("by-phone-number/{phoneNumber}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByPhoneNumberAsync(string phoneNumber, [FromQuery] int limit = 50)
    {
        var records = await _smsDeliveryService.GetByPhoneNumberAsync(phoneNumber, limit);
        return ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records);
    }

    /// <summary>
    /// Get SMS delivery records by status
    /// </summary>
    [HttpGet("by-status/{status}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByStatusAsync(Entities.SmsDeliveryStatus status)
    {
        var records = await _smsDeliveryService.GetByStatusAsync(status);
        return ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records);
    }

    /// <summary>
    /// Search SMS delivery records
    /// </summary>
    [HttpPost("search")]
    public async Task<PagedResultDto<SmsDeliveryRecordDto>> SearchAsync([FromBody] SmsDeliverySearchParametersDto parameters)
    {
        var searchParams = new SmsDeliverySearchParameters
        {
            MessageId = parameters.MessageId,
            ExternalMessageId = parameters.ExternalMessageId,
            ToPhoneNumber = parameters.ToPhoneNumber,
            FromPhoneNumber = parameters.FromPhoneNumber,
            Status = !string.IsNullOrWhiteSpace(parameters.Status)
                ? Enum.Parse<Entities.SmsDeliveryStatus>(parameters.Status)
                : null,
            Provider = parameters.Provider,
            Category = parameters.Category,
            StartDate = parameters.StartDate,
            EndDate = parameters.EndDate,
            CampaignId = parameters.CampaignId,
            Tags = parameters.Tags,
            DeliveredSuccessfully = parameters.DeliveredSuccessfully,
            MaxResultCount = parameters.MaxResultCount,
            SkipCount = parameters.SkipCount,
            Sorting = parameters.Sorting
        };

        var records = await _smsDeliveryService.SearchAsync(searchParams);
        var totalCount = records.Count; // Simplified - in real implementation would get total count separately

        return new PagedResultDto<SmsDeliveryRecordDto>(
            totalCount,
            ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records)
        );
    }

    /// <summary>
    /// Get SMS delivery records requiring retry
    /// </summary>
    [HttpGet("pending-retries")]
    public async Task<List<SmsDeliveryRecordDto>> GetPendingRetriesAsync()
    {
        var records = await _smsDeliveryService.GetPendingRetriesAsync();
        return ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records);
    }

    /// <summary>
    /// Get SMS delivery records with active tracking
    /// </summary>
    [HttpGet("active-tracking")]
    public async Task<List<SmsDeliveryRecordDto>> GetActiveTrackingAsync()
    {
        var records = await _smsDeliveryService.GetActiveTrackingAsync();
        return ObjectMapper.Map<List<SmsDeliveryRecord>, List<SmsDeliveryRecordDto>>(records);
    }

    /// <summary>
    /// Get SMS delivery statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<SmsDeliveryStatisticsDto> GetStatisticsAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var stats = await _smsDeliveryService.GetStatisticsAsync(startDate, endDate);

        return new SmsDeliveryStatisticsDto
        {
            TotalMessages = stats.TotalMessages,
            QueuedMessages = stats.QueuedMessages,
            SentMessages = stats.SentMessages,
            DeliveredMessages = stats.DeliveredMessages,
            ReadMessages = stats.ReadMessages,
            FailedMessages = stats.FailedMessages,
            UndeliveredMessages = stats.UndeliveredMessages,
            RejectedMessages = stats.RejectedMessages,
            CanceledMessages = stats.CanceledMessages,
            TotalCost = stats.TotalCost,
            AverageCost = stats.AverageCost,
            DeliveryRate = stats.DeliveryRate,
            ReadRate = stats.ReadRate,
            FailureRate = stats.FailureRate,
            AverageDeliveryTime = stats.AverageDeliveryTime,
            TotalSegments = stats.TotalSegments,
            TotalRetries = stats.TotalRetries,
            LastActivity = stats.LastActivity
        };
    }

    /// <summary>
    /// Get SMS delivery statistics by category
    /// </summary>
    [HttpGet("statistics-by-category")]
    public async Task<Dictionary<SmsCategory, SmsDeliveryStatisticsDto>> GetStatisticsByCategoryAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var stats = await _smsDeliveryService.GetStatisticsByCategoryAsync(startDate, endDate);

        return stats.ToDictionary(
            kvp => kvp.Key,
            kvp => new SmsDeliveryStatisticsDto
            {
                TotalMessages = kvp.Value.TotalMessages,
                QueuedMessages = kvp.Value.QueuedMessages,
                SentMessages = kvp.Value.SentMessages,
                DeliveredMessages = kvp.Value.DeliveredMessages,
                ReadMessages = kvp.Value.ReadMessages,
                FailedMessages = kvp.Value.FailedMessages,
                UndeliveredMessages = kvp.Value.UndeliveredMessages,
                RejectedMessages = kvp.Value.RejectedMessages,
                CanceledMessages = kvp.Value.CanceledMessages,
                TotalCost = kvp.Value.TotalCost,
                AverageCost = kvp.Value.AverageCost,
                DeliveryRate = kvp.Value.DeliveryRate,
                ReadRate = kvp.Value.ReadRate,
                FailureRate = kvp.Value.FailureRate,
                AverageDeliveryTime = kvp.Value.AverageDeliveryTime,
                TotalSegments = kvp.Value.TotalSegments,
                TotalRetries = kvp.Value.TotalRetries,
                LastActivity = kvp.Value.LastActivity
            }
        );
    }

    /// <summary>
    /// Get SMS delivery statistics by provider
    /// </summary>
    [HttpGet("statistics-by-provider")]
    public async Task<Dictionary<SmsProvider, SmsDeliveryStatisticsDto>> GetStatisticsByProviderAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var stats = await _smsDeliveryService.GetStatisticsByProviderAsync(startDate, endDate);

        return stats.ToDictionary(
            kvp => kvp.Key,
            kvp => new SmsDeliveryStatisticsDto
            {
                TotalMessages = kvp.Value.TotalMessages,
                QueuedMessages = kvp.Value.QueuedMessages,
                SentMessages = kvp.Value.SentMessages,
                DeliveredMessages = kvp.Value.DeliveredMessages,
                ReadMessages = kvp.Value.ReadMessages,
                FailedMessages = kvp.Value.FailedMessages,
                UndeliveredMessages = kvp.Value.UndeliveredMessages,
                RejectedMessages = kvp.Value.RejectedMessages,
                CanceledMessages = kvp.Value.CanceledMessages,
                TotalCost = kvp.Value.TotalCost,
                AverageCost = kvp.Value.AverageCost,
                DeliveryRate = kvp.Value.DeliveryRate,
                ReadRate = kvp.Value.ReadRate,
                FailureRate = kvp.Value.FailureRate,
                AverageDeliveryTime = kvp.Value.AverageDeliveryTime,
                TotalSegments = kvp.Value.TotalSegments,
                TotalRetries = kvp.Value.TotalRetries,
                LastActivity = kvp.Value.LastActivity
            }
        );
    }

    /// <summary>
    /// Process delivery status updates from provider
    /// </summary>
    [HttpPost("process-provider-updates")]
    public async Task<int> ProcessProviderUpdatesAsync([FromBody] ProcessProviderUpdatesRequestDto request)
    {
        var updates = request.Updates.ToDictionary(
            kvp => kvp.Key,
            kvp => Enum.Parse<Entities.SmsDeliveryStatus>(kvp.Value.ToString())
        );

        return await _smsDeliveryService.ProcessProviderUpdatesAsync(request.Provider, updates);
    }

    /// <summary>
    /// Sync delivery status with provider
    /// </summary>
    [HttpPost("sync-with-provider")]
    public async Task<int> SyncWithProviderAsync([FromBody] SmsDeliverySyncRequestDto request)
    {
        return await _smsDeliveryService.SyncWithProviderAsync(request.Provider, request.MaxRecords);
    }

    /// <summary>
    /// Retry failed messages
    /// </summary>
    [HttpPost("retry-failed-messages")]
    public async Task<int> RetryFailedMessagesAsync([FromQuery] int maxRetries = 10)
    {
        return await _smsDeliveryService.RetryFailedMessagesAsync(maxRetries);
    }

    /// <summary>
    /// Cancel pending messages
    /// </summary>
    [HttpPost("cancel-pending-messages")]
    public async Task<int> CancelPendingMessagesAsync([FromQuery] string? campaignId = null)
    {
        return await _smsDeliveryService.CancelPendingMessagesAsync(campaignId);
    }

    /// <summary>
    /// Get delivery timeline for a message
    /// </summary>
    [HttpGet("timeline/{recordId}")]
    public async Task<SmsDeliveryTimelineDto> GetDeliveryTimelineAsync(Guid recordId)
    {
        var events = await _smsDeliveryService.GetDeliveryTimelineAsync(recordId);

        return new SmsDeliveryTimelineDto
        {
            RecordId = recordId,
            Events = events.Select(x => new SmsDeliveryEventDto
            {
                Timestamp = x.Timestamp,
                Status = x.Status.ToString(),
                ErrorCode = x.ErrorCode,
                ErrorMessage = x.ErrorMessage,
                Description = x.Description
            }).ToList()
        };
    }

    /// <summary>
    /// Get daily delivery statistics
    /// </summary>
    [HttpGet("daily-statistics")]
    public async Task<List<DailyDeliveryStatsDto>> GetDailyStatisticsAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var stats = await _smsDeliveryService.GetDailyStatisticsAsync(startDate, endDate);

        return stats.Select(x => new DailyDeliveryStatsDto
        {
            Date = x.Date,
            TotalMessages = x.TotalMessages,
            DeliveredMessages = x.DeliveredMessages,
            FailedMessages = x.FailedMessages,
            TotalCost = x.TotalCost,
            DeliveryRate = x.DeliveryRate
        }).ToList();
    }

    /// <summary>
    /// Get top delivery destinations
    /// </summary>
    [HttpGet("top-destinations")]
    public async Task<List<TopDestinationStatsDto>> GetTopDestinationsAsync([FromQuery] int limit = 10, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var stats = await _smsDeliveryService.GetTopDestinationsAsync(limit, startDate, endDate);

        return stats.Select(x => new TopDestinationStatsDto
        {
            CountryCode = x.CountryCode,
            CountryName = x.CountryName,
            MessageCount = x.MessageCount,
            DeliveredCount = x.DeliveredCount,
            TotalCost = x.TotalCost,
            DeliveryRate = x.DeliveryRate
        }).ToList();
    }

    /// <summary>
    /// Clean up old delivery records
    /// </summary>
    [HttpPost("cleanup-old-records")]
    public async Task<SmsDeliveryCleanupResultDto> CleanupOldRecordsAsync([FromBody] SmsDeliveryCleanupRequestDto request)
    {
        try
        {
            var deletedCount = await _smsDeliveryService.CleanupOldRecordsAsync(request.DaysToKeep);

            return new SmsDeliveryCleanupResultDto
            {
                DeletedRecords = deletedCount,
                Success = true,
                CleanupTime = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            return new SmsDeliveryCleanupResultDto
            {
                DeletedRecords = 0,
                Success = false,
                ErrorMessage = ex.Message,
                CleanupTime = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Export delivery records to CSV
    /// </summary>
    [HttpPost("export-csv")]
    public async Task<string> ExportToCsvAsync([FromBody] SmsDeliverySearchParametersDto parameters)
    {
        var searchParams = new SmsDeliverySearchParameters
        {
            MessageId = parameters.MessageId,
            ExternalMessageId = parameters.ExternalMessageId,
            ToPhoneNumber = parameters.ToPhoneNumber,
            FromPhoneNumber = parameters.FromPhoneNumber,
            Status = !string.IsNullOrWhiteSpace(parameters.Status)
                ? Enum.Parse<Entities.SmsDeliveryStatus>(parameters.Status)
                : null,
            Provider = parameters.Provider,
            Category = parameters.Category,
            StartDate = parameters.StartDate,
            EndDate = parameters.EndDate,
            CampaignId = parameters.CampaignId,
            Tags = parameters.Tags,
            DeliveredSuccessfully = parameters.DeliveredSuccessfully,
            MaxResultCount = parameters.MaxResultCount,
            SkipCount = parameters.SkipCount,
            Sorting = parameters.Sorting
        };

        return await _smsDeliveryService.ExportToCsvAsync(searchParams);
    }

    /// <summary>
    /// Generate delivery report
    /// </summary>
    [HttpPost("generate-report")]
    public async Task<SmsDeliveryReportDto> GenerateReportAsync([FromBody] SmsDeliveryReportParametersDto parameters)
    {
        var reportParams = new ReportParameters
        {
            StartDate = parameters.StartDate,
            EndDate = parameters.EndDate,
            Provider = parameters.Provider,
            Category = parameters.Category,
            CampaignId = parameters.CampaignId,
            ReportType = parameters.ReportType
        };

        var report = await _smsDeliveryService.GenerateReportAsync(reportParams);

        return new SmsDeliveryReportDto
        {
            ReportType = report.ReportType,
            GeneratedAt = report.GeneratedAt,
            PeriodStart = report.PeriodStart,
            PeriodEnd = report.PeriodEnd,
            OverallStatistics = new SmsDeliveryStatisticsDto
            {
                TotalMessages = report.OverallStatistics.TotalMessages,
                QueuedMessages = report.OverallStatistics.QueuedMessages,
                SentMessages = report.OverallStatistics.SentMessages,
                DeliveredMessages = report.OverallStatistics.DeliveredMessages,
                ReadMessages = report.OverallStatistics.ReadMessages,
                FailedMessages = report.OverallStatistics.FailedMessages,
                UndeliveredMessages = report.OverallStatistics.UndeliveredMessages,
                RejectedMessages = report.OverallStatistics.RejectedMessages,
                CanceledMessages = report.OverallStatistics.CanceledMessages,
                TotalCost = report.OverallStatistics.TotalCost,
                AverageCost = report.OverallStatistics.AverageCost,
                DeliveryRate = report.OverallStatistics.DeliveryRate,
                ReadRate = report.OverallStatistics.ReadRate,
                FailureRate = report.OverallStatistics.FailureRate,
                AverageDeliveryTime = report.OverallStatistics.AverageDeliveryTime,
                TotalSegments = report.OverallStatistics.TotalSegments,
                TotalRetries = report.OverallStatistics.TotalRetries,
                LastActivity = report.OverallStatistics.LastActivity
            },
            DailyBreakdown = report.DailyBreakdown.Select(x => new DailyDeliveryStatsDto
            {
                Date = x.Date,
                TotalMessages = x.TotalMessages,
                DeliveredMessages = x.DeliveredMessages,
                FailedMessages = x.FailedMessages,
                TotalCost = x.TotalCost,
                DeliveryRate = x.DeliveryRate
            }).ToList(),
            TopDestinations = report.TopDestinations.Select(x => new TopDestinationStatsDto
            {
                CountryCode = x.CountryCode,
                CountryName = x.CountryName,
                MessageCount = x.MessageCount,
                DeliveredCount = x.DeliveredCount,
                TotalCost = x.TotalCost,
                DeliveryRate = x.DeliveryRate
            }).ToList(),
            ProviderBreakdown = report.ProviderBreakdown.ToDictionary(
                kvp => kvp.Key,
                kvp => new SmsDeliveryStatisticsDto
                {
                    TotalMessages = kvp.Value.TotalMessages,
                    QueuedMessages = kvp.Value.QueuedMessages,
                    SentMessages = kvp.Value.SentMessages,
                    DeliveredMessages = kvp.Value.DeliveredMessages,
                    ReadMessages = kvp.Value.ReadMessages,
                    FailedMessages = kvp.Value.FailedMessages,
                    UndeliveredMessages = kvp.Value.UndeliveredMessages,
                    RejectedMessages = kvp.Value.RejectedMessages,
                    CanceledMessages = kvp.Value.CanceledMessages,
                    TotalCost = kvp.Value.TotalCost,
                    AverageCost = kvp.Value.AverageCost,
                    DeliveryRate = kvp.Value.DeliveryRate,
                    ReadRate = kvp.Value.ReadRate,
                    FailureRate = kvp.Value.FailureRate,
                    AverageDeliveryTime = kvp.Value.AverageDeliveryTime,
                    TotalSegments = kvp.Value.TotalSegments,
                    TotalRetries = kvp.Value.TotalRetries,
                    LastActivity = kvp.Value.LastActivity
                }
            ),
            CategoryBreakdown = report.CategoryBreakdown.ToDictionary(
                kvp => kvp.Key,
                kvp => new SmsDeliveryStatisticsDto
                {
                    TotalMessages = kvp.Value.TotalMessages,
                    QueuedMessages = kvp.Value.QueuedMessages,
                    SentMessages = kvp.Value.SentMessages,
                    DeliveredMessages = kvp.Value.DeliveredMessages,
                    ReadMessages = kvp.Value.ReadMessages,
                    FailedMessages = kvp.Value.FailedMessages,
                    UndeliveredMessages = kvp.Value.UndeliveredMessages,
                    RejectedMessages = kvp.Value.RejectedMessages,
                    CanceledMessages = kvp.Value.CanceledMessages,
                    TotalCost = kvp.Value.TotalCost,
                    AverageCost = kvp.Value.AverageCost,
                    DeliveryRate = kvp.Value.DeliveryRate,
                    ReadRate = kvp.Value.ReadRate,
                    FailureRate = kvp.Value.FailureRate,
                    AverageDeliveryTime = kvp.Value.AverageDeliveryTime,
                    TotalSegments = kvp.Value.TotalSegments,
                    TotalRetries = kvp.Value.TotalRetries,
                    LastActivity = kvp.Value.LastActivity
                }
            ),
            ExportData = report.ExportData,
            ExportFormat = report.ExportFormat
        };
    }
}

/// <summary>
/// SMS delivery app service interface
/// </summary>
public interface ISmsDeliveryAppService : IApplicationService
{
    Task<SmsDeliveryRecordDto> GetAsync(Guid id);
    Task<SmsDeliveryRecordDto> GetByExternalIdAsync(string externalMessageId);
    Task<List<SmsDeliveryRecordDto>> GetByMessageIdAsync(Guid messageId);
    Task<List<SmsDeliveryRecordDto>> GetByPhoneNumberAsync(string phoneNumber, int limit = 50);
    Task<List<SmsDeliveryRecordDto>> GetByStatusAsync(Entities.SmsDeliveryStatus status);
    Task<PagedResultDto<SmsDeliveryRecordDto>> SearchAsync(SmsDeliverySearchParametersDto parameters);
    Task<List<SmsDeliveryRecordDto>> GetPendingRetriesAsync();
    Task<List<SmsDeliveryRecordDto>> GetActiveTrackingAsync();
    Task<SmsDeliveryStatisticsDto> GetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<SmsCategory, SmsDeliveryStatisticsDto>> GetStatisticsByCategoryAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<SmsProvider, SmsDeliveryStatisticsDto>> GetStatisticsByProviderAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<int> ProcessProviderUpdatesAsync(ProcessProviderUpdatesRequestDto request);
    Task<int> SyncWithProviderAsync(SmsDeliverySyncRequestDto request);
    Task<int> RetryFailedMessagesAsync(int maxRetries = 10);
    Task<int> CancelPendingMessagesAsync(string? campaignId = null);
    Task<SmsDeliveryTimelineDto> GetDeliveryTimelineAsync(Guid recordId);
    Task<List<DailyDeliveryStatsDto>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate);
    Task<List<TopDestinationStatsDto>> GetTopDestinationsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);
    Task<SmsDeliveryCleanupResultDto> CleanupOldRecordsAsync(SmsDeliveryCleanupRequestDto request);
    Task<string> ExportToCsvAsync(SmsDeliverySearchParametersDto parameters);
    Task<SmsDeliveryReportDto> GenerateReportAsync(SmsDeliveryReportParametersDto parameters);
}

/// <summary>
/// Process provider updates request DTO
/// </summary>
public class ProcessProviderUpdatesRequestDto
{
    public SmsProvider Provider { get; set; }
    public Dictionary<string, object> Updates { get; set; } = new();
}
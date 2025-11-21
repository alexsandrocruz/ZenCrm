using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using ZenCrm.Communication;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Controllers;

/// <summary>
/// SMS delivery tracking controller
/// </summary>
[ApiController]
[Route("api/sms-delivery")]
public class SmsDeliveryController : AbpControllerBase
{
    private readonly ISmsDeliveryAppService _smsDeliveryAppService;

    public SmsDeliveryController(ISmsDeliveryAppService smsDeliveryAppService)
    {
        _smsDeliveryAppService = smsDeliveryAppService;
    }

    /// <summary>
    /// Get SMS delivery record by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<SmsDeliveryRecordDto> GetAsync(Guid id)
    {
        return await _smsDeliveryAppService.GetAsync(id);
    }

    /// <summary>
    /// Get SMS delivery record by external message ID
    /// </summary>
    [HttpGet("by-external-id/{externalMessageId}")]
    public async Task<SmsDeliveryRecordDto> GetByExternalIdAsync(string externalMessageId)
    {
        return await _smsDeliveryAppService.GetByExternalIdAsync(externalMessageId);
    }

    /// <summary>
    /// Get SMS delivery records by message ID
    /// </summary>
    [HttpGet("by-message-id/{messageId}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByMessageIdAsync(Guid messageId)
    {
        return await _smsDeliveryAppService.GetByMessageIdAsync(messageId);
    }

    /// <summary>
    /// Get SMS delivery records by phone number
    /// </summary>
    [HttpGet("by-phone-number/{phoneNumber}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByPhoneNumberAsync(string phoneNumber, [FromQuery] int limit = 50)
    {
        return await _smsDeliveryAppService.GetByPhoneNumberAsync(phoneNumber, limit);
    }

    /// <summary>
    /// Get SMS delivery records by status
    /// </summary>
    [HttpGet("by-status/{status}")]
    public async Task<List<SmsDeliveryRecordDto>> GetByStatusAsync(string status)
    {
        var statusEnum = Enum.Parse<ZenCrm.Communication.Entities.SmsDeliveryStatus>(status);
        return await _smsDeliveryAppService.GetByStatusAsync(statusEnum);
    }

    /// <summary>
    /// Search SMS delivery records
    /// </summary>
    [HttpPost("search")]
    public async Task<PagedResultDto<SmsDeliveryRecordDto>> SearchAsync([FromBody] SmsDeliverySearchParametersDto parameters)
    {
        return await _smsDeliveryAppService.SearchAsync(parameters);
    }

    /// <summary>
    /// Get SMS delivery records requiring retry
    /// </summary>
    [HttpGet("pending-retries")]
    public async Task<List<SmsDeliveryRecordDto>> GetPendingRetriesAsync()
    {
        return await _smsDeliveryAppService.GetPendingRetriesAsync();
    }

    /// <summary>
    /// Get SMS delivery records with active tracking
    /// </summary>
    [HttpGet("active-tracking")]
    public async Task<List<SmsDeliveryRecordDto>> GetActiveTrackingAsync()
    {
        return await _smsDeliveryAppService.GetActiveTrackingAsync();
    }

    /// <summary>
    /// Get SMS delivery statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<SmsDeliveryStatisticsDto> GetStatisticsAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return await _smsDeliveryAppService.GetStatisticsAsync(startDate, endDate);
    }

    /// <summary>
    /// Get SMS delivery statistics by category
    /// </summary>
    [HttpGet("statistics-by-category")]
    public async Task<Dictionary<SmsCategory, SmsDeliveryStatisticsDto>> GetStatisticsByCategoryAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return await _smsDeliveryAppService.GetStatisticsByCategoryAsync(startDate, endDate);
    }

    /// <summary>
    /// Get SMS delivery statistics by provider
    /// </summary>
    [HttpGet("statistics-by-provider")]
    public async Task<Dictionary<SmsProvider, SmsDeliveryStatisticsDto>> GetStatisticsByProviderAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return await _smsDeliveryAppService.GetStatisticsByProviderAsync(startDate, endDate);
    }

    /// <summary>
    /// Process delivery status updates from provider
    /// </summary>
    [HttpPost("process-provider-updates")]
    public async Task<int> ProcessProviderUpdatesAsync([FromBody] ProcessProviderUpdatesRequestDto request)
    {
        return await _smsDeliveryAppService.ProcessProviderUpdatesAsync(request);
    }

    /// <summary>
    /// Sync delivery status with provider
    /// </summary>
    [HttpPost("sync-with-provider")]
    public async Task<int> SyncWithProviderAsync([FromBody] SmsDeliverySyncRequestDto request)
    {
        return await _smsDeliveryAppService.SyncWithProviderAsync(request);
    }

    /// <summary>
    /// Retry failed messages
    /// </summary>
    [HttpPost("retry-failed-messages")]
    public async Task<int> RetryFailedMessagesAsync([FromQuery] int maxRetries = 10)
    {
        return await _smsDeliveryAppService.RetryFailedMessagesAsync(maxRetries);
    }

    /// <summary>
    /// Cancel pending messages
    /// </summary>
    [HttpPost("cancel-pending-messages")]
    public async Task<int> CancelPendingMessagesAsync([FromQuery] string? campaignId = null)
    {
        return await _smsDeliveryAppService.CancelPendingMessagesAsync(campaignId);
    }

    /// <summary>
    /// Get delivery timeline for a message
    /// </summary>
    [HttpGet("timeline/{recordId}")]
    public async Task<SmsDeliveryTimelineDto> GetDeliveryTimelineAsync(Guid recordId)
    {
        return await _smsDeliveryAppService.GetDeliveryTimelineAsync(recordId);
    }

    /// <summary>
    /// Get daily delivery statistics
    /// </summary>
    [HttpGet("daily-statistics")]
    public async Task<List<DailyDeliveryStatsDto>> GetDailyStatisticsAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        return await _smsDeliveryAppService.GetDailyStatisticsAsync(startDate, endDate);
    }

    /// <summary>
    /// Get top delivery destinations
    /// </summary>
    [HttpGet("top-destinations")]
    public async Task<List<TopDestinationStatsDto>> GetTopDestinationsAsync([FromQuery] int limit = 10, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return await _smsDeliveryAppService.GetTopDestinationsAsync(limit, startDate, endDate);
    }

    /// <summary>
    /// Clean up old delivery records
    /// </summary>
    [HttpPost("cleanup-old-records")]
    public async Task<SmsDeliveryCleanupResultDto> CleanupOldRecordsAsync([FromBody] SmsDeliveryCleanupRequestDto request)
    {
        return await _smsDeliveryAppService.CleanupOldRecordsAsync(request);
    }

    /// <summary>
    /// Export delivery records to CSV
    /// </summary>
    [HttpPost("export-csv")]
    public async Task<string> ExportToCsvAsync([FromBody] SmsDeliverySearchParametersDto parameters)
    {
        return await _smsDeliveryAppService.ExportToCsvAsync(parameters);
    }

    /// <summary>
    /// Generate delivery report
    /// </summary>
    [HttpPost("generate-report")]
    public async Task<SmsDeliveryReportDto> GenerateReportAsync([FromBody] SmsDeliveryReportParametersDto parameters)
    {
        return await _smsDeliveryAppService.GenerateReportAsync(parameters);
    }
}
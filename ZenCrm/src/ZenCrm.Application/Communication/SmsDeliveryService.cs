using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// SMS delivery tracking service implementation
/// </summary>
public class SmsDeliveryService : DomainService, ISmsDeliveryService
{
    private readonly IRepository<SmsDeliveryRecord, Guid> _deliveryRepository;
    private readonly ILogger<SmsDeliveryService> _logger;

    public SmsDeliveryService(
        IRepository<SmsDeliveryRecord, Guid> deliveryRepository,
        ILogger<SmsDeliveryService> logger)
    {
        _deliveryRepository = deliveryRepository;
        _logger = logger;
    }

    public async Task<SmsDeliveryRecord> CreateAsync(SmsDeliveryRecord record)
    {
        try
        {
            await _deliveryRepository.InsertAsync(record);
            _logger.LogInformation("Created SMS delivery record for message: {MessageId}", record.MessageId);
            return record;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SMS delivery record for message: {MessageId}", record.MessageId);
            throw;
        }
    }

    public async Task<SmsDeliveryRecord> UpdateStatusAsync(Guid recordId, SmsDeliveryStatus status, string errorCode = null, string errorMessage = null)
    {
        try
        {
            var record = await _deliveryRepository.GetAsync(recordId);
            record.UpdateStatus(status, errorCode, errorMessage);
            await _deliveryRepository.UpdateAsync(record);

            _logger.LogInformation("Updated SMS delivery status for record: {RecordId} to {Status}", recordId, status);
            return record;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SMS delivery status for record: {RecordId}", recordId);
            throw;
        }
    }

    public async Task<SmsDeliveryRecord> UpdateStatusByExternalIdAsync(string externalMessageId, SmsDeliveryStatus status, string errorCode = null, string errorMessage = null)
    {
        try
        {
            var record = await GetByExternalIdAsync(externalMessageId);
            if (record == null)
            {
                _logger.LogWarning("SMS delivery record not found for external ID: {ExternalMessageId}", externalMessageId);
                return null;
            }

            record.UpdateStatus(status, errorCode, errorMessage);
            await _deliveryRepository.UpdateAsync(record);

            _logger.LogInformation("Updated SMS delivery status for external ID: {ExternalMessageId} to {Status}", externalMessageId, status);
            return record;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SMS delivery status for external ID: {ExternalMessageId}", externalMessageId);
            throw;
        }
    }

    public async Task<SmsDeliveryRecord> GetAsync(Guid id)
    {
        return await _deliveryRepository.GetAsync(id);
    }

    public async Task<SmsDeliveryRecord> GetByExternalIdAsync(string externalMessageId)
    {
        return await _deliveryRepository.FirstOrDefaultAsync(x => x.ExternalMessageId == externalMessageId);
    }

    public async Task<List<SmsDeliveryRecord>> GetByMessageIdAsync(Guid messageId)
    {
        return await _deliveryRepository.GetListAsync(x => x.MessageId == messageId);
    }

    public async Task<List<SmsDeliveryRecord>> GetByPhoneNumberAsync(string phoneNumber, int limit = 50)
    {
        return await _deliveryRepository.GetListAsync(
            x => x.ToPhoneNumber == phoneNumber,
            maxResultCount: limit);
    }

    public async Task<List<SmsDeliveryRecord>> GetByStatusAsync(SmsDeliveryStatus status)
    {
        return await _deliveryRepository.GetListAsync(x => x.Status == status);
    }

    public async Task<List<SmsDeliveryRecord>> GetPendingRetriesAsync()
    {
        var now = DateTime.UtcNow;
        return await _deliveryRepository.GetListAsync(x =>
            x.TrackingActive &&
            x.NextRetryAt.HasValue &&
            x.NextRetryAt <= now);
    }

    public async Task<List<SmsDeliveryRecord>> GetActiveTrackingAsync()
    {
        return await _deliveryRepository.GetListAsync(x => x.TrackingActive);
    }

    public async Task<List<SmsDeliveryRecord>> SearchAsync(SmsDeliverySearchParameters parameters)
    {
        var queryable = await _deliveryRepository.GetQueryableAsync();

        var query = queryable.AsQueryable();

        // Apply filters
        if (parameters.MessageId.HasValue)
            query = query.Where(x => x.MessageId == parameters.MessageId.Value);

        if (!string.IsNullOrWhiteSpace(parameters.ExternalMessageId))
            query = query.Where(x => x.ExternalMessageId == parameters.ExternalMessageId);

        if (!string.IsNullOrWhiteSpace(parameters.ToPhoneNumber))
            query = query.Where(x => x.ToPhoneNumber.Contains(parameters.ToPhoneNumber));

        if (!string.IsNullOrWhiteSpace(parameters.FromPhoneNumber))
            query = query.Where(x => x.FromPhoneNumber.Contains(parameters.FromPhoneNumber));

        if (parameters.Status.HasValue)
            query = query.Where(x => x.Status == parameters.Status.Value);

        if (parameters.Provider.HasValue)
            query = query.Where(x => x.Provider == parameters.Provider.Value);

        if (parameters.Category.HasValue)
            query = query.Where(x => x.Category == parameters.Category.Value);

        if (parameters.StartDate.HasValue)
            query = query.Where(x => x.CreationTime >= parameters.StartDate.Value);

        if (parameters.EndDate.HasValue)
            query = query.Where(x => x.CreationTime <= parameters.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(parameters.CampaignId))
            query = query.Where(x => x.CampaignId == parameters.CampaignId);

        if (!string.IsNullOrWhiteSpace(parameters.Tags))
            query = query.Where(x => x.Tags.Contains(parameters.Tags));

        if (parameters.DeliveredSuccessfully.HasValue)
        {
            var successfulStatuses = new[] { SmsDeliveryStatus.Delivered, SmsDeliveryStatus.Read };
            query = parameters.DeliveredSuccessfully.Value
                ? query.Where(x => successfulStatuses.Contains(x.Status))
                : query.Where(x => !successfulStatuses.Contains(x.Status));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(parameters.Sorting))
        {
            // Simple sorting implementation
            var parts = parameters.Sorting.Split(' ');
            var field = parts[0];
            var descending = parts.Length > 1 && parts[1].ToLower() == "desc";

            query = field.ToLower() switch
            {
                "creationtime" => descending ? query.OrderByDescending(x => x.CreationTime) : query.OrderBy(x => x.CreationTime),
                "status" => descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
                "to" => descending ? query.OrderByDescending(x => x.ToPhoneNumber) : query.OrderBy(x => x.ToPhoneNumber),
                "cost" => descending ? query.OrderByDescending(x => x.Cost) : query.OrderBy(x => x.Cost),
                _ => query.OrderByDescending(x => x.CreationTime)
            };
        }
        else
        {
            query = query.OrderByDescending(x => x.CreationTime);
        }

        // Apply pagination
        return query
            .Skip(parameters.SkipCount)
            .Take(parameters.MaxResultCount)
            .ToList();
    }

    public async Task<SmsDeliveryStatistics> GetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var queryable = await _deliveryRepository.GetQueryableAsync();
        var query = queryable.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.CreationTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.CreationTime <= endDate.Value);

        var records = query.ToList();

        return CalculateStatistics(records);
    }

    public async Task<Dictionary<SmsCategory, SmsDeliveryStatistics>> GetStatisticsByCategoryAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var queryable = await _deliveryRepository.GetQueryableAsync();
        var query = queryable.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.CreationTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.CreationTime <= endDate.Value);

        var records = query.ToList();
        var groupedRecords = records.GroupBy(x => x.Category);

        return groupedRecords.ToDictionary(
            g => g.Key,
            g => CalculateStatistics(g.ToList())
        );
    }

    public async Task<Dictionary<SmsProvider, SmsDeliveryStatistics>> GetStatisticsByProviderAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var queryable = await _deliveryRepository.GetQueryableAsync();
        var query = queryable.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.CreationTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.CreationTime <= endDate.Value);

        var records = query.ToList();
        var groupedRecords = records.GroupBy(x => x.Provider);

        return groupedRecords.ToDictionary(
            g => g.Key,
            g => CalculateStatistics(g.ToList())
        );
    }

    public async Task<int> ProcessProviderUpdatesAsync(SmsProvider provider, Dictionary<string, SmsDeliveryStatus> updates)
    {
        int updatedCount = 0;

        try
        {
            foreach (var update in updates)
            {
                var record = await GetByExternalIdAsync(update.Key);
                if (record != null)
                {
                    record.UpdateStatus(update.Value);
                    await _deliveryRepository.UpdateAsync(record);
                    updatedCount++;
                }
            }

            _logger.LogInformation("Processed {Count} provider updates for {Provider}", updatedCount, provider);
            return updatedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing provider updates for {Provider}", provider);
            throw;
        }
    }

    public async Task<int> SyncWithProviderAsync(SmsProvider provider, int maxRecords = 100)
    {
        try
        {
            var activeRecords = await _deliveryRepository.GetListAsync(
                x => x.Provider == provider && x.TrackingActive,
                maxResultCount: maxRecords);

            int syncedCount = 0;

            // This would integrate with the actual provider API
            // For now, just simulate the sync
            foreach (var record in activeRecords)
            {
                record.MarkStatusCheck();
                await _deliveryRepository.UpdateAsync(record);
                syncedCount++;
            }

            _logger.LogInformation("Synced {Count} records with {Provider}", syncedCount, provider);
            return syncedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing with provider {Provider}", provider);
            throw;
        }
    }

    public async Task<int> RetryFailedMessagesAsync(int maxRetries = 10)
    {
        try
        {
            var retryRecords = await GetPendingRetriesAsync();
            var retryCount = Math.Min(retryRecords.Count, maxRetries);

            for (int i = 0; i < retryCount; i++)
            {
                var record = retryRecords[i];
                record.IncrementRetry();
                record.UpdateStatus(SmsDeliveryStatus.Queued);
                await _deliveryRepository.UpdateAsync(record);
            }

            _logger.LogInformation("Retrying {Count} failed SMS messages", retryCount);
            return retryCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrying failed messages");
            throw;
        }
    }

    public async Task<int> CancelPendingMessagesAsync(string campaignId = null)
    {
        try
        {
            var query = campaignId != null
                ? x => x.CampaignId == campaignId && (x.Status == SmsDeliveryStatus.Queued || x.Status == SmsDeliveryStatus.Sending)
                : x => x.Status == SmsDeliveryStatus.Queued || x.Status == SmsDeliveryStatus.Sending;

            var pendingRecords = await _deliveryRepository.GetListAsync(query);

            foreach (var record in pendingRecords)
            {
                record.UpdateStatus(SmsDeliveryStatus.Canceled);
                await _deliveryRepository.UpdateAsync(record);
            }

            _logger.LogInformation("Canceled {Count} pending SMS messages", pendingRecords.Count);
            return pendingRecords.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling pending messages");
            throw;
        }
    }

    public async Task<List<SmsDeliveryEvent>> GetDeliveryTimelineAsync(Guid recordId)
    {
        try
        {
            var record = await GetAsync(recordId);
            var events = new List<SmsDeliveryEvent>();

            // Create timeline events based on record history
            events.Add(new SmsDeliveryEvent
            {
                Timestamp = record.CreationTime,
                Status = SmsDeliveryStatus.Queued,
                Description = "Message queued for sending"
            });

            if (record.SentAt.HasValue)
            {
                events.Add(new SmsDeliveryEvent
                {
                    Timestamp = record.SentAt.Value,
                    Status = SmsDeliveryStatus.Sent,
                    Description = "Message sent to network"
                });
            }

            if (record.DeliveredAt.HasValue)
            {
                events.Add(new SmsDeliveryEvent
                {
                    Timestamp = record.DeliveredAt.Value,
                    Status = SmsDeliveryStatus.Delivered,
                    Description = "Message delivered to recipient"
                });
            }

            if (record.ReadAt.HasValue)
            {
                events.Add(new SmsDeliveryEvent
                {
                    Timestamp = record.ReadAt.Value,
                    Status = SmsDeliveryStatus.Read,
                    Description = "Message read by recipient"
                });
            }

            if (record.FailedAt.HasValue)
            {
                events.Add(new SmsDeliveryEvent
                {
                    Timestamp = record.FailedAt.Value,
                    Status = SmsDeliveryStatus.Failed,
                    ErrorCode = record.ErrorCode,
                    ErrorMessage = record.ErrorMessage,
                    Description = "Delivery failed"
                });
            }

            return events.OrderBy(e => e.Timestamp).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting delivery timeline for record: {RecordId}", recordId);
            throw;
        }
    }

    public async Task<List<DailyDeliveryStats>> GetDailyStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var queryable = await _deliveryRepository.GetQueryableAsync();
            var records = queryable
                .AsQueryable()
                .Where(x => x.CreationTime >= startDate && x.CreationTime <= endDate)
                .ToList();

            return records
                .GroupBy(x => x.CreationTime.Date)
                .Select(g => new DailyDeliveryStats
                {
                    Date = g.Key,
                    TotalMessages = g.Count(),
                    DeliveredMessages = g.Count(x => x.Status == SmsDeliveryStatus.Delivered || x.Status == SmsDeliveryStatus.Read),
                    FailedMessages = g.Count(x => x.Status == SmsDeliveryStatus.Failed || x.Status == SmsDeliveryStatus.Undelivered),
                    TotalCost = g.Sum(x => x.Cost),
                    DeliveryRate = g.Count() > 0 ? (double)g.Count(x => x.Status == SmsDeliveryStatus.Delivered || x.Status == SmsDeliveryStatus.Read) / g.Count() * 100 : 0
                })
                .OrderBy(x => x.Date)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting daily statistics");
            throw;
        }
    }

    public async Task<List<TopDestinationStats>> GetTopDestinationsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var queryable = await _deliveryRepository.GetQueryableAsync();
            var query = queryable.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.CreationTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.CreationTime <= endDate.Value);

            var records = query.ToList();

            return records
                .Where(x => !string.IsNullOrWhiteSpace(x.RecipientCountry))
                .GroupBy(x => x.RecipientCountry)
                .Select(g => new TopDestinationStats
                {
                    CountryCode = g.Key,
                    CountryName = GetCountryName(g.Key),
                    MessageCount = g.Count(),
                    DeliveredCount = g.Count(x => x.Status == SmsDeliveryStatus.Delivered || x.Status == SmsDeliveryStatus.Read),
                    TotalCost = g.Sum(x => x.Cost),
                    DeliveryRate = g.Count() > 0 ? (double)g.Count(x => x.Status == SmsDeliveryStatus.Delivered || x.Status == SmsDeliveryStatus.Read) / g.Count() * 100 : 0
                })
                .OrderByDescending(x => x.MessageCount)
                .Take(limit)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top destinations");
            throw;
        }
    }

    public async Task<int> CleanupOldRecordsAsync(int daysToKeep = 90)
    {
        try
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
            var oldRecords = await _deliveryRepository.GetListAsync(x =>
                x.CreationTime < cutoffDate &&
                !x.TrackingActive);

            foreach (var record in oldRecords)
            {
                await _deliveryRepository.DeleteAsync(record);
            }

            _logger.LogInformation("Cleaned up {Count} old SMS delivery records", oldRecords.Count);
            return oldRecords.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old records");
            throw;
        }
    }

    public async Task<string> ExportToCsvAsync(SmsDeliverySearchParameters parameters)
    {
        try
        {
            var records = await SearchAsync(parameters);
            var csv = new StringBuilder();

            // Header
            csv.AppendLine("ID,MessageId,ExternalMessageId,ToPhoneNumber,FromPhoneNumber,Status,Provider,Category,CreationTime,SentAt,DeliveredAt,Cost,Segments,RetryCount,ErrorMessage");

            // Data
            foreach (var record in records)
            {
                csv.AppendLine($"{record.Id},{record.MessageId},{record.ExternalMessageId},{record.ToPhoneNumber},{record.FromPhoneNumber},{record.Status},{record.Provider},{record.Category},{record.CreationTime:yyyy-MM-dd HH:mm:ss},{record.SentAt:yyyy-MM-dd HH:mm:ss},{record.DeliveredAt:yyyy-MM-dd HH:mm:ss},{record.Cost},{record.Segments},{record.RetryCount},\"{record.ErrorMessage}\"");
            }

            return csv.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting delivery records to CSV");
            throw;
        }
    }

    public async Task<SmsDeliveryReport> GenerateReportAsync(ReportParameters parameters)
    {
        try
        {
            var report = new SmsDeliveryReport
            {
                ReportType = parameters.ReportType,
                GeneratedAt = DateTime.UtcNow,
                PeriodStart = parameters.StartDate,
                PeriodEnd = parameters.EndDate
            };

            // Get overall statistics
            report.OverallStatistics = await GetStatisticsAsync(parameters.StartDate, parameters.EndDate);

            // Get daily breakdown
            report.DailyBreakdown = await GetDailyStatisticsAsync(parameters.StartDate, parameters.EndDate);

            // Get top destinations
            report.TopDestinations = await GetTopDestinationsAsync(10, parameters.StartDate, parameters.EndDate);

            // Get provider breakdown
            report.ProviderBreakdown = await GetStatisticsByProviderAsync(parameters.StartDate, parameters.EndDate);

            // Get category breakdown
            report.CategoryBreakdown = await GetStatisticsByCategoryAsync(parameters.StartDate, parameters.EndDate);

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating SMS delivery report");
            throw;
        }
    }

    private SmsDeliveryStatistics CalculateStatistics(List<SmsDeliveryRecord> records)
    {
        var totalMessages = records.Count;
        var deliveredMessages = records.Count(x => x.Status == SmsDeliveryStatus.Delivered || x.Status == SmsDeliveryStatus.Read);
        var readMessages = records.Count(x => x.Status == SmsDeliveryStatus.Read);
        var failedMessages = records.Count(x => x.Status == SmsDeliveryStatus.Failed || x.Status == SmsDeliveryStatus.Undelivered);

        var deliveryTimes = records
            .Where(x => x.SentAt.HasValue && x.DeliveredAt.HasValue)
            .Select(x => x.DeliveredAt!.Value - x.SentAt!.Value)
            .ToList();

        return new SmsDeliveryStatistics
        {
            TotalMessages = totalMessages,
            QueuedMessages = records.Count(x => x.Status == SmsDeliveryStatus.Queued),
            SentMessages = records.Count(x => x.Status == SmsDeliveryStatus.Sent),
            DeliveredMessages = records.Count(x => x.Status == SmsDeliveryStatus.Delivered),
            ReadMessages = readMessages,
            FailedMessages = records.Count(x => x.Status == SmsDeliveryStatus.Failed),
            UndeliveredMessages = records.Count(x => x.Status == SmsDeliveryStatus.Undelivered),
            RejectedMessages = records.Count(x => x.Status == SmsDeliveryStatus.Rejected),
            CanceledMessages = records.Count(x => x.Status == SmsDeliveryStatus.Canceled),
            TotalCost = records.Sum(x => x.Cost),
            AverageCost = totalMessages > 0 ? records.Sum(x => x.Cost) / totalMessages : 0,
            DeliveryRate = totalMessages > 0 ? (double)deliveredMessages / totalMessages * 100 : 0,
            ReadRate = deliveredMessages > 0 ? (double)readMessages / deliveredMessages * 100 : 0,
            FailureRate = totalMessages > 0 ? (double)failedMessages / totalMessages * 100 : 0,
            AverageDeliveryTime = deliveryTimes.Any() ? deliveryTimes.Average() : null,
            TotalSegments = records.Sum(x => x.Segments),
            TotalRetries = records.Sum(x => x.RetryCount),
            LastActivity = records.Any() ? records.Max(x => x.CreationTime) : null
        };
    }

    private string GetCountryName(string countryCode)
    {
        // Simple country name mapping - in a real app, use a proper library
        return countryCode.ToUpper() switch
        {
            "US" => "United States",
            "BR" => "Brazil",
            "GB" => "United Kingdom",
            "CA" => "Canada",
            "MX" => "Mexico",
            "ES" => "Spain",
            "DE" => "Germany",
            "FR" => "France",
            "IT" => "Italy",
            "AU" => "Australia",
            _ => countryCode
        };
    }
}
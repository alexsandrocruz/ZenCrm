using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Communication;

/// <summary>
/// SMS application service
/// </summary>
public class SmsAppService : ApplicationService, ISmsAppService
{
    private readonly ISmsService _smsService;

    public SmsAppService(ISmsService smsService)
    {
        _smsService = smsService;
    }

    /// <summary>
    /// Send SMS message
    /// </summary>
    public async Task<SmsSendResultDto> SendAsync(SendSmsRequestDto input)
    {
        try
        {
            var result = await _smsService.SendSmsAsync(
                input.PhoneNumber,
                input.Content,
                input.Category,
                input.Priority,
                input.ScheduledSendDate,
                input.SmsTemplateId,
                input.Variables,
                input.CampaignId,
                input.RelatedEntityId,
                input.RelatedEntityType,
                input.InteractionId
            );

            return new SmsSendResultDto
            {
                MessageId = Guid.Parse(result.MessageId ?? Guid.NewGuid().ToString()),
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                ExternalMessageId = result.MessageId,
                Cost = 0.05m,
                Currency = "USD",
                Segments = 1,
                SentAt = result.SentAt
            };
        }
        catch (Exception ex)
        {
            return new SmsSendResultDto
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Send bulk SMS messages
    /// </summary>
    [HttpPost("send-bulk")]
    public async Task<List<SmsSendResultDto>> SendBulkAsync(SendBulkSmsRequestDto input)
    {
        var results = new List<SmsSendResultDto>();

        try
        {
            var messageIds = await _communicationManager.SendBulkSmsAsync(
                input.PhoneNumbers,
                input.SmsTemplateId,
                input.Content,
                input.Variables,
                input.Priority,
                input.ScheduledSendDate,
                input.RelatedEntityId,
                input.RelatedEntityType,
                input.CampaignId,
                input.Category
            );

            foreach (var messageId in messageIds)
            {
                results.Add(new SmsSendResultDto
                {
                    MessageId = messageId,
                    Success = true,
                    SentAt = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error sending bulk SMS");
            results.Add(new SmsSendResultDto
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }

        return results;
    }

    /// <summary>
    /// Validate phone numbers
    /// </summary>
    [HttpPost("validate-phone-numbers")]
    public async Task<PhoneValidationResultDto> ValidatePhoneNumbersAsync([FromBody] string[] phoneNumbers)
    {
        var result = await _communicationManager.ValidatePhoneNumbersAsync(phoneNumbers);

        return new PhoneValidationResultDto
        {
            ValidNumbers = result.ValidNumbers.Select(x => new ValidPhoneNumberDto
            {
                Original = x.Original,
                Formatted = x.Formatted,
                CountryCode = x.CountryCode,
                Type = x.Type
            }).ToList(),
            InvalidNumbers = result.InvalidNumbers.Select(x => new InvalidPhoneNumberDto
            {
                Original = x.Original,
                ErrorReason = x.ErrorReason
            }).ToList()
        };
    }

    /// <summary>
    /// Calculate SMS cost
    /// </summary>
    [HttpPost("calculate-cost")]
    public async Task<SmsCostResultDto> CalculateCostAsync([FromBody] CalculateSmsCostRequestDto request)
    {
        var result = await _communicationManager.CalculateSmsCostAsync(
            request.PhoneNumber,
            request.MessageLength,
            request.CountryCode
        );

        return new SmsCostResultDto
        {
            Price = result.Price,
            Currency = result.Currency,
            Segments = result.Segments,
            CountryCode = result.CountryCode,
            IsInternational = result.IsInternational,
            PricePerSegment = result.PricePerSegment
        };
    }

    /// <summary>
    /// Get SMS delivery status
    /// </summary>
    [HttpGet("delivery-status/{externalMessageId}")]
    public async Task<SmsDeliveryStatusDto> GetDeliveryStatusAsync(string externalMessageId)
    {
        var result = await _communicationManager.GetSmsDeliveryStatusAsync(externalMessageId);

        // Convert to DTO
        return new SmsDeliveryStatusDto
        {
            MessageId = externalMessageId,
            Status = result.ToString(),
            // Note: We would need to get more details from delivery service for full status
        };
    }

    /// <summary>
    /// Get SMS delivery records for a message
    /// </summary>
    [HttpGet("delivery-records/{messageId}")]
    public async Task<List<SmsDeliveryRecordDto>> GetDeliveryRecordsAsync(Guid messageId)
    {
        var records = await _communicationManager.GetSmsDeliveryRecordsAsync(messageId);

        return records.Select(x => new SmsDeliveryRecordDto
        {
            Id = x.Id,
            MessageId = x.MessageId,
            ExternalMessageId = x.ExternalMessageId,
            ToPhoneNumber = x.ToPhoneNumber,
            FromPhoneNumber = x.FromPhoneNumber,
            Status = x.Status.ToString(),
            PreviousStatus = x.PreviousStatus?.ToString(),
            Provider = x.Provider,
            Category = x.Category,
            MessageContent = x.MessageContent,
            Segments = x.Segments,
            Cost = x.Cost,
            Currency = x.Currency,
            SentAt = x.SentAt,
            DeliveredAt = x.DeliveredAt,
            ReadAt = x.ReadAt,
            FailedAt = x.FailedAt,
            ErrorCode = x.ErrorCode,
            ErrorMessage = x.ErrorMessage,
            RetryCount = x.RetryCount,
            MaxRetryAttempts = x.MaxRetryAttempts,
            NextRetryAt = x.NextRetryAt,
            TrackingActive = x.TrackingActive,
            LastStatusCheckAt = x.LastStatusCheckAt,
            DeliveryInfo = x.DeliveryInfo,
            RecipientCountry = x.RecipientCountry,
            RecipientTimeZone = x.RecipientTimeZone,
            CampaignId = x.CampaignId,
            Tags = x.Tags,
            CreationTime = x.CreationTime
        }).ToList();
    }

    /// <summary>
    /// Get SMS delivery statistics
    /// </summary>
    [HttpGet("delivery-statistics")]
    public async Task<SmsDeliveryStatisticsDto> GetDeliveryStatisticsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var result = await _communicationManager.GetSmsDeliveryStatisticsAsync(startDate, endDate);

        return new SmsDeliveryStatisticsDto
        {
            TotalMessages = result.TotalMessages,
            QueuedMessages = result.QueuedMessages,
            SentMessages = result.SentMessages,
            DeliveredMessages = result.DeliveredMessages,
            ReadMessages = result.ReadMessages,
            FailedMessages = result.FailedMessages,
            UndeliveredMessages = result.UndeliveredMessages,
            RejectedMessages = result.RejectedMessages,
            CanceledMessages = result.CanceledMessages,
            TotalCost = result.TotalCost,
            AverageCost = result.AverageCost,
            DeliveryRate = result.DeliveryRate,
            ReadRate = result.ReadRate,
            FailureRate = result.FailureRate,
            AverageDeliveryTime = result.AverageDeliveryTime,
            TotalSegments = result.TotalSegments,
            TotalRetries = result.TotalRetries,
            LastActivity = result.LastActivity
        };
    }

    /// <summary>
    /// Sync SMS delivery status with provider
    /// </summary>
    [HttpPost("sync-delivery-status")]
    public async Task<SmsDeliverySyncResultDto> SyncDeliveryStatusAsync(SmsDeliverySyncRequestDto input)
    {
        try
        {
            var syncedCount = await _communicationManager.SyncSmsDeliveryStatusAsync(input.Provider);

            return new SmsDeliverySyncResultDto
            {
                SyncedRecords = syncedCount,
                Success = true,
                SyncTime = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            return new SmsDeliverySyncResultDto
            {
                SyncedRecords = 0,
                Success = false,
                ErrorMessage = ex.Message,
                SyncTime = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Check SMS service availability
    /// </summary>
    [HttpGet("availability")]
    public async Task<bool> IsAvailableAsync()
    {
        return await _smsService.IsAvailableAsync();
    }

    /// <summary>
    /// Format phone number
    /// </summary>
    [HttpPost("format-phone-number")]
    public async Task<string> FormatPhoneNumberAsync([FromBody] FormatPhoneNumberRequestDto request)
    {
        return await _smsService.FormatPhoneNumberAsync(request.PhoneNumber, request.CountryCode);
    }
}

/// <summary>
/// SMS app service interface
/// </summary>
public interface ISmsAppService : IApplicationService
{
    Task<SmsSendResultDto> SendAsync(SendSmsRequestDto input);
    Task<List<SmsSendResultDto>> SendBulkAsync(SendBulkSmsRequestDto input);
    Task<PhoneValidationResultDto> ValidatePhoneNumbersAsync(string[] phoneNumbers);
    Task<SmsCostResultDto> CalculateCostAsync(CalculateSmsCostRequestDto request);
    Task<SmsDeliveryStatusDto> GetDeliveryStatusAsync(string externalMessageId);
    Task<List<SmsDeliveryRecordDto>> GetDeliveryRecordsAsync(Guid messageId);
    Task<SmsDeliveryStatisticsDto> GetDeliveryStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<SmsDeliverySyncResultDto> SyncDeliveryStatusAsync(SmsDeliverySyncRequestDto input);
    Task<bool> IsAvailableAsync();
    Task<string> FormatPhoneNumberAsync(FormatPhoneNumberRequestDto request);
}

/// <summary>
/// Calculate SMS cost request DTO
/// </summary>
public class CalculateSmsCostRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public int MessageLength { get; set; }
    public string? CountryCode { get; set; }
}

/// <summary>
/// Format phone number request DTO
/// </summary>
public class FormatPhoneNumberRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
}
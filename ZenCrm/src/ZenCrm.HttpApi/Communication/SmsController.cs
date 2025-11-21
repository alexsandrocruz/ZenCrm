using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using ZenCrm.Communication;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Controllers;

/// <summary>
/// SMS controller
/// </summary>
[ApiController]
[Route("api/sms")]
public class SmsController : AbpControllerBase
{
    private readonly ISmsAppService _smsAppService;

    public SmsController(ISmsAppService smsAppService)
    {
        _smsAppService = smsAppService;
    }

    /// <summary>
    /// Send SMS message
    /// </summary>
    [HttpPost("send")]
    public async Task<SmsSendResultDto> SendAsync([FromBody] SendSmsRequestDto input)
    {
        return await _smsAppService.SendAsync(input);
    }

    /// <summary>
    /// Send bulk SMS messages
    /// </summary>
    [HttpPost("send-bulk")]
    public async Task<List<SmsSendResultDto>> SendBulkAsync([FromBody] SendBulkSmsRequestDto input)
    {
        return await _smsAppService.SendBulkAsync(input);
    }

    /// <summary>
    /// Validate phone numbers
    /// </summary>
    [HttpPost("validate-phone-numbers")]
    public async Task<PhoneValidationResultDto> ValidatePhoneNumbersAsync([FromBody] string[] phoneNumbers)
    {
        return await _smsAppService.ValidatePhoneNumbersAsync(phoneNumbers);
    }

    /// <summary>
    /// Calculate SMS cost
    /// </summary>
    [HttpPost("calculate-cost")]
    public async Task<SmsCostResultDto> CalculateCostAsync([FromBody] CalculateSmsCostRequestDto request)
    {
        return await _smsAppService.CalculateCostAsync(request);
    }

    /// <summary>
    /// Get SMS delivery status
    /// </summary>
    [HttpGet("delivery-status/{externalMessageId}")]
    public async Task<SmsDeliveryStatusDto> GetDeliveryStatusAsync(string externalMessageId)
    {
        return await _smsAppService.GetDeliveryStatusAsync(externalMessageId);
    }

    /// <summary>
    /// Get SMS delivery records for a message
    /// </summary>
    [HttpGet("delivery-records/{messageId}")]
    public async Task<List<SmsDeliveryRecordDto>> GetDeliveryRecordsAsync(Guid messageId)
    {
        return await _smsAppService.GetDeliveryRecordsAsync(messageId);
    }

    /// <summary>
    /// Get SMS delivery statistics
    /// </summary>
    [HttpGet("delivery-statistics")]
    public async Task<SmsDeliveryStatisticsDto> GetDeliveryStatisticsAsync(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        return await _smsAppService.GetDeliveryStatisticsAsync(startDate, endDate);
    }

    /// <summary>
    /// Sync SMS delivery status with provider
    /// </summary>
    [HttpPost("sync-delivery-status")]
    public async Task<SmsDeliverySyncResultDto> SyncDeliveryStatusAsync([FromBody] SmsDeliverySyncRequestDto input)
    {
        return await _smsAppService.SyncDeliveryStatusAsync(input);
    }

    /// <summary>
    /// Check SMS service availability
    /// </summary>
    [HttpGet("availability")]
    public async Task<bool> IsAvailableAsync()
    {
        return await _smsAppService.IsAvailableAsync();
    }

    /// <summary>
    /// Format phone number
    /// </summary>
    [HttpPost("format-phone-number")]
    public async Task<string> FormatPhoneNumberAsync([FromBody] FormatPhoneNumberRequestDto request)
    {
        return await _smsAppService.FormatPhoneNumberAsync(request);
    }
}
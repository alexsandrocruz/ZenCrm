using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using ZenCrm.Communication;
using ZenCrm.Communication.DTOs;

namespace ZenCrm.Communication;

/// <summary>
/// Communication application service interface
/// </summary>
public interface ICommunicationAppService : ICrudAppService<
        MessageDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateMessageDto>
{
    /// <summary>
    /// Send a new message
    /// </summary>
    Task<MessageDto> SendMessageAsync(SendMessageInput input);

    /// <summary>
    /// Send message using a template
    /// </summary>
    Task<MessageDto> SendTemplatedMessageAsync(SendTemplatedMessageInput input);

    /// <summary>
    /// Send bulk messages
    /// </summary>
    Task<BulkMessageResultDto> SendBulkMessagesAsync(BulkMessageInput input);

    /// <summary>
    /// Get delivery status of a message
    /// </summary>
    Task<MessageDeliveryInfoDto> GetDeliveryStatusAsync(Guid id);

    /// <summary>
    /// Cancel a message
    /// </summary>
    Task<bool> CancelMessageAsync(Guid id);

    /// <summary>
    /// Retry a failed message
    /// </summary>
    Task<bool> RetryMessageAsync(Guid id);

    /// <summary>
    /// Mark message as read
    /// </summary>
    Task<bool> MarkMessageAsReadAsync(Guid id);

    /// <summary>
    /// Get messages by related entity
    /// </summary>
    Task<PagedResultDto<MessageDto>> GetMessagesByEntityAsync(GetMessagesByEntityInput input);

    /// <summary>
    /// Get messages by status
    /// </summary>
    Task<PagedResultDto<MessageDto>> GetMessagesByStatusAsync(GetMessagesByStatusInput input);

    /// <summary>
    /// Get message templates
    /// </summary>
    Task<PagedResultDto<MessageTemplateDto>> GetTemplatesAsync(PagedAndSortedResultRequestDto input);

    /// <summary>
    /// Create a new message template
    /// </summary>
    Task<MessageTemplateDto> CreateTemplateAsync(CreateUpdateMessageTemplateDto input);

    /// <summary>
    /// Update a message template
    /// </summary>
    Task<MessageTemplateDto> UpdateTemplateAsync(Guid id, CreateUpdateMessageTemplateDto input);

    /// <summary>
    /// Delete a message template
    /// </summary>
    Task DeleteTemplateAsync(Guid id);

    /// <summary>
    /// Preview template with variables
    /// </summary>
    Task<TemplatePreviewDto> PreviewTemplateAsync(PreviewTemplateInput input);

    /// <summary>
    /// Get communication statistics
    /// </summary>
    Task<CommunicationStatsDto> GetStatisticsAsync(GetCommunicationStatsInput input);
}
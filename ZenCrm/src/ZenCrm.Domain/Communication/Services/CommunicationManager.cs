using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using ZenCrm.Communication.Entities;
using ZenCrm.Communication.Jobs;

namespace ZenCrm.Communication.Services;

/// <summary>
/// Main communication service implementation
/// </summary>
public class CommunicationManager : DomainService, ICommunicationManager
{
    private readonly IRepository<Message, Guid> _messageRepository;
    private readonly IRepository<MessageTemplate, Guid> _templateRepository;
    private readonly IMessageTemplateService _templateService;
    private readonly IEmailService _emailService;
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<CommunicationManager> _logger;

    public CommunicationManager(
        IRepository<Message, Guid> messageRepository,
        IRepository<MessageTemplate, Guid> templateRepository,
        IMessageTemplateService templateService,
        IEmailService emailService,
        IBackgroundJobManager backgroundJobManager,
        IGuidGenerator guidGenerator,
        ILogger<CommunicationManager> logger)
    {
        _messageRepository = messageRepository;
        _templateRepository = templateRepository;
        _templateService = templateService;
        _emailService = emailService;
        _backgroundJobManager = backgroundJobManager;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task<Guid> SendMessageAsync(
        string subject,
        string content,
        CommunicationChannel channel,
        string toAddress,
        MessageType type = MessageType.Notification,
        MessagePriority priority = MessagePriority.Normal,
        string? fromAddress = null,
        DateTime? scheduledSendDate = null,
        Guid? templateId = null,
        Dictionary<string, object>? templateVariables = null,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null,
        Guid? interactionId = null)
    {
        var message = new Message(
            _guidGenerator.Create(),
            subject,
            content,
            channel,
            toAddress,
            type,
            priority
        );

        // Set optional properties
        message.SetFromAddress(fromAddress);
        message.SetScheduledSendDate(scheduledSendDate);

        if (templateId.HasValue)
        {
            message.AssociateWithTemplate(templateId.Value,
                templateVariables != null ? JsonSerializer.Serialize(templateVariables) : null);
        }

        if (relatedEntityId.HasValue && !string.IsNullOrWhiteSpace(relatedEntityType))
        {
            message.AssociateWithEntity(relatedEntityType, relatedEntityId.Value);
        }

        if (interactionId.HasValue)
        {
            message.AssociateWithInteraction(interactionId.Value);
        }

        // Save the message
        await _messageRepository.InsertAsync(message);

        // Queue for sending
        await QueueMessageInternalAsync(message);

        _logger.LogInformation("Message {MessageId} queued for sending to {ToAddress} via {Channel}",
            message.Id, message.ToAddress, message.Channel);

        return message.Id;
    }

    public async Task<Guid> SendTemplatedMessageAsync(
        Guid templateId,
        string toAddress,
        Dictionary<string, object> variables,
        MessagePriority priority = MessagePriority.Normal,
        DateTime? scheduledSendDate = null,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null)
    {
        var template = await _templateRepository.GetAsync(templateId);

        // Render the template
        var renderResult = await _templateService.RenderTemplateAsync(templateId, variables);

        if (!renderResult.Success)
        {
            throw new InvalidOperationException($"Template rendering failed: {string.Join(", ", renderResult.ValidationErrors)}");
        }

        return await SendMessageAsync(
            renderResult.RenderedSubject,
            renderResult.RenderedContent,
            template.Channel,
            toAddress,
            template.Type,
            priority,
            scheduledSendDate: scheduledSendDate,
            templateId: templateId,
            templateVariables: variables,
            relatedEntityId: relatedEntityId,
            relatedEntityType: relatedEntityType
        );
    }

    public async Task<List<Guid>> SendBulkMessagesAsync(List<BulkMessageRequest> messages)
    {
        var messageIds = new List<Guid>();

        foreach (var request in messages)
        {
            try
            {
                var messageId = await SendMessageAsync(
                    request.Subject,
                    request.Content,
                    request.Channel,
                    request.ToAddress,
                    request.Type,
                    request.Priority,
                    request.FromAddress,
                    request.ScheduledSendDate,
                    request.TemplateId,
                    request.TemplateVariables,
                    request.RelatedEntityId,
                    request.RelatedEntityType
                );

                messageIds.Add(messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to queue bulk message to {ToAddress}: {Error}",
                    request.ToAddress, ex.Message);
                // Continue with other messages even if one fails
            }
        }

        return messageIds;
    }

    public async Task<Guid> QueueMessageAsync(
        string subject,
        string content,
        CommunicationChannel channel,
        string toAddress,
        MessageType type = MessageType.Notification,
        MessagePriority priority = MessagePriority.Normal,
        DateTime? scheduledSendDate = null)
    {
        var message = new Message(
            _guidGenerator.Create(),
            subject,
            content,
            channel,
            toAddress,
            type,
            priority
        );

        message.SetScheduledSendDate(scheduledSendDate);
        await _messageRepository.InsertAsync(message);

        await QueueMessageInternalAsync(message);

        return message.Id;
    }

    public async Task ProcessMessageAsync(Guid messageId)
    {
        var message = await _messageRepository.GetAsync(messageId);

        if (message.Status != MessageStatus.Queued)
        {
            _logger.LogWarning("Message {MessageId} is not in queued status. Current status: {Status}",
                messageId, message.Status);
            return;
        }

        message.StartProcessing();
        await _messageRepository.UpdateAsync(message);

        try
        {
            MessageDeliveryResult result;

            // Send based on channel
            switch (message.Channel)
            {
                case CommunicationChannel.Email:
                    result = await _emailService.SendAsync(message);
                    break;

                // Other channels will be implemented in future phases
                case CommunicationChannel.SMS:
                case CommunicationChannel.WhatsApp:
                case CommunicationChannel.PushNotification:
                    throw new NotImplementedException($"Channel {message.Channel} not yet implemented");

                default:
                    throw new ArgumentException($"Unsupported channel: {message.Channel}");
            }

            if (result.Success)
            {
                message.MarkAsSent(result.ExternalMessageId, result.ProviderResponse);
                _logger.LogInformation("Message {MessageId} sent successfully", messageId);
            }
            else
            {
                message.MarkAsFailed(result.ErrorMessage ?? "Unknown error");
                _logger.LogError("Message {MessageId} sending failed: {Error}",
                    messageId, result.ErrorMessage);

                // Schedule retry if possible
                if (message.CanRetry() && result.ShouldRetry)
                {
                    await ScheduleRetryAsync(message, result.NextRetryDate);
                }
            }

            await _messageRepository.UpdateAsync(message);
        }
        catch (Exception ex)
        {
            message.MarkAsFailed(ex.Message);
            await _messageRepository.UpdateAsync(message);

            _logger.LogError(ex, "Error processing message {MessageId}: {Error}",
                messageId, ex.Message);

            // Schedule retry if possible
            if (message.CanRetry())
            {
                await ScheduleRetryAsync(message);
            }
        }
    }

    public async Task<Message> GetMessageAsync(Guid messageId)
    {
        return await _messageRepository.GetAsync(messageId);
    }

    public async Task<MessageDeliveryInfo> GetDeliveryStatusAsync(Guid messageId)
    {
        var message = await _messageRepository.GetAsync(messageId);

        return new MessageDeliveryInfo
        {
            MessageId = message.Id,
            Status = message.Status,
            SentDate = message.SentDate,
            DeliveredDate = message.DeliveredDate,
            ReadDate = message.ReadDate,
            ExternalMessageId = message.ExternalMessageId,
            ErrorMessage = message.ErrorMessage,
            RetryCount = message.RetryCount,
            CanRetry = message.CanRetry(),
            IsOverdue = message.IsOverdue()
        };
    }

    public async Task<bool> CancelMessageAsync(Guid messageId)
    {
        var message = await _messageRepository.GetAsync(messageId);

        try
        {
            message.Cancel();
            await _messageRepository.UpdateAsync(message);

            _logger.LogInformation("Message {MessageId} cancelled successfully", messageId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to cancel message {MessageId}: {Error}",
                messageId, ex.Message);
            return false;
        }
    }

    public async Task<bool> RetryMessageAsync(Guid messageId)
    {
        var message = await _messageRepository.GetAsync(messageId);

        if (!message.CanRetry())
        {
            _logger.LogWarning("Message {MessageId} cannot be retried. Status: {Status}, RetryCount: {RetryCount}",
                messageId, message.Status, message.RetryCount);
            return false;
        }

        try
        {
            message.ResetForRetry();
            await _messageRepository.UpdateAsync(message);

            // Queue for processing
            await QueueMessageInternalAsync(message);

            _logger.LogInformation("Message {MessageId} queued for retry", messageId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retry message {MessageId}: {Error}",
                messageId, ex.Message);
            return false;
        }
    }

    public async Task<List<Message>> GetMessagesByRelatedEntityAsync(string entityType, Guid entityId)
    {
        return await _messageRepository.GetListAsync(m =>
            m.RelatedEntityType == entityType && m.RelatedEntityId == entityId);
    }

    public async Task<List<Message>> GetMessagesByInteractionAsync(Guid interactionId)
    {
        return await _messageRepository.GetListAsync(m => m.InteractionId == interactionId);
    }

    public async Task<List<Message>> GetMessagesByStatusAsync(MessageStatus status, int maxCount = 100)
    {
        return await _messageRepository.GetListAsync(m => m.Status == status);
    }

    public async Task<List<Message>> GetOverdueMessagesAsync()
    {
        var now = DateTime.UtcNow;
        return await _messageRepository.GetListAsync(m =>
            m.Status == MessageStatus.Queued &&
            m.ScheduledSendDate.HasValue &&
            m.ScheduledSendDate.Value < now);
    }

    public async Task<List<Message>> GetRetryableMessagesAsync()
    {
        return await _messageRepository.GetListAsync(m =>
            m.Status == MessageStatus.Failed &&
            m.RetryCount < 3); // Max retries configuration
    }

    public async Task<bool> UpdateMessageStatusFromWebhookAsync(
        string externalMessageId,
        MessageStatus newStatus,
        string? providerResponse = null)
    {
        var messages = await _messageRepository.GetListAsync(m => m.ExternalMessageId == externalMessageId);
        var message = messages.FirstOrDefault();

        if (message == null)
        {
            _logger.LogWarning("Message with external ID {ExternalMessageId} not found", externalMessageId);
            return false;
        }

        try
        {
            switch (newStatus)
            {
                case MessageStatus.Delivered:
                    message.MarkAsDelivered();
                    break;
                case MessageStatus.Read:
                    message.MarkAsRead();
                    break;
                case MessageStatus.Failed:
                    message.MarkAsFailed(providerResponse ?? "Provider reported failure");
                    break;
                default:
                    _logger.LogWarning("Unsupported status update {Status} for message {MessageId}",
                        newStatus, message.Id);
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(providerResponse))
            {
                message.ProviderResponse = providerResponse;
            }

            await _messageRepository.UpdateAsync(message);

            _logger.LogInformation("Message {MessageId} status updated to {Status}",
                message.Id, newStatus);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update status for message {MessageId}: {Error}",
                message.Id, ex.Message);
            return false;
        }
    }

    public async Task<bool> MarkMessageAsReadAsync(Guid messageId)
    {
        var message = await _messageRepository.GetAsync(messageId);

        try
        {
            message.MarkAsRead();
            await _messageRepository.UpdateAsync(message);

            _logger.LogInformation("Message {MessageId} marked as read", messageId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to mark message {MessageId} as read: {Error}",
                messageId, ex.Message);
            return false;
        }
    }

    public async Task DeleteMessageAsync(Guid messageId)
    {
        await _messageRepository.DeleteAsync(messageId);
        _logger.LogInformation("Message {MessageId} deleted", messageId);
    }

    private async Task QueueMessageInternalAsync(Message message)
    {
        message.Queue();
        await _messageRepository.UpdateAsync(message);

        // Schedule background job
        var jobArgs = new SendMessageJobArgs
        {
            MessageId = message.Id
        };

        // Schedule immediately or for specified date
        if (message.ScheduledSendDate.HasValue && message.ScheduledSendDate.Value > DateTime.UtcNow)
        {
            await _backgroundJobManager.EnqueueAsync(jobArgs, BackgroundJobPriority.High,
                delay: message.ScheduledSendDate.Value - DateTime.UtcNow);
        }
        else
        {
            await _backgroundJobManager.EnqueueAsync(jobArgs, BackgroundJobPriority.High);
        }
    }

    private async Task ScheduleRetryAsync(Message message, DateTime? retryDate = null)
    {
        var jobArgs = new SendMessageJobArgs
        {
            MessageId = message.Id
        };

        var delay = retryDate.HasValue && retryDate.Value > DateTime.UtcNow
            ? retryDate.Value - DateTime.UtcNow
            : TimeSpan.FromMinutes(5 * (message.RetryCount + 1)); // Exponential backoff

        await _backgroundJobManager.EnqueueAsync(jobArgs, BackgroundJobPriority.Normal, delay: delay);

        _logger.LogInformation("Message {MessageId} scheduled for retry in {Delay} minutes",
            message.Id, delay.TotalMinutes);
    }
}
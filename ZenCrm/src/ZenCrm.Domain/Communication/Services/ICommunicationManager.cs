using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// Main communication service interface for managing messages across all channels
/// </summary>
public interface ICommunicationManager : IDomainService
{
    /// <summary>
    /// Send a new message immediately or queue it for later sending
    /// </summary>
    Task<Guid> SendMessageAsync(
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
        Guid? interactionId = null
    );

    /// <summary>
    /// Send a message using a predefined template
    /// </summary>
    Task<Guid> SendTemplatedMessageAsync(
        Guid templateId,
        string toAddress,
        Dictionary<string, object> variables,
        MessagePriority priority = MessagePriority.Normal,
        DateTime? scheduledSendDate = null,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null
    );

    /// <summary>
    /// Send multiple messages in bulk
    /// </summary>
    Task<List<Guid>> SendBulkMessagesAsync(
        List<BulkMessageRequest> messages
    );

    /// <summary>
    /// Queue a message for sending without immediately processing it
    /// </summary>
    Task<Guid> QueueMessageAsync(
        string subject,
        string content,
        CommunicationChannel channel,
        string toAddress,
        MessageType type = MessageType.Notification,
        MessagePriority priority = MessagePriority.Normal,
        DateTime? scheduledSendDate = null
    );

    /// <summary>
    /// Process a queued message (used by background jobs)
    /// </summary>
    Task ProcessMessageAsync(Guid messageId);

    /// <summary>
    /// Get the current delivery status of a message
    /// </summary>
    Task<Message> GetMessageAsync(Guid messageId);

    /// <summary>
    /// Get delivery status information
    /// </summary>
    Task<MessageDeliveryInfo> GetDeliveryStatusAsync(Guid messageId);

    /// <summary>
    /// Cancel a message that hasn't been sent yet
    /// </summary>
    Task<bool> CancelMessageAsync(Guid messageId);

    /// <summary>
    /// Retry sending a failed message
    /// </summary>
    Task<bool> RetryMessageAsync(Guid messageId);

    /// <summary>
    /// Get messages related to a specific entity
    /// </summary>
    Task<List<Message>> GetMessagesByRelatedEntityAsync(string entityType, Guid entityId);

    /// <summary>
    /// Get messages for a specific interaction
    /// </summary>
    Task<List<Message>> GetMessagesByInteractionAsync(Guid interactionId);

    /// <summary>
    /// Get messages by status
    /// </summary>
    Task<List<Message>> GetMessagesByStatusAsync(MessageStatus status, int maxCount = 100);

    /// <summary>
    /// Get overdue queued messages
    /// </summary>
    Task<List<Message>> GetOverdueMessagesAsync();

    /// <summary>
    /// Get messages that can be retried
    /// </summary>
    Task<List<Message>> GetRetryableMessagesAsync();

    /// <summary>
    /// Update message status from external provider webhook
    /// </summary>
    Task<bool> UpdateMessageStatusFromWebhookAsync(
        string externalMessageId,
        MessageStatus newStatus,
        string? providerResponse = null
    );

    /// <summary>
    /// Mark message as read
    /// </summary>
    Task<bool> MarkMessageAsReadAsync(Guid messageId);

    /// <summary>
    /// Delete a message (soft delete through ABP audit system)
    /// </summary>
    Task DeleteMessageAsync(Guid messageId);
}

/// <summary>
/// Bulk message request for batch sending
/// </summary>
public class BulkMessageRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CommunicationChannel Channel { get; set; }
    public string ToAddress { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Notification;
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    public string? FromAddress { get; set; }
    public DateTime? ScheduledSendDate { get; set; }
    public Guid? TemplateId { get; set; }
    public Dictionary<string, object>? TemplateVariables { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
}

/// <summary>
/// Message delivery information
/// </summary>
public class MessageDeliveryInfo
{
    public Guid MessageId { get; set; }
    public MessageStatus Status { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ReadDate { get; set; }
    public string? ExternalMessageId { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public bool CanRetry { get; set; }
    public bool IsOverdue { get; set; }
}
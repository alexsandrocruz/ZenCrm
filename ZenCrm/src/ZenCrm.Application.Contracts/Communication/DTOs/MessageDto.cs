using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using ZenCrm.Communication;

namespace ZenCrm.Communication.DTOs;

/// <summary>
/// Message DTO
/// </summary>
public class MessageDto : FullAuditedEntityDto<Guid>
{
    /// <summary>
    /// Subject of the message
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Content/body of the message
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Communication channel used
    /// </summary>
    public CommunicationChannel Channel { get; set; }

    /// <summary>
    /// Type of the message
    /// </summary>
    public MessageType Type { get; set; }

    /// <summary>
    /// Current status of the message
    /// </summary>
    public MessageStatus Status { get; set; }

    /// <summary>
    /// Processing priority
    /// </summary>
    public MessagePriority Priority { get; set; }

    /// <summary>
    /// Scheduled date for sending
    /// </summary>
    public DateTime? ScheduledSendDate { get; set; }

    /// <summary>
    /// When the message was actually sent
    /// </summary>
    public DateTime? SentDate { get; set; }

    /// <summary>
    /// When the message was delivered
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// When the message was read
    /// </summary>
    public DateTime? ReadDate { get; set; }

    /// <summary>
    /// Recipient address (email, phone, etc.)
    /// </summary>
    public string ToAddress { get; set; } = string.Empty;

    /// <summary>
    /// Sender address
    /// </summary>
    public string? FromAddress { get; set; }

    /// <summary>
    /// CC recipients for email
    /// </summary>
    public string? CcAddress { get; set; }

    /// <summary>
    /// BCC recipients for email
    /// </summary>
    public string? BccAddress { get; set; }

    /// <summary>
    /// External message ID from provider
    /// </summary>
    public string? ExternalMessageId { get; set; }

    /// <summary>
    /// Provider response data
    /// </summary>
    public string? ProviderResponse { get; set; }

    /// <summary>
    /// Number of retry attempts
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Error message if sending failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Related template if using templated message
    /// </summary>
    public Guid? TemplateId { get; set; }

    /// <summary>
    /// Template variables in JSON format
    /// </summary>
    public string? TemplateVariables { get; set; }

    /// <summary>
    /// Related entity ID (for linking with CRM entities)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Related entity type (Interaction, Lead, Client, etc.)
    /// </summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Related interaction if this message is part of an interaction
    /// </summary>
    public Guid? InteractionId { get; set; }

    /// <summary>
    /// Computed properties
    /// </summary>
    public bool CanRetry { get; set; }
    public bool IsOverdue { get; set; }
    public string StatusDisplayText { get; set; } = string.Empty;
    public string ChannelDisplayText { get; set; } = string.Empty;
    public string TypeDisplayText { get; set; } = string.Empty;
    public string PriorityDisplayText { get; set; } = string.Empty;

    /// <summary>
    /// Navigation properties
    /// </summary>
    public MessageTemplateDto? Template { get; set; }
}
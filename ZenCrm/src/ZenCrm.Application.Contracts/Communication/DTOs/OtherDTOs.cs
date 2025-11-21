using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using ZenCrm.Communication;

namespace ZenCrm.Communication.DTOs;

/// <summary>
/// Input for sending templated message
/// </summary>
public class SendTemplatedMessageInput
{
    /// <summary>
    /// Template ID to use
    /// </summary>
    [Required]
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Recipient address
    /// </summary>
    [Required]
    public string ToAddress { get; set; } = string.Empty;

    /// <summary>
    /// Template variables
    /// </summary>
    public Dictionary<string, object> Variables { get; set; } = new();

    /// <summary>
    /// Message priority
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;

    /// <summary>
    /// Scheduled send date
    /// </summary>
    public DateTime? ScheduledSendDate { get; set; }

    /// <summary>
    /// Related entity ID
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Related entity type
    /// </summary>
    public string? RelatedEntityType { get; set; }
}

/// <summary>
/// Input for bulk message sending
/// </summary>
public class BulkMessageInput
{
    /// <summary>
    /// List of messages to send
    /// </summary>
    [Required]
    public List<SendMessageInput> Messages { get; set; } = new();
}

/// <summary>
/// Result of bulk message operation
/// </summary>
public class BulkMessageResultDto
{
    /// <summary>
    /// Total messages requested
    /// </summary>
    public int TotalRequested { get; set; }

    /// <summary>
    /// Messages successfully queued
    /// </summary>
    public int SuccessfullyQueued { get; set; }

    /// <summary>
    /// Messages that failed
    /// </summary>
    public int Failed { get; set; }

    /// <summary>
    /// List of successfully queued message IDs
    /// </summary>
    public List<Guid> QueuedMessageIds { get; set; } = new();

    /// <summary>
    /// List of errors for failed messages
    /// </summary>
    public List<BulkMessageError> Errors { get; set; } = new();
}

/// <summary>
/// Error in bulk message operation
/// </summary>
public class BulkMessageError
{
    public string ToAddress { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

/// <summary>
/// Message delivery information
/// </summary>
public class MessageDeliveryInfoDto
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
    public string StatusDisplayText { get; set; } = string.Empty;
}

/// <summary>
/// Input for getting messages by entity
/// </summary>
public class GetMessagesByEntityInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// Entity type
    /// </summary>
    [Required]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Entity ID
    /// </summary>
    [Required]
    public Guid EntityId { get; set; }
}

/// <summary>
/// Input for getting messages by status
/// </summary>
public class GetMessagesByStatusInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// Message status
    /// </summary>
    public MessageStatus Status { get; set; }
}

/// <summary>
/// Template preview result
/// </summary>
public class TemplatePreviewDto
{
    /// <summary>
    /// Rendered subject
    /// </summary>
    public string RenderedSubject { get; set; } = string.Empty;

    /// <summary>
    /// Rendered content
    /// </summary>
    public string RenderedContent { get; set; } = string.Empty;

    /// <summary>
    /// Whether preview was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Validation errors
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new();

    /// <summary>
    /// Missing variables
    /// </summary>
    public List<string> MissingVariables { get; set; } = new();
}

/// <summary>
/// Input for template preview
/// </summary>
public class PreviewTemplateInput
{
    /// <summary>
    /// Template ID
    /// </summary>
    [Required]
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Variables to use in preview
    /// </summary>
    public Dictionary<string, object> Variables { get; set; } = new();
}

/// <summary>
/// Input for communication statistics
/// </summary>
public class GetCommunicationStatsInput
{
    /// <summary>
    /// Start date for statistics
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// End date for statistics
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Channel filter
    /// </summary>
    public CommunicationChannel? Channel { get; set; }
}

/// <summary>
/// Communication statistics
/// </summary>
public class CommunicationStatsDto
{
    /// <summary>
    /// Total messages sent
    /// </summary>
    public int TotalMessages { get; set; }

    /// <summary>
    /// Messages by status
    /// </summary>
    public Dictionary<MessageStatus, int> MessagesByStatus { get; set; } = new();

    /// <summary>
    /// Messages by channel
    /// </summary>
    public Dictionary<CommunicationChannel, int> MessagesByChannel { get; set; } = new();

    /// <summary>
    /// Messages by type
    /// </summary>
    public Dictionary<MessageType, int> MessagesByType { get; set; } = new();

    /// <summary>
    /// Average delivery time in minutes
    /// </summary>
    public double AverageDeliveryTimeMinutes { get; set; }

    /// <summary>
    /// Success rate percentage
    /// </summary>
    public double SuccessRate { get; set; }

    /// <summary>
    /// Messages sent in the period
    /// </summary>
    public int MessagesInPeriod { get; set; }
}
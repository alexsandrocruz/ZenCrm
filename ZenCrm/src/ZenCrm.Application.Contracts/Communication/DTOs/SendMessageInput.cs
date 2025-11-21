using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Communication;

namespace ZenCrm.Communication.DTOs;

/// <summary>
/// Input DTO for sending a message
/// </summary>
public class SendMessageInput
{
    /// <summary>
    /// Subject of the message
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Content/body of the message
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Communication channel used
    /// </summary>
    public CommunicationChannel Channel { get; set; }

    /// <summary>
    /// Recipient address (email, phone, etc.)
    /// </summary>
    [Required]
    [StringLength(512)]
    public string ToAddress { get; set; } = string.Empty;

    /// <summary>
    /// Type of the message
    /// </summary>
    public MessageType Type { get; set; } = MessageType.Notification;

    /// <summary>
    /// Processing priority
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;

    /// <summary>
    /// Sender address
    /// </summary>
    [StringLength(512)]
    public string? FromAddress { get; set; }

    /// <summary>
    /// CC recipients for email
    /// </summary>
    [StringLength(1000)]
    public string? CcAddress { get; set; }

    /// <summary>
    /// BCC recipients for email
    /// </summary>
    [StringLength(1000)]
    public string? BccAddress { get; set; }

    /// <summary>
    /// Scheduled date for sending
    /// </summary>
    public DateTime? ScheduledSendDate { get; set; }

    /// <summary>
    /// Related template if using templated message
    /// </summary>
    public Guid? TemplateId { get; set; }

    /// <summary>
    /// Template variables as dictionary
    /// </summary>
    public Dictionary<string, object>? TemplateVariables { get; set; }

    /// <summary>
    /// Related entity ID (for linking with CRM entities)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Related entity type (Interaction, Lead, Client, etc.)
    /// </summary>
    [StringLength(64)]
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Related interaction if this message is part of an interaction
    /// </summary>
    public Guid? InteractionId { get; set; }
}
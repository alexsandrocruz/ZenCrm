using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Communication;

namespace ZenCrm.Communication.Entities;

public class Message : FullAuditedAggregateRoot<Guid>
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
    /// Type of the message
    /// </summary>
    public MessageType Type { get; set; }

    /// <summary>
    /// Current status of the message
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Draft;

    /// <summary>
    /// Processing priority
    /// </summary>
    public MessagePriority Priority { get; set; } = MessagePriority.Normal;

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
    [Required]
    [StringLength(512)]
    public string ToAddress { get; set; } = string.Empty;

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
    /// External message ID from provider
    /// </summary>
    [StringLength(256)]
    public string? ExternalMessageId { get; set; }

    /// <summary>
    /// Provider response data
    /// </summary>
    [StringLength(2000)]
    public string? ProviderResponse { get; set; }

    /// <summary>
    /// Number of retry attempts
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Error message if sending failed
    /// </summary>
    [StringLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Related template if using templated message
    /// </summary>
    public Guid? TemplateId { get; set; }

    /// <summary>
    /// Template variables in JSON format
    /// </summary>
    [StringLength(2000)]
    public string? TemplateVariables { get; set; }

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

    protected Message()
    {
    }

    public Message(
        Guid id,
        string subject,
        string content,
        CommunicationChannel channel,
        string toAddress,
        MessageType type = MessageType.Notification,
        MessagePriority priority = MessagePriority.Normal
    ) : base(id)
    {
        SetSubject(subject);
        SetContent(content);
        Channel = channel;
        SetToAddress(toAddress);
        Type = type;
        Priority = priority;
        Status = MessageStatus.Draft;
    }

    public Message SetSubject(string subject)
    {
        Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject), maxLength: 256);
        return this;
    }

    public Message SetContent(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), maxLength: 4000);
        return this;
    }

    public Message SetToAddress(string toAddress)
    {
        toAddress = Check.NotNullOrWhiteSpace(toAddress, nameof(toAddress), maxLength: 512);

        // Basic validation based on channel
        if (Channel == CommunicationChannel.Email && !IsValidEmail(toAddress))
        {
            throw new BusinessException("Invalid email address");
        }

        if ((Channel == CommunicationChannel.SMS || Channel == CommunicationChannel.WhatsApp)
            && !IsValidPhoneNumber(toAddress))
        {
            throw new BusinessException("Invalid phone number");
        }

        ToAddress = toAddress;
        return this;
    }

    public Message SetFromAddress(string? fromAddress)
    {
        FromAddress = fromAddress?.Trim();
        return this;
    }

    public Message SetCcAddress(string? ccAddress)
    {
        CcAddress = ccAddress?.Trim();
        return this;
    }

    public Message SetBccAddress(string? bccAddress)
    {
        BccAddress = bccAddress?.Trim();
        return this;
    }

    public Message SetType(MessageType type)
    {
        Type = type;
        return this;
    }

    public Message SetPriority(MessagePriority priority)
    {
        Priority = priority;
        return this;
    }

    public Message SetScheduledSendDate(DateTime? scheduledDate)
    {
        if (scheduledDate.HasValue && scheduledDate.Value < DateTime.UtcNow)
        {
            throw new BusinessException("Scheduled send date cannot be in the past");
        }
        ScheduledSendDate = scheduledDate;
        return this;
    }

    public Message AssociateWithTemplate(Guid templateId, string? templateVariables = null)
    {
        TemplateId = templateId;
        TemplateVariables = templateVariables;
        return this;
    }

    public Message AssociateWithEntity(string entityType, Guid entityId)
    {
        RelatedEntityType = entityType;
        RelatedEntityId = entityId;
        return this;
    }

    public Message AssociateWithInteraction(Guid interactionId)
    {
        InteractionId = interactionId;
        return this;
    }

    public Message Queue()
    {
        if (Status != MessageStatus.Draft)
        {
            throw new BusinessException("Only draft messages can be queued");
        }

        Status = MessageStatus.Queued;
        return this;
    }

    public Message StartProcessing()
    {
        if (Status != MessageStatus.Queued)
        {
            throw new BusinessException("Only queued messages can be processed");
        }

        Status = MessageStatus.Processing;
        return this;
    }

    public Message MarkAsSent(string? externalId = null, string? providerResponse = null)
    {
        if (Status != MessageStatus.Processing)
        {
            throw new BusinessException("Only processing messages can be marked as sent");
        }

        Status = MessageStatus.Sent;
        SentDate = DateTime.UtcNow;
        ExternalMessageId = externalId;
        ProviderResponse = providerResponse;
        ErrorMessage = null;
        return this;
    }

    public Message MarkAsDelivered()
    {
        if (Status != MessageStatus.Sent)
        {
            throw new BusinessException("Only sent messages can be marked as delivered");
        }

        Status = MessageStatus.Delivered;
        DeliveredDate = DateTime.UtcNow;
        return this;
    }

    public Message MarkAsRead()
    {
        if (Status != MessageStatus.Delivered)
        {
            throw new BusinessException("Only delivered messages can be marked as read");
        }

        Status = MessageStatus.Read;
        ReadDate = DateTime.UtcNow;
        return this;
    }

    public Message MarkAsFailed(string errorMessage)
    {
        Status = MessageStatus.Failed;
        ErrorMessage = errorMessage;
        RetryCount++;
        return this;
    }

    public Message Cancel()
    {
        if (Status == MessageStatus.Sent || Status == MessageStatus.Delivered || Status == MessageStatus.Read)
        {
            throw new BusinessException("Cannot cancel a message that has already been sent");
        }

        Status = MessageStatus.Cancelled;
        return this;
    }

    public Message ResetForRetry()
    {
        if (Status != MessageStatus.Failed)
        {
            throw new BusinessException("Only failed messages can be reset for retry");
        }

        Status = MessageStatus.Queued;
        ErrorMessage = null;
        return this;
    }

    public bool CanRetry()
    {
        return Status == MessageStatus.Failed && RetryCount < 3; // Max retries configuration
    }

    public bool IsOverdue()
    {
        return Status == MessageStatus.Queued &&
               ScheduledSendDate.HasValue &&
               ScheduledSendDate.Value < DateTime.UtcNow;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneNumber(string phone)
    {
        // Basic phone validation - can be enhanced based on requirements
        return System.Text.RegularExpressions.Regex.IsMatch(
            phone,
            @"^\+?[\d\s\-\(\)]{10,}$"
        );
    }
}
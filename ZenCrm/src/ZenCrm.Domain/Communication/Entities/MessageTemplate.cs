using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Communication;

namespace ZenCrm.Communication.Entities;

public class MessageTemplate : FullAuditedEntity<Guid>
{
    /// <summary>
    /// Unique name for the template
    /// </summary>
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the template purpose
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Communication channel this template supports
    /// </summary>
    public CommunicationChannel Channel { get; set; }

    /// <summary>
    /// Type of messages this template creates
    /// </summary>
    public MessageType Type { get; set; }

    /// <summary>
    /// Template for the subject line
    /// </summary>
    [Required]
    [StringLength(256)]
    public string SubjectTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Template for the message body/content
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string ContentTemplate { get; set; } = string.Empty;

    /// <summary>
    /// JSON definition of template variables
    /// </summary>
    [StringLength(2000)]
    public string? VariableDefinitions { get; set; }

    /// <summary>
    /// Whether this template is active and usable
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Category for organizing templates
    /// </summary>
    [StringLength(64)]
    public string? Category { get; set; }

    /// <summary>
    /// Tags for searching and filtering
    /// </summary>
    [StringLength(256)]
    public string? Tags { get; set; }

    /// <summary>
    /// Language/culture for this template
    /// </summary>
    [StringLength(10)]
    public string? Culture { get; set; }

    protected MessageTemplate()
    {
    }

    public MessageTemplate(
        Guid id,
        string name,
        string subjectTemplate,
        string contentTemplate,
        CommunicationChannel channel,
        MessageType type = MessageType.Notification
    ) : base(id)
    {
        SetName(name);
        SetSubjectTemplate(subjectTemplate);
        SetContentTemplate(contentTemplate);
        Channel = channel;
        Type = type;
    }

    public MessageTemplate SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 128);
        return this;
    }

    public MessageTemplate SetDescription(string? description)
    {
        Description = description?.Trim();
        return this;
    }

    public MessageTemplate SetSubjectTemplate(string subjectTemplate)
    {
        SubjectTemplate = Check.NotNullOrWhiteSpace(subjectTemplate, nameof(subjectTemplate), maxLength: 256);
        return this;
    }

    public MessageTemplate SetContentTemplate(string contentTemplate)
    {
        ContentTemplate = Check.NotNullOrWhiteSpace(contentTemplate, nameof(contentTemplate), maxLength: 4000);
        return this;
    }

    public MessageTemplate SetChannel(CommunicationChannel channel)
    {
        Channel = channel;
        return this;
    }

    public MessageTemplate SetType(MessageType type)
    {
        Type = type;
        return this;
    }

    public MessageTemplate SetVariableDefinitions(string? variableDefinitions)
    {
        VariableDefinitions = variableDefinitions?.Trim();
        return this;
    }

    public MessageTemplate SetCategory(string? category)
    {
        Category = category?.Trim();
        return this;
    }

    public MessageTemplate SetTags(string? tags)
    {
        Tags = tags?.Trim();
        return this;
    }

    public MessageTemplate SetCulture(string? culture)
    {
        Culture = culture?.Trim();
        return this;
    }

    public MessageTemplate Activate()
    {
        IsActive = true;
        return this;
    }

    public MessageTemplate Deactivate()
    {
        IsActive = false;
        return this;
    }
}
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using ZenCrm.Communication;

namespace ZenCrm.Communication.DTOs;

/// <summary>
/// Message Template DTO
/// </summary>
public class MessageTemplateDto : FullAuditedEntityDto<Guid>
{
    /// <summary>
    /// Unique name for the template
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the template purpose
    /// </summary>
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
    public string SubjectTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Template for the message body/content
    /// </summary>
    public string ContentTemplate { get; set; } = string.Empty;

    /// <summary>
    /// JSON definition of template variables
    /// </summary>
    public string? VariableDefinitions { get; set; }

    /// <summary>
    /// Whether this template is active and usable
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Category for organizing templates
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Tags for searching and filtering
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Language/culture for this template
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// Display properties
    /// </summary>
    public string ChannelDisplayText { get; set; } = string.Empty;
    public string TypeDisplayText { get; set; } = string.Empty;
    public List<string> TagList { get; set; } = new();
}
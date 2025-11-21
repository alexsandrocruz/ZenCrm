using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Communication;

namespace ZenCrm.Communication.DTOs;

/// <summary>
/// DTO for creating or updating a message template
/// </summary>
public class CreateUpdateMessageTemplateDto
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
    public MessageType Type { get; set; } = MessageType.Notification;

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
}
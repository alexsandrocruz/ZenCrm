using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// Service for managing message templates
/// </summary>
public interface IMessageTemplateService : IDomainService
{
    /// <summary>
    /// Create a new message template
    /// </summary>
    Task<MessageTemplate> CreateTemplateAsync(
        string name,
        string subjectTemplate,
        string contentTemplate,
        CommunicationChannel channel,
        MessageType type = MessageType.Notification,
        string? description = null,
        string? variableDefinitions = null,
        string? category = null
    );

    /// <summary>
    /// Update an existing template
    /// </summary>
    Task<MessageTemplate> UpdateTemplateAsync(
        Guid templateId,
        string name,
        string subjectTemplate,
        string contentTemplate,
        string? description = null,
        string? variableDefinitions = null,
        string? category = null
    );

    /// <summary>
    /// Get template by ID
    /// </summary>
    Task<MessageTemplate?> GetTemplateAsync(Guid templateId);

    /// <summary>
    /// Get template by name and channel
    /// </summary>
    Task<MessageTemplate?> GetTemplateByNameAsync(string name, CommunicationChannel channel);

    /// <summary>
    /// Get all templates for a channel
    /// </summary>
    Task<List<MessageTemplate>> GetTemplatesByChannelAsync(CommunicationChannel channel);

    /// <summary>
    /// Get active templates
    /// </summary>
    Task<List<MessageTemplate>> GetActiveTemplatesAsync();

    /// <summary>
    /// Delete a template
    /// </summary>
    Task DeleteTemplateAsync(Guid templateId);

    /// <summary>
    /// Activate a template
    /// </summary>
    Task<MessageTemplate> ActivateTemplateAsync(Guid templateId);

    /// <summary>
    /// Deactivate a template
    /// </summary>
    Task<MessageTemplate> DeactivateTemplateAsync(Guid templateId);

    /// <summary>
    /// Render a template with provided variables
    /// </summary>
    Task<TemplateRenderResult> RenderTemplateAsync(
        Guid templateId,
        Dictionary<string, object> variables
    );

    /// <summary>
    /// Validate template syntax and variables
    /// </summary>
    TemplateValidationResult ValidateTemplate(
        string subjectTemplate,
        string contentTemplate,
        string? variableDefinitions = null
    );
}

/// <summary>
/// Result of template rendering
/// </summary>
public class TemplateRenderResult
{
    public bool Success { get; set; }
    public string RenderedSubject { get; set; } = string.Empty;
    public string RenderedContent { get; set; } = string.Empty;
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> MissingVariables { get; set; } = new();
}

/// <summary>
/// Result of template validation
/// </summary>
public class TemplateValidationResult
{
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> UsedVariables { get; set; } = new();
    public List<string> UndefinedVariables { get; set; } = new();
}
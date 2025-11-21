using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// Message template service implementation
/// </summary>
public class MessageTemplateService : DomainService, IMessageTemplateService
{
    private readonly IRepository<MessageTemplate, Guid> _templateRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<MessageTemplateService> _logger;

    public MessageTemplateService(
        IRepository<MessageTemplate, Guid> templateRepository,
        IGuidGenerator guidGenerator,
        ILogger<MessageTemplateService> logger)
    {
        _templateRepository = templateRepository;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task<MessageTemplate> CreateTemplateAsync(
        string name,
        string subjectTemplate,
        string contentTemplate,
        CommunicationChannel channel,
        MessageType type = MessageType.Notification,
        string? description = null,
        string? variableDefinitions = null,
        string? category = null)
    {
        // Check if template name already exists for this channel
        var existingTemplate = await GetTemplateByNameAsync(name, channel);
        if (existingTemplate != null)
        {
            throw new InvalidOperationException($"Template '{name}' already exists for channel {channel}");
        }

        var template = new MessageTemplate(
            _guidGenerator.Create(),
            name,
            subjectTemplate,
            contentTemplate,
            channel,
            type
        );

        template.SetDescription(description);
        template.SetVariableDefinitions(variableDefinitions);
        template.SetCategory(category);

        await _templateRepository.InsertAsync(template);

        _logger.LogInformation("Created new message template {TemplateId} with name {TemplateName}",
            template.Id, template.Name);

        return template;
    }

    public async Task<MessageTemplate> UpdateTemplateAsync(
        Guid templateId,
        string name,
        string subjectTemplate,
        string contentTemplate,
        string? description = null,
        string? variableDefinitions = null,
        string? category = null)
    {
        var template = await _templateRepository.GetAsync(templateId);

        // Check if name conflict with other template
        var existingTemplate = await GetTemplateByNameAsync(name, template.Channel);
        if (existingTemplate != null && existingTemplate.Id != templateId)
        {
            throw new InvalidOperationException($"Template '{name}' already exists for channel {template.Channel}");
        }

        template.SetName(name);
        template.SetSubjectTemplate(subjectTemplate);
        template.SetContentTemplate(contentTemplate);
        template.SetDescription(description);
        template.SetVariableDefinitions(variableDefinitions);
        template.SetCategory(category);

        await _templateRepository.UpdateAsync(template);

        _logger.LogInformation("Updated message template {TemplateId}", templateId);

        return template;
    }

    public async Task<MessageTemplate?> GetTemplateAsync(Guid templateId)
    {
        return await _templateRepository.FirstOrDefaultAsync(x => x.Id == templateId && x.IsActive);
    }

    public async Task<MessageTemplate?> GetTemplateByNameAsync(string name, CommunicationChannel channel)
    {
        return await _templateRepository.FirstOrDefaultAsync(
            x => x.Name == name && x.Channel == channel && x.IsActive);
    }

    public async Task<List<MessageTemplate>> GetTemplatesByChannelAsync(CommunicationChannel channel)
    {
        return await _templateRepository.GetListAsync(x => x.Channel == channel && x.IsActive);
    }

    public async Task<List<MessageTemplate>> GetActiveTemplatesAsync()
    {
        return await _templateRepository.GetListAsync(x => x.IsActive);
    }

    public async Task DeleteTemplateAsync(Guid templateId)
    {
        await _templateRepository.DeleteAsync(templateId);
        _logger.LogInformation("Deleted message template {TemplateId}", templateId);
    }

    public async Task<MessageTemplate> ActivateTemplateAsync(Guid templateId)
    {
        var template = await _templateRepository.GetAsync(templateId);
        template.Activate();
        await _templateRepository.UpdateAsync(template);

        _logger.LogInformation("Activated message template {TemplateId}", templateId);
        return template;
    }

    public async Task<MessageTemplate> DeactivateTemplateAsync(Guid templateId)
    {
        var template = await _templateRepository.GetAsync(templateId);
        template.Deactivate();
        await _templateRepository.UpdateAsync(template);

        _logger.LogInformation("Deactivated message template {TemplateId}", templateId);
        return template;
    }

    public async Task<TemplateRenderResult> RenderTemplateAsync(
        Guid templateId,
        Dictionary<string, object> variables)
    {
        var template = await _templateRepository.GetAsync(templateId);

        var result = new TemplateRenderResult();

        try
        {
            // Render subject
            result.RenderedSubject = RenderTemplateString(template.SubjectTemplate, variables);

            // Render content
            result.RenderedContent = RenderTemplateString(template.ContentTemplate, variables);

            // Validate missing variables
            var requiredVariables = ExtractVariables($"{template.SubjectTemplate} {template.ContentTemplate}");
            var providedVariables = variables.Select(v => v.Key).ToHashSet();

            result.MissingVariables = requiredVariables.Where(v => !providedVariables.Contains(v)).ToList();

            result.Success = !result.MissingVariables.Any() || !IsVariableRequired(result.MissingVariables);

            if (!result.Success)
            {
                result.ValidationErrors.Add("Missing required variables: " + string.Join(", ", result.MissingVariables));
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ValidationErrors.Add($"Template rendering failed: {ex.Message}");
            _logger.LogError(ex, "Failed to render template {TemplateId}", templateId);
        }

        return result;
    }

    public TemplateValidationResult ValidateTemplate(
        string subjectTemplate,
        string contentTemplate,
        string? variableDefinitions = null)
    {
        var result = new TemplateValidationResult();

        try
        {
            // Basic syntax validation
            if (string.IsNullOrWhiteSpace(subjectTemplate))
            {
                result.ValidationErrors.Add("Subject template is required");
            }

            if (string.IsNullOrWhiteSpace(contentTemplate))
            {
                result.ValidationErrors.Add("Content template is required");
            }

            // Extract variables from templates
            var allTemplateText = $"{subjectTemplate} {contentTemplate}";
            result.UsedVariables = ExtractVariables(allTemplateText).ToList();

            // Parse variable definitions if provided
            var definedVariables = new HashSet<string>();
            if (!string.IsNullOrWhiteSpace(variableDefinitions))
            {
                try
                {
                    var definitions = JsonSerializer.Deserialize<Dictionary<string, object>>(variableDefinitions);
                    if (definitions != null)
                    {
                        definedVariables = definitions.Keys.ToHashSet();
                    }
                }
                catch (Exception ex)
                {
                    result.ValidationErrors.Add($"Invalid variable definitions JSON: {ex.Message}");
                }
            }

            // Find undefined variables
            result.UndefinedVariables = result.UsedVariables
                .Where(v => !definedVariables.Contains(v))
                .ToList();

            // Consider valid if no syntax errors
            result.IsValid = !result.ValidationErrors.Any();

            if (!result.IsValid)
            {
                _logger.LogWarning("Template validation failed: {Errors}",
                    string.Join("; ", result.ValidationErrors));
            }
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ValidationErrors.Add($"Validation error: {ex.Message}");
            _logger.LogError(ex, "Template validation exception");
        }

        return result;
    }

    private static string RenderTemplateString(string template, Dictionary<string, object> variables)
    {
        var result = template;

        // Simple variable replacement using {{variable}} syntax
        foreach (var variable in variables)
        {
            var placeholder = $"{{{{{variable.Key}}}}}";
            result = result.Replace(placeholder, variable.Value?.ToString() ?? string.Empty);
        }

        return result;
    }

    private static HashSet<string> ExtractVariables(string template)
    {
        var variables = new HashSet<string>();
        var pattern = @"\{\{([^}]+)\}\}";

        var matches = Regex.Matches(template, pattern);
        foreach (Match match in matches)
        {
            variables.Add(match.Groups[1].Value.Trim());
        }

        return variables;
    }

    private static bool IsVariableRequired(List<string> missingVariables)
    {
        // Simple heuristic: consider variables in subject as required
        // This can be enhanced with more sophisticated logic
        return missingVariables.Any(v => v.ToLowerInvariant().Contains("subject") ||
                                       v.ToLowerInvariant().Contains("name") ||
                                       v.ToLowerInvariant().Contains("title"));
    }
}
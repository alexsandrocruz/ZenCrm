using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// SMS template service implementation
/// </summary>
public class SmsTemplateService : DomainService, ISmsTemplateService
{
    private readonly IRepository<SmsTemplate, Guid> _templateRepository;
    private readonly ILogger<SmsTemplateService> _logger;

    public SmsTemplateService(
        IRepository<SmsTemplate, Guid> templateRepository,
        ILogger<SmsTemplateService> logger)
    {
        _templateRepository = templateRepository;
        _logger = logger;
    }

    public async Task<SmsTemplate> CreateAsync(SmsTemplate template)
    {
        try
        {
            // Check if template name already exists
            var existingTemplate = await _templateRepository.FirstOrDefaultAsync(x => x.Name == template.Name);
            if (existingTemplate != null)
            {
                throw new InvalidOperationException($"Template with name '{template.Name}' already exists");
            }

            await _templateRepository.InsertAsync(template);
            _logger.LogInformation("Created SMS template: {TemplateName}", template.Name);

            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SMS template: {TemplateName}", template.Name);
            throw;
        }
    }

    public async Task<SmsTemplate> UpdateAsync(Guid id, SmsTemplate template)
    {
        try
        {
            var existingTemplate = await _templateRepository.GetAsync(id);

            // Check if another template with the same name exists
            var duplicateTemplate = await _templateRepository.FirstOrDefaultAsync(x => x.Name == template.Name && x.Id != id);
            if (duplicateTemplate != null)
            {
                throw new InvalidOperationException($"Another template with name '{template.Name}' already exists");
            }

            existingTemplate.SetName(template.Name);
            existingTemplate.SetDescription(template.Description);
            existingTemplate.SetCategory(template.Category);
            existingTemplate.SetContentTemplate(template.ContentTemplate);
            existingTemplate.SetVariableDefinitions(template.VariableDefinitions);
            existingTemplate.SetCulture(template.Culture);
            existingTemplate.SetActive(template.IsActive);
            existingTemplate.SetTags(template.Tags);
            existingTemplate.SetSmsSettings(template.MaxCharactersPerSegment, template.UseUnicode, template.AutoSplitLongMessages);
            existingTemplate.SetSenderName(template.SenderName);
            existingTemplate.SetMarketingSettings(template.RequiredConsentType, template.DefaultPriority);

            await _templateRepository.UpdateAsync(existingTemplate);
            _logger.LogInformation("Updated SMS template: {TemplateName}", template.Name);

            return existingTemplate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SMS template with ID: {TemplateId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var template = await _templateRepository.GetAsync(id);
            await _templateRepository.DeleteAsync(template);
            _logger.LogInformation("Deleted SMS template: {TemplateName}", template.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SMS template with ID: {TemplateId}", id);
            throw;
        }
    }

    public async Task<SmsTemplate> GetAsync(Guid id)
    {
        return await _templateRepository.GetAsync(id);
    }

    public async Task<SmsTemplate> GetByNameAsync(string name)
    {
        return await _templateRepository.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<SmsTemplate>> GetAllAsync()
    {
        return await _templateRepository.GetListAsync();
    }

    public async Task<List<SmsTemplate>> GetByCategoryAsync(SmsCategory category)
    {
        return await _templateRepository.GetListAsync(x => x.Category == category);
    }

    public async Task<List<SmsTemplate>> GetActiveAsync()
    {
        return await _templateRepository.GetListAsync(x => x.IsActive);
    }

    public async Task<List<SmsTemplate>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<SmsTemplate>();

        var lowerQuery = query.ToLowerInvariant();
        return await _templateRepository.GetListAsync(x =>
            (x.Name.ToLower().Contains(lowerQuery) ||
             x.Description.ToLower().Contains(lowerQuery) ||
             x.ContentTemplate.ToLower().Contains(lowerQuery) ||
             x.Tags.ToLower().Contains(lowerQuery)) &&
            x.IsActive);
    }

    public async Task<string> GenerateContentAsync(Guid templateId, Dictionary<string, object> variables)
    {
        try
        {
            var template = await _templateRepository.GetAsync(templateId);

            if (!template.IsActive)
                throw new InvalidOperationException($"Template '{template.Name}' is not active");

            // Validate variables
            var missingVariables = template.ValidateVariables(variables);
            if (missingVariables.Any())
            {
                var missingList = string.Join(", ", missingVariables.Select(kv => kv.Key));
                throw new ArgumentException($"Missing required variables: {missingList}");
            }

            var content = template.GenerateContent(variables);

            // Increment usage count
            template.IncrementUsage();
            await _templateRepository.UpdateAsync(template);

            return content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating content for template ID: {TemplateId}", templateId);
            throw;
        }
    }

    public async Task<Dictionary<string, string>> ValidateVariablesAsync(Guid templateId, Dictionary<string, object> variables)
    {
        try
        {
            var template = await _templateRepository.GetAsync(templateId);
            return template.ValidateVariables(variables);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating variables for template ID: {TemplateId}", templateId);
            throw;
        }
    }

    public async Task<SmsTemplate> CloneAsync(Guid sourceTemplateId, string newName, string newDescription = null)
    {
        try
        {
            var sourceTemplate = await _templateRepository.GetAsync(sourceTemplateId);

            var clonedTemplate = new SmsTemplate(
                GuidGenerator.Create(),
                newName,
                sourceTemplate.ContentTemplate,
                sourceTemplate.Category)
            {
                Description = newDescription ?? $"Cloned from {sourceTemplate.Name}",
                VariableDefinitions = sourceTemplate.VariableDefinitions,
                Culture = sourceTemplate.Culture,
                IsActive = false, // Cloned templates start as inactive
                Tags = sourceTemplate.Tags,
                MaxCharactersPerSegment = sourceTemplate.MaxCharactersPerSegment,
                UseUnicode = sourceTemplate.UseUnicode,
                AutoSplitLongMessages = sourceTemplate.AutoSplitLongMessages,
                SenderName = sourceTemplate.SenderName,
                RequiredConsentType = sourceTemplate.RequiredConsentType,
                DefaultPriority = sourceTemplate.DefaultPriority,
                UsageCount = 0 // Reset usage count for cloned template
            };

            await _templateRepository.InsertAsync(clonedTemplate);
            _logger.LogInformation("Cloned SMS template: {SourceTemplateName} -> {NewTemplateName}", sourceTemplate.Name, newName);

            return clonedTemplate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cloning template ID: {TemplateId}", sourceTemplateId);
            throw;
        }
    }

    public async Task<SmsTemplateStatistics> GetStatisticsAsync()
    {
        try
        {
            var allTemplates = await _templateRepository.GetListAsync();

            var statistics = new SmsTemplateStatistics
            {
                TotalTemplates = allTemplates.Count,
                ActiveTemplates = allTemplates.Count(x => x.IsActive),
                InactiveTemplates = allTemplates.Count(x => !x.IsActive),
                MarketingTemplates = allTemplates.Count(x => x.Category == SmsCategory.Marketing),
                TransactionalTemplates = allTemplates.Count(x => x.Category == SmsCategory.Transactional),
                TotalUsages = allTemplates.Sum(x => x.UsageCount)
            };

            // Calculate category breakdown
            foreach (var category in Enum.GetValues<SmsCategory>())
            {
                statistics.TemplatesByCategory[category] = allTemplates.Count(x => x.Category == category);
            }

            // Calculate culture breakdown
            statistics.TemplatesByCulture = allTemplates
                .GroupBy(x => x.Culture)
                .ToDictionary(g => g.Key, g => g.Count());

            // Calculate average usage
            statistics.AverageUsagesPerTemplate = allTemplates.Count > 0
                ? (double)statistics.TotalUsages / allTemplates.Count
                : 0;

            return statistics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SMS template statistics");
            throw;
        }
    }

    public async Task<List<SmsTemplate>> GetMostUsedAsync(int count = 10)
    {
        return await _templateRepository.GetListAsync(
            orderBy: x => x.OrderByDescending(t => t.UsageCount),
            maxResultCount: count);
    }

    public async Task<List<SmsTemplate>> GetRecentlyUsedAsync(int count = 10)
    {
        return await _templateRepository.GetListAsync(
            orderBy: x => x.OrderByDescending(t => t.LastModificationTime),
            maxResultCount: count);
    }

    public async Task<List<SmsTemplate>> ImportFromJsonAsync(string jsonContent)
    {
        try
        {
            var templatesData = JsonSerializer.Deserialize<List<ImportedSmsTemplate>>(jsonContent);
            var importedTemplates = new List<SmsTemplate>();

            foreach (var templateData in templatesData)
            {
                var template = new SmsTemplate(
                    GuidGenerator.Create(),
                    templateData.Name,
                    templateData.ContentTemplate,
                    templateData.Category)
                {
                    Description = templateData.Description,
                    VariableDefinitions = templateData.VariableDefinitions,
                    Culture = templateData.Culture,
                    Tags = templateData.Tags,
                    MaxCharactersPerSegment = templateData.MaxCharactersPerSegment,
                    UseUnicode = templateData.UseUnicode,
                    AutoSplitLongMessages = templateData.AutoSplitLongMessages,
                    SenderName = templateData.SenderName
                };

                await _templateRepository.InsertAsync(template);
                importedTemplates.Add(template);
            }

            _logger.LogInformation("Imported {Count} SMS templates from JSON", importedTemplates.Count);
            return importedTemplates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing SMS templates from JSON");
            throw;
        }
    }

    public async Task<string> ExportToJsonAsync(List<Guid> templateIds = null)
    {
        try
        {
            var templates = templateIds != null
                ? await _templateRepository.GetListAsync(x => templateIds.Contains(x.Id))
                : await _templateRepository.GetListAsync();

            var exportData = templates.Select(t => new
            {
                t.Name,
                t.Description,
                Category = t.Category.ToString(),
                t.ContentTemplate,
                t.VariableDefinitions,
                t.Culture,
                t.Tags,
                t.MaxCharactersPerSegment,
                t.UseUnicode,
                t.AutoSplitLongMessages,
                t.SenderName,
                t.IsMarketingTemplate,
                RequiredConsentType = t.RequiredConsentType.ToString(),
                DefaultPriority = t.DefaultPriority.ToString()
            });

            var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            _logger.LogInformation("Exported {Count} SMS templates to JSON", templates.Count);
            return json;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting SMS templates to JSON");
            throw;
        }
    }

    /// <summary>
    /// Create default SMS templates for common use cases
    /// </summary>
    public async Task CreateDefaultTemplatesAsync()
    {
        var defaultTemplates = new List<SmsTemplate>
        {
            new(
                GuidGenerator.Create(),
                "Appointment Confirmation",
                "Hi {customerName}, your appointment at {businessName} is confirmed for {appointmentDateTime}. Location: {location}. Reply CANCEL to cancel.",
                SmsCategory.Transactional),

            new(
                GuidGenerator.Create(),
                "Appointment Reminder",
                "Reminder: Your appointment at {businessName} is tomorrow at {appointmentDateTime}. We look forward to seeing you at {location}. Reply HELP for assistance.",
                SmsCategory.Transactional),

            new(
                GuidGenerator.Create(),
                "Order Confirmation",
                "Thank you {customerName}! Your order #{orderNumber} has been confirmed and will be delivered by {deliveryDate}. Track: {trackingUrl}",
                SmsCategory.Transactional),

            new(
                GuidGenerator.Create(),
                "OTP Verification",
                "Your verification code for {serviceName} is {otpCode}. It expires in {expiryMinutes} minutes. Do not share this code.",
                SmsCategory.Authentication),

            new(
                GuidGenerator.Create(),
                "Marketing Promotion",
                "Hi {customerName}! Special offer just for you: {promotionDetails}. Use code {promoCode} at checkout. Valid until {expiryDate}. Reply STOP to unsubscribe.",
                SmsCategory.Marketing),

            new(
                GuidGenerator.Create(),
                "Payment Reminder",
                "Hi {customerName}, this is a reminder that your payment of {amount} for {invoiceNumber} is due on {dueDate}. Pay now: {paymentUrl}",
                SmsCategory.Transactional)
        };

        foreach (var template in defaultTemplates)
        {
            var existingTemplate = await _templateRepository.FirstOrDefaultAsync(x => x.Name == template.Name);
            if (existingTemplate == null)
            {
                await _templateRepository.InsertAsync(template);
                _logger.LogInformation("Created default SMS template: {TemplateName}", template.Name);
            }
        }
    }
}

/// <summary>
/// SMS template data for import/export
/// </summary>
public class ImportedSmsTemplate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public SmsCategory Category { get; set; }
    public string ContentTemplate { get; set; }
    public string VariableDefinitions { get; set; }
    public string Culture { get; set; }
    public string Tags { get; set; }
    public int MaxCharactersPerSegment { get; set; }
    public bool UseUnicode { get; set; }
    public bool AutoSplitLongMessages { get; set; }
    public string SenderName { get; set; }
}
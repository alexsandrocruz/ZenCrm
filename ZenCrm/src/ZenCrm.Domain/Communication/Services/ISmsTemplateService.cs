using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZenCrm.Communication.Entities;

namespace ZenCrm.Communication.Services;

/// <summary>
/// SMS template service interface
/// </summary>
public interface ISmsTemplateService
{
    /// <summary>
    /// Create a new SMS template
    /// </summary>
    Task<SmsTemplate> CreateAsync(SmsTemplate template);

    /// <summary>
    /// Update an existing SMS template
    /// </summary>
    Task<SmsTemplate> UpdateAsync(Guid id, SmsTemplate template);

    /// <summary>
    /// Delete an SMS template
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Get SMS template by ID
    /// </summary>
    Task<SmsTemplate> GetAsync(Guid id);

    /// <summary>
    /// Get SMS template by name
    /// </summary>
    Task<SmsTemplate> GetByNameAsync(string name);

    /// <summary>
    /// Get all SMS templates
    /// </summary>
    Task<List<SmsTemplate>> GetAllAsync();

    /// <summary>
    /// Get SMS templates by category
    /// </summary>
    Task<List<SmsTemplate>> GetByCategoryAsync(SmsCategory category);

    /// <summary>
    /// Get active SMS templates
    /// </summary>
    Task<List<SmsTemplate>> GetActiveAsync();

    /// <summary>
    /// Search SMS templates by name or description
    /// </summary>
    Task<List<SmsTemplate>> SearchAsync(string query);

    /// <summary>
    /// Generate SMS content from template with variables
    /// </summary>
    Task<string> GenerateContentAsync(Guid templateId, Dictionary<string, object> variables);

    /// <summary>
    /// Validate template variables
    /// </summary>
    Task<Dictionary<string, string>> ValidateVariablesAsync(Guid templateId, Dictionary<string, object> variables);

    /// <summary>
    /// Clone an existing template
    /// </summary>
    Task<SmsTemplate> CloneAsync(Guid sourceTemplateId, string newName, string newDescription = null);

    /// <summary>
    /// Get template usage statistics
    /// </summary>
    Task<SmsTemplateStatistics> GetStatisticsAsync();

    /// <summary>
    /// Get most used templates
    /// </summary>
    Task<List<SmsTemplate>> GetMostUsedAsync(int count = 10);

    /// <summary>
    /// Get recently used templates
    /// </summary>
    Task<List<SmsTemplate>> GetRecentlyUsedAsync(int count = 10);

    /// <summary>
    /// Import templates from JSON
    /// </summary>
    Task<List<SmsTemplate>> ImportFromJsonAsync(string jsonContent);

    /// <summary>
    /// Export templates to JSON
    /// </summary>
    Task<string> ExportToJsonAsync(List<Guid> templateIds = null);
}

/// <summary>
/// SMS template statistics
/// </summary>
public class SmsTemplateStatistics
{
    public int TotalTemplates { get; set; }
    public int ActiveTemplates { get; set; }
    public int InactiveTemplates { get; set; }
    public int MarketingTemplates { get; set; }
    public int TransactionalTemplates { get; set; }
    public Dictionary<SmsCategory, int> TemplatesByCategory { get; set; } = new();
    public Dictionary<string, int> TemplatesByCulture { get; set; } = new();
    public int TotalUsages { get; set; }
    public double AverageUsagesPerTemplate { get; set; }
}
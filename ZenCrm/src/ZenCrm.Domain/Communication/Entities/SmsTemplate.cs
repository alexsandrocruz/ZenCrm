using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ZenCrm.Communication.Entities;

/// <summary>
/// SMS-specific template with character limits and segmentation support
/// </summary>
public class SmsTemplate : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// Template name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Template description
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// SMS category
    /// </summary>
    public SmsCategory Category { get; private set; }

    /// <summary>
    /// SMS message content template
    /// </summary>
    public string ContentTemplate { get; private set; }

    /// <summary>
    /// Template variables definitions in JSON format
    /// </summary>
    public string VariableDefinitions { get; private set; }

    /// <summary>
    /// Culture code for localization
    /// </summary>
    public string Culture { get; private set; }

    /// <summary>
    /// Whether this template is active
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Tags for template organization
    /// </summary>
    public string Tags { get; private set; }

    /// <summary>
    /// Maximum characters per SMS segment (default 160 for GSM)
    /// </summary>
    public int MaxCharactersPerSegment { get; private set; }

    /// <summary>
    /// Whether to use Unicode encoding
    /// </summary>
    public bool UseUnicode { get; private set; }

    /// <summary>
    /// Whether to automatically split long messages
    /// </summary>
    public bool AutoSplitLongMessages { get; private set; }

    /// <summary>
    /// Optional sender name/ID override
    /// </summary>
    public string SenderName { get; private set; }

    /// <summary>
    /// Whether this is a template for marketing messages
    /// </summary>
    public bool IsMarketingTemplate { get; private set; }

    /// <summary>
    /// Required consent type for marketing messages
    /// </summary>
    public ConsentType RequiredConsentType { get; private set; }

    /// <summary>
    /// Default priority for messages using this template
    /// </summary>
    public MessagePriority DefaultPriority { get; private set; }

    /// <summary>
    /// Number of times this template has been used
    /// </summary>
    public int UsageCount { get; private set; }

    protected SmsTemplate()
    {
    }

    public SmsTemplate(
        Guid id,
        string name,
        string contentTemplate,
        SmsCategory category = SmsCategory.Transactional) : base(id)
    {
        SetName(name);
        SetContentTemplate(contentTemplate);
        Category = category;
        Culture = "en";
        IsActive = true;
        MaxCharactersPerSegment = 160;
        UseUnicode = false;
        AutoSplitLongMessages = true;
        IsMarketingTemplate = category == SmsCategory.Marketing;
        RequiredConsentType = IsMarketingTemplate ? ConsentType.Marketing : ConsentType.Transactional;
        DefaultPriority = MessagePriority.Normal;
        UsageCount = 0;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name cannot be empty", nameof(name));

        if (name.Length > 128)
            throw new ArgumentException("Template name cannot exceed 128 characters", nameof(name));

        Name = name.Trim();
    }

    public void SetContentTemplate(string contentTemplate)
    {
        if (string.IsNullOrWhiteSpace(contentTemplate))
            throw new ArgumentException("Content template cannot be empty", nameof(contentTemplate));

        if (contentTemplate.Length > 4000)
            throw new ArgumentException("Content template cannot exceed 4000 characters", nameof(contentTemplate));

        ContentTemplate = contentTemplate.Trim();
    }

    public void SetDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
        if (Description.Length > 500)
            Description = Description.Substring(0, 500);
    }

    public void SetCategory(SmsCategory category)
    {
        Category = category;
        IsMarketingTemplate = category == SmsCategory.Marketing;
        RequiredConsentType = IsMarketingTemplate ? ConsentType.Marketing : ConsentType.Transactional;
    }

    public void SetVariableDefinitions(string variableDefinitions)
    {
        VariableDefinitions = variableDefinitions?.Trim() ?? string.Empty;
        if (VariableDefinitions.Length > 2000)
            VariableDefinitions = VariableDefinitions.Substring(0, 2000);
    }

    public void SetCulture(string culture)
    {
        Culture = culture?.Trim() ?? "en";
        if (Culture.Length > 10)
            Culture = Culture.Substring(0, 10);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetTags(string tags)
    {
        Tags = tags?.Trim() ?? string.Empty;
        if (Tags.Length > 256)
            Tags = Tags.Substring(0, 256);
    }

    public void SetSmsSettings(int maxCharactersPerSegment = 160, bool useUnicode = false, bool autoSplitLongMessages = true)
    {
        if (maxCharactersPerSegment < 60 || maxCharactersPerSegment > 1000)
            throw new ArgumentException("Max characters per segment must be between 60 and 1000", nameof(maxCharactersPerSegment));

        MaxCharactersPerSegment = maxCharactersPerSegment;
        UseUnicode = useUnicode;
        AutoSplitLongMessages = autoSplitLongMessages;
    }

    public void SetSenderName(string senderName)
    {
        SenderName = senderName?.Trim() ?? string.Empty;
        if (SenderName.Length > 11) // SMS sender ID limit
            SenderName = SenderName.Substring(0, 11);
    }

    public void SetMarketingSettings(ConsentType requiredConsentType, MessagePriority defaultPriority = MessagePriority.Normal)
    {
        RequiredConsentType = requiredConsentType;
        DefaultPriority = defaultPriority;
    }

    public void IncrementUsage()
    {
        UsageCount++;
    }

    /// <summary>
    /// Generate SMS content by replacing variables in the template
    /// </summary>
    public string GenerateContent(Dictionary<string, object> variables)
    {
        var content = ContentTemplate;

        if (variables != null)
        {
            foreach (var variable in variables)
            {
                var placeholder = $"{{{{{variable.Key}}}}}";
                var value = variable.Value?.ToString() ?? string.Empty;
                content = content.Replace(placeholder, value);
            }
        }

        return content;
    }

    /// <summary>
    /// Calculate the number of SMS segments required for the given content
    /// </summary>
    public int CalculateSegments(string content)
    {
        if (string.IsNullOrEmpty(content))
            return 0;

        return (int)Math.Ceiling((double)content.Length / MaxCharactersPerSegment);
    }

    /// <summary>
    /// Split a long message into multiple SMS segments
    /// </summary>
    public List<string> SplitMessage(string content)
    {
        var segments = new List<string>();

        if (string.IsNullOrEmpty(content))
            return segments;

        if (content.Length <= MaxCharactersPerSegment)
        {
            segments.Add(content);
            return segments;
        }

        // Split content into segments
        for (int i = 0; i < content.Length; i += MaxCharactersPerSegment)
        {
            var segmentLength = Math.Min(MaxCharactersPerSegment, content.Length - i);
            var segment = content.Substring(i, segmentLength);

            // Add segment indicator for multi-part messages
            if (segments.Count > 0)
            {
                segment = $"({segments.Count + 1}) {segment}";
            }

            segments.Add(segment);
        }

        return segments;
    }

    /// <summary>
    /// Validate that all required variables are provided
    /// </summary>
    public Dictionary<string, string> ValidateVariables(Dictionary<string, object> providedVariables)
    {
        var missingVariables = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(VariableDefinitions))
            return missingVariables;

        try
        {
            // Parse variable definitions (simplified JSON parsing)
            var definitions = VariableDefinitions.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var definition in definitions)
            {
                var parts = definition.Trim().Split(':');
                if (parts.Length >= 1)
                {
                    var varName = parts[0].Trim().Replace("\"", "").Replace("{", "").Replace("}", "");

                    if (!providedVariables.ContainsKey(varName) || providedVariables[varName] == null)
                    {
                        var description = parts.Length > 1 ? parts[1].Trim().Replace("\"", "") : "Required variable";
                        missingVariables[varName] = description;
                    }
                }
            }
        }
        catch
        {
            // If parsing fails, assume no variables are required
        }

        return missingVariables;
    }
}

/// <summary>
/// SMS message categories
/// </summary>
public enum SmsCategory
{
    /// <summary>
    /// Transactional messages (appointments, confirmations, etc.)
    /// </summary>
    Transactional = 0,

    /// <summary>
    /// Marketing messages (promotions, offers, etc.)
    /// </summary>
    Marketing = 1,

    /// <summary>
    /// Notification messages (alerts, reminders, etc.)
    /// </summary>
    Notification = 2,

    /// <summary>
    /// Authentication messages (OTP, verification codes, etc.)
    /// </summary>
    Authentication = 3,

    /// <summary>
    /// Customer support messages
    /// </summary>
    Support = 4
}

/// <summary>
/// Consent types for SMS messaging
/// </summary>
public enum ConsentType
{
    /// <summary>
    /// No consent required (transactional)
    /// </summary>
    None = 0,

    /// <summary>
    /// Transactional consent implied from business relationship
    /// </summary>
    Transactional = 1,

    /// <summary>
    /// Explicit marketing consent required
    /// </summary>
    Marketing = 2,

    /// <summary>
    /// Explicit consent for all types of messages
    /// </summary>
    All = 3
}
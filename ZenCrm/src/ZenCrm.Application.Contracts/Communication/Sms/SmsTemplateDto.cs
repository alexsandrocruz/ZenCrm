using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Communication.Sms;

/// <summary>
/// SMS Template DTO
/// </summary>
public class SmsTemplateDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SmsCategory Category { get; set; }
    public string ContentTemplate { get; set; } = string.Empty;
    public string VariableDefinitions { get; set; } = string.Empty;
    public string Culture { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Tags { get; set; } = string.Empty;
    public int MaxCharactersPerSegment { get; set; }
    public bool UseUnicode { get; set; }
    public bool AutoSplitLongMessages { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public bool IsMarketingTemplate { get; set; }
    public ConsentType RequiredConsentType { get; set; }
    public MessagePriority DefaultPriority { get; set; }
    public int UsageCount { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}

/// <summary>
/// Create/update SMS template DTO
/// </summary>
public class CreateUpdateSmsTemplateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SmsCategory Category { get; set; }
    public string ContentTemplate { get; set; } = string.Empty;
    public string VariableDefinitions { get; set; } = string.Empty;
    public string Culture { get; set; } = "en";
    public bool IsActive { get; set; } = true;
    public string Tags { get; set; } = string.Empty;
    public int MaxCharactersPerSegment { get; set; } = 160;
    public bool UseUnicode { get; set; } = false;
    public bool AutoSplitLongMessages { get; set; } = true;
    public string SenderName { get; set; } = string.Empty;
    public ConsentType RequiredConsentType { get; set; } = ConsentType.None;
    public MessagePriority DefaultPriority { get; set; } = MessagePriority.Normal;
}

/// <summary>
/// Generate SMS content request DTO
/// </summary>
public class GenerateSmsContentRequestDto
{
    public Guid TemplateId { get; set; }
    public Dictionary<string, object> Variables { get; set; } = new();
}

/// <summary>
/// Generate SMS content result DTO
/// </summary>
public class GenerateSmsContentResultDto
{
    public string Content { get; set; } = string.Empty;
    public int Segments { get; set; }
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public decimal EstimatedCost { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// SMS template statistics DTO
/// </summary>
public class SmsTemplateStatisticsDto
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

/// <summary>
/// Import SMS templates request DTO
/// </summary>
public class ImportSmsTemplatesRequestDto
{
    public string JsonContent { get; set; } = string.Empty;
}

/// <summary>
/// Export SMS templates request DTO
/// </summary>
public class ExportSmsTemplatesRequestDto
{
    public List<Guid> TemplateIds { get; set; } = new();
}

/// <summary>
/// Clone SMS template request DTO
/// </summary>
public class CloneSmsTemplateRequestDto
{
    public Guid SourceTemplateId { get; set; }
    public string NewName { get; set; } = string.Empty;
    public string? NewDescription { get; set; }
}

/// <summary>
/// SMS template lookup parameters
/// </summary>
public class SmsTemplateLookupParameters
{
    public string? Filter { get; set; }
    public SmsCategory? Category { get; set; }
    public bool? IsActive { get; set; }
    public string? Culture { get; set; }
    public string? Tags { get; set; }
    public int MaxResultCount { get; set; } = 10;
    public int SkipCount { get; set; } = 0;
    public string? Sorting { get; set; }
}
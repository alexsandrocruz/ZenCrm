using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using ZenCrm.Communication;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Controllers;

/// <summary>
/// SMS template controller
/// </summary>
[ApiController]
[Route("api/sms-templates")]
public class SmsTemplateController : AbpControllerBase
{
    private readonly ISmsTemplateAppService _smsTemplateAppService;

    public SmsTemplateController(ISmsTemplateAppService smsTemplateAppService)
    {
        _smsTemplateAppService = smsTemplateAppService;
    }

    /// <summary>
    /// Get SMS template by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<SmsTemplateDto> GetAsync(Guid id)
    {
        return await _smsTemplateAppService.GetAsync(id);
    }

    /// <summary>
    /// Get SMS template by name
    /// </summary>
    [HttpGet("by-name/{name}")]
    public async Task<SmsTemplateDto> GetByNameAsync(string name)
    {
        return await _smsTemplateAppService.GetByNameAsync(name);
    }

    /// <summary>
    /// Get list of SMS templates
    /// </summary>
    [HttpGet]
    public async Task<PagedResultDto<SmsTemplateDto>> GetListAsync([FromQuery] SmsTemplateLookupParameters input)
    {
        return await _smsTemplateAppService.GetListAsync(input);
    }

    /// <summary>
    /// Create new SMS template
    /// </summary>
    [HttpPost]
    public async Task<SmsTemplateDto> CreateAsync([FromBody] CreateUpdateSmsTemplateDto input)
    {
        return await _smsTemplateAppService.CreateAsync(input);
    }

    /// <summary>
    /// Update existing SMS template
    /// </summary>
    [HttpPut("{id}")]
    public async Task<SmsTemplateDto> UpdateAsync(Guid id, [FromBody] CreateUpdateSmsTemplateDto input)
    {
        return await _smsTemplateAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// Delete SMS template
    /// </summary>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _smsTemplateAppService.DeleteAsync(id);
    }

    /// <summary>
    /// Generate SMS content from template
    /// </summary>
    [HttpPost("generate-content")]
    public async Task<GenerateSmsContentResultDto> GenerateContentAsync([FromBody] GenerateSmsContentRequestDto input)
    {
        return await _smsTemplateAppService.GenerateContentAsync(input);
    }

    /// <summary>
    /// Clone SMS template
    /// </summary>
    [HttpPost("clone")]
    public async Task<SmsTemplateDto> CloneAsync([FromBody] CloneSmsTemplateRequestDto input)
    {
        return await _smsTemplateAppService.CloneAsync(input);
    }

    /// <summary>
    /// Get SMS template statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<SmsTemplateStatisticsDto> GetStatisticsAsync()
    {
        return await _smsTemplateAppService.GetStatisticsAsync();
    }

    /// <summary>
    /// Get most used SMS templates
    /// </summary>
    [HttpGet("most-used")]
    public async Task<List<SmsTemplateDto>> GetMostUsedAsync([FromQuery] int count = 10)
    {
        return await _smsTemplateAppService.GetMostUsedAsync(count);
    }

    /// <summary>
    /// Get recently used SMS templates
    /// </summary>
    [HttpGet("recently-used")]
    public async Task<List<SmsTemplateDto>> GetRecentlyUsedAsync([FromQuery] int count = 10)
    {
        return await _smsTemplateAppService.GetRecentlyUsedAsync(count);
    }

    /// <summary>
    /// Import SMS templates from JSON
    /// </summary>
    [HttpPost("import")]
    public async Task<List<SmsTemplateDto>> ImportFromJsonAsync([FromBody] ImportSmsTemplatesRequestDto input)
    {
        return await _smsTemplateAppService.ImportFromJsonAsync(input);
    }

    /// <summary>
    /// Export SMS templates to JSON
    /// </summary>
    [HttpPost("export")]
    public async Task<string> ExportToJsonAsync([FromBody] ExportSmsTemplatesRequestDto input)
    {
        return await _smsTemplateAppService.ExportToJsonAsync(input);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using ZenCrm.Communication.Sms;

namespace ZenCrm.Communication;

/// <summary>
/// SMS template application service
/// </summary>
public class SmsTemplateAppService : ApplicationService, ISmsTemplateAppService
{
    // Mock data for now - implement actual service later
    private static readonly List<SmsTemplateDto> _mockTemplates = new()
    {
        new SmsTemplateDto
        {
            Id = Guid.NewGuid(),
            Name = "Bem-vindo ao ZenCRM",
            Description = "Template de boas-vindas para novos clientes",
            Category = SmsCategory.Transactional,
            ContentTemplate = "Olá {{nome}}, seja bem-vindo ao ZenCRM! Estamos felizes em ter você conosco. Acesse: {{link}}",
            VariableDefinitions = "{\"nome\":{\"description\":\"Nome do cliente\"},\"link\":{\"description\":\"Link de acesso\"}}",
            Culture = "pt-BR",
            IsActive = true,
            Tags = "boas-vindas,onboarding",
            MaxCharactersPerSegment = 160,
            UseUnicode = false,
            AutoSplitLongMessages = true,
            SenderName = "ZenCRM",
            IsMarketingTemplate = false,
            RequiredConsentType = ConsentType.None,
            DefaultPriority = MessagePriority.Normal,
            UsageCount = 0,
            CreationTime = DateTime.Now
        },
        new SmsTemplateDto
        {
            Id = Guid.NewGuid(),
            Name = "Lembrete de Reunião",
            Description = "Template para lembretes de reuniões agendadas",
            Category = SmsCategory.Notification,
            ContentTemplate = "Lembrete: Reunião agendada para {{data}} às {{hora}}. Assunto: {{assunto}}. Local: {{local}}",
            VariableDefinitions = "{\"data\":{\"description\":\"Data da reunião\"},\"hora\":{\"description\":\"Horário da reunião\"},\"assunto\":{\"description\":\"Assunto da reunião\"},\"local\":{\"description\":\"Local da reunião\"}}",
            Culture = "pt-BR",
            IsActive = true,
            Tags = "reunião,lembrete,agenda",
            MaxCharactersPerSegment = 160,
            UseUnicode = false,
            AutoSplitLongMessages = true,
            SenderName = "ZenCRM",
            IsMarketingTemplate = false,
            RequiredConsentType = ConsentType.None,
            DefaultPriority = MessagePriority.High,
            UsageCount = 15,
            CreationTime = DateTime.Now.AddDays(-5)
        },
        new SmsTemplateDto
        {
            Id = Guid.NewGuid(),
            Name = "Promoção Especial",
            Description = "Template para campanhas de marketing",
            Category = SmsCategory.Marketing,
            ContentTemplate = "🎉 OFERTA EXCLUSIVA! {{produto}} por apenas R${{preco}}. Aproveite! Código: {{codigo}}. Validade: {{validade}}",
            VariableDefinitions = "{\"produto\":{\"description\":\"Nome do produto\"},\"preco\":{\"description\":\"Preço promocional\"},\"codigo\":{\"description\":\"Código do cupom\"},\"validade\":{\"description\":\"Data de validade\"}}",
            Culture = "pt-BR",
            IsActive = true,
            Tags = "promoção,marketing,vendas",
            MaxCharactersPerSegment = 160,
            UseUnicode = false,
            AutoSplitLongMessages = true,
            SenderName = "ZenCRM",
            IsMarketingTemplate = true,
            RequiredConsentType = ConsentType.Explicit,
            DefaultPriority = MessagePriority.Normal,
            UsageCount = 250,
            CreationTime = DateTime.Now.AddDays(-30)
        },
        new SmsTemplateDto
        {
            Id = Guid.NewGuid(),
            Name = "Verificação de Conta",
            Description = "Template para código de verificação em duas etapas",
            Category = SmsCategory.Authentication,
            ContentTemplate = "Seu código de verificação ZenCRM é: {{codigo}}. Válido por {{validade}} minutos. Não compartilhe este código.",
            VariableDefinitions = "{\"codigo\":{\"description\":\"Código de verificação\"},\"validade\":{\"description\":\"Tempo de validade em minutos\"}}",
            Culture = "pt-BR",
            IsActive = true,
            Tags = "segurança,2fa,verificação",
            MaxCharactersPerSegment = 160,
            UseUnicode = false,
            AutoSplitLongMessages = false,
            SenderName = "ZenCRM",
            IsMarketingTemplate = false,
            RequiredConsentType = ConsentType.Required,
            DefaultPriority = MessagePriority.High,
            UsageCount = 89,
            CreationTime = DateTime.Now.AddDays(-10)
        }
    };

    /// <summary>
    /// Get SMS template by ID
    /// </summary>
    public async Task<SmsTemplateDto> GetAsync(Guid id)
    {
        var template = _mockTemplates.FirstOrDefault(t => t.Id == id);
        if (template == null)
        {
            throw new InvalidOperationException("Template não encontrado");
        }
        return await Task.FromResult(template);
    }

    /// <summary>
    /// Get list of SMS templates
    /// </summary>
    public async Task<List<SmsTemplateDto>> GetListAsync()
    {
        return await Task.FromResult(_mockTemplates.Where(t => t.IsActive).ToList());
    }

    
    /// <summary>
    /// Generate SMS content from template
    /// </summary>
    public async Task<GenerateSmsContentResultDto> GenerateContentAsync(GenerateSmsContentRequestDto input)
    {
        try
        {
            var template = await GetAsync(input.TemplateId);
            var content = template.ContentTemplate;

            // Substitui variáveis no template
            foreach (var variable in input.Variables)
            {
                content = content.Replace($"{{{{{variable.Key}}}}}", variable.Value?.ToString() ?? "");
            }

            var segments = (int)Math.Ceiling((double)content.Length / template.MaxCharactersPerSegment);

            return new GenerateSmsContentResultDto
            {
                Content = content,
                Segments = segments,
                IsValid = !string.IsNullOrEmpty(content),
                EstimatedCost = segments * 0.05m,
                Currency = "USD"
            };
        }
        catch (Exception ex)
        {
            return new GenerateSmsContentResultDto
            {
                IsValid = false,
                ValidationErrors = new List<string> { ex.Message }
            };
        }
    }

    /// <summary>
    /// Get SMS template statistics
    /// </summary>
    public async Task<SmsTemplateStatisticsDto> GetStatisticsAsync()
    {
        return await Task.FromResult(new SmsTemplateStatisticsDto
        {
            TotalTemplates = _mockTemplates.Count,
            ActiveTemplates = _mockTemplates.Count(t => t.IsActive),
            InactiveTemplates = _mockTemplates.Count(t => !t.IsActive),
            MarketingTemplates = _mockTemplates.Count(t => t.Category == SmsCategory.Marketing),
            TransactionalTemplates = _mockTemplates.Count(t => t.Category == SmsCategory.Transactional),
            TemplatesByCategory = _mockTemplates.GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Count()),
            TemplatesByCulture = _mockTemplates.GroupBy(t => t.Culture)
                .ToDictionary(g => g.Key, g => g.Count()),
            TotalUsages = _mockTemplates.Sum(t => t.UsageCount),
            AverageUsagesPerTemplate = _mockTemplates.Average(t => t.UsageCount)
        });
    }
}

/// <summary>
/// SMS template app service interface
/// </summary>
public interface ISmsTemplateAppService : IApplicationService
{
    Task<SmsTemplateDto> GetAsync(Guid id);
    Task<List<SmsTemplateDto>> GetListAsync();
    Task<GenerateSmsContentResultDto> GenerateContentAsync(GenerateSmsContentRequestDto input);
    Task<SmsTemplateStatisticsDto> GetStatisticsAsync();
}
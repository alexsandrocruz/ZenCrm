# Planejamento - Serviço de Mensageria ZenCrm

## Visão Geral

Este documento descreve a implementação de um serviço de mensageria para dar suporte aos workflows de comunicação no ZenCrm. O sistema integrará múltiplos canais de comunicação (Email, SMS, WhatsApp) utilizando a infraestrutura existente do ABP Framework.

## Objetivos

1. **Centralizar** o envio de mensagens através de uma interface unificada
2. **Padronizar** o workflow de comunicação para diferentes tipos de interação
3. **Integrar** com serviços externos (Email ABP, SMS, WhatsApp UazAPI)
4. **Garantir** delivery e rastreamento de mensagens
5. **Suportar** processamento assíncrono via background jobs

## Arquitetura Proposta

### 1. Módulo de Comunicação (`ZenCrm.Communication`)

Estrutura de pastas:
```
src/
├── ZenCrm.Domain.Shared/Communication/
│   ├── CommunicationChannel.cs
│   ├── MessageStatus.cs
│   ├── MessagePriority.cs
│   └── MessageType.cs
├── ZenCrm.Domain/Communication/
│   ├── Entities/
│   │   ├── Message.cs
│   │   ├── MessageTemplate.cs
│   │   ├── CommunicationProvider.cs
│   │   └── MessageDeliveryLog.cs
│   ├── Services/
│   │   ├── ICommunicationManager.cs
│   │   ├── CommunicationManager.cs
│   │   ├── IEmailService.cs
│   │   ├── ISmsService.cs
│   │   └── IWhatsAppService.cs
│   ├── Providers/
│   │   ├── AbpEmailProvider.cs
│   │   ├── UazApiWhatsAppProvider.cs
│   │   └── SmsProvider.cs
│   └── BackgroundJobs/
│       ├── SendMessageJob.cs
│       └── ProcessMessageDeliveryJob.cs
├── ZenCrm.Application.Contracts/Communication/
│   ├── ICommunicationAppService.cs
│   ├── MessageDto.cs
│   ├── SendMessageInput.cs
│   └── MessageDeliveryReportDto.cs
├── ZenCrm.Application/Communication/
│   ├── CommunicationAppService.cs
│   └── CommunicationAutoMapperProfile.cs
└── ZenCrm.EntityFrameworkCore/Communication/
    ├── MessageConfiguration.cs
    ├── MessageTemplateConfiguration.cs
    └── CommunicationProviderConfiguration.cs
```

### 2. Entidades Principais

#### Message (Entidade Central)
```csharp
public class Message : FullAuditedAggregateRoot<Guid>
{
    public string Subject { get; set; }
    public string Content { get; set; }
    public CommunicationChannel Channel { get; set; }
    public MessageType Type { get; set; }
    public MessageStatus Status { get; set; }
    public MessagePriority Priority { get; set; }
    public DateTime? ScheduledSendDate { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ReadDate { get; set; }
    public string ToAddress { get; set; }
    public string FromAddress { get; set; }
    public string CcAddress { get; set; }
    public string BccAddress { get; set; }
    public string ExternalMessageId { get; set; }
    public string ProviderResponse { get; set; }
    public int RetryCount { get; set; }
    public string ErrorMessage { get; set; }
    public Guid? TemplateId { get; set; }
    public string TemplateVariables { get; set; } // JSON
    public Guid? RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
    public Guid? InteractionId { get; set; }
    public Guid? TenantId { get; set; }
}
```

#### MessageTemplate
```csharp
public class MessageTemplate : FullAuditedEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public CommunicationChannel Channel { get; set; }
    public MessageType Type { get; set; }
    public string SubjectTemplate { get; set; }
    public string ContentTemplate { get; set; }
    public string VariableDefinitions { get; set; } // JSON
    public bool IsActive { get; set; }
    public Guid? TenantId { get; set; }
}
```

### 3. Enums Principais

```csharp
public enum CommunicationChannel
{
    Email = 1,
    SMS = 2,
    WhatsApp = 3,
    PushNotification = 4
}

public enum MessageStatus
{
    Draft = 1,
    Queued = 2,
    Processing = 3,
    Sent = 4,
    Delivered = 5,
    Read = 6,
    Failed = 7,
    Cancelled = 8
}

public enum MessageType
{
    Notification = 1,
    Marketing = 2,
    Transactional = 3,
    Alert = 4,
    Reminder = 5
}

public enum MessagePriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}
```

### 4. Interface de Serviço Unificada

```csharp
public interface ICommunicationManager
{
    Task<Guid> SendMessageAsync(SendMessageInput input);
    Task<Guid> SendTemplatedMessageAsync(Guid templateId, string toAddress, Dictionary<string, object> variables);
    Task<Guid> SendBulkMessagesAsync(List<SendMessageInput> messages);
    Task<MessageDeliveryReportDto> GetDeliveryStatusAsync(Guid messageId);
    Task CancelMessageAsync(Guid messageId);
    Task RetryFailedMessageAsync(Guid messageId);
    Task<List<MessageDto>> GetMessagesByRelatedEntityAsync(string entityType, Guid entityId);
}
```

### 5. Integração com Interações Existente

A entidade `Interaction` atual será extendida para suportar comunicação:

```csharp
// Novas propriedades na entidade Interaction
public Guid? MessageId { get; set; }  // Link para mensagem enviada
public bool CommunicationSent { get; set; }
public DateTime? CommunicationSentDate { get; set; }

// Novos métodos
public Interaction SendCommunication(CommunicationChannel channel, string content);
public Interaction MarkCommunicationDelivered();
public Interaction MarkCommunicationRead();
```

## Implementação por Canal

### 1. Email (Integração ABP Existente)

**Configuração:**
- Utilizar `AbpEmailingModule` já configurado
- Suporte a templates Razor
- Configuração por tenant

**Provider:**
```csharp
public class AbpEmailProvider : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;

    public async Task<MessageDeliveryResult> SendAsync(Message message)
    {
        // Lógica de envio usando ABP EmailSender
        // Suporte a templates e anexos
    }
}
```

### 2. SMS

**Provider Sugerido:**
- Twilio (principal)
- Vonage (alternativa)

**Configuração:**
```csharp
public class TwilioSmsProvider : ISmsService
{
    public async Task<MessageDeliveryResult> SendAsync(Message message)
    {
        // Integração com Twilio API
        // Validação de formato de telefone
        // Status tracking
    }
}
```

### 3. WhatsApp (UazAPI)

**Implementação:**
```csharp
public class UazApiWhatsAppProvider : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UazApiWhatsAppProvider> _logger;

    public async Task<MessageDeliveryResult> SendAsync(Message message)
    {
        // Integração com UazAPI (API não-oficial WhatsApp)
        // Suporte a mensagens de texto, imagem, documento
        // Validação de formato WhatsApp
        // Status tracking via webhooks
    }
}
```

**Configuração:**
```json
{
  "UazApi": {
    "BaseUrl": "https://api.uazapi.com/v1",
    "InstanceId": "your-instance-id",
    "Token": "your-api-token",
    "WebhookSecret": "your-webhook-secret"
  }
}
```

## Background Jobs Integration

### 1. SendMessageJob
```csharp
public class SendMessageJob : AsyncBackgroundJob<SendMessageJobArgs>
{
    private readonly ICommunicationManager _communicationManager;

    public override async Task ExecuteAsync(SendMessageJobArgs args)
    {
        await _communicationManager.ProcessMessageAsync(args.MessageId);
    }
}
```

### 2. ProcessMessageDeliveryJob
```csharp
public class ProcessMessageDeliveryJob : AsyncBackgroundJob<ProcessDeliveryJobArgs>
{
    public override async Task ExecuteAsync(ProcessDeliveryJobArgs args)
    {
        // Verificar status de entrega
        // Atualizar status da mensagem
        // Processar webhooks
    }
}
```

## Workflow de Comunicação

### 1. Fluxo Principal

```
1. Criação da Interação → 2. Preparação da Mensagem → 3. Enfileiramento → 4. Processamento → 5. Envio → 6. Rastreamento
```

### 2. Integração com Interações

```csharp
// Exemplo de uso em InteractionAppService
[Authorize]
public async Task<InteractionDto> CreateInteractionAsync(CreateUpdateInteractionDto input)
{
    // Criar interação
    var interaction = await CreateInteractionInternalAsync(input);

    // Se tipo de interação requer comunicação
    if (RequiresCommunication(input.Type))
    {
        await _communicationManager.QueueCommunicationAsync(interaction, input);
    }

    return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
}
```

### 3. Gatilhos Automáticos

**Reminders:**
- Interações com `RequiresReminder = true`
- Lembrete via email/SMS 24h antes

**Follow-ups:**
- Comunicação automática após interação
- Sequências de follow-up configuráveis

**Status Updates:**
- Notificações de mudança de status
- Alertas para interações atrasadas

## Configurações e Features

### 1. Configurações por Tenant

```csharp
public class CommunicationSettings
{
    public bool EmailEnabled { get; set; } = true;
    public bool SmsEnabled { get; set; } = false;
    public bool WhatsAppEnabled { get; set; } = false;
    public string DefaultFromEmail { get; set; }
    public string DefaultFromSms { get; set; }
    public int MaxRetries { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMinutes(5);
}
```

### 2. Templates de Mensagem

**Templates de Sistema:**
- Novo lead recebido
- Agendamento de reunião
- Confirmação de participação
- Follow-up pós-interação

**Templates Customizáveis:**
- Editor de templates HTML/Texto
- Variáveis dinâmicas
- Pré-visualização

### 3. Dashboards e Relatórios

**Métricas:**
- Taxa de delivery por canal
- Tempo médio de resposta
- Engagement rate
- Custo por mensagem

**Relatórios:**
- Histórico de comunicações
- Análise de performance
- Relatórios de auditoria

## Segurança e Compliance

### 1. Validações

- **Sanitização** de conteúdo para XSS
- **Validação** de endereços (email, telefone)
- **Rate limiting** por tenant/usuário
- **Blacklist** de destinatários

### 2. Privacidade

- **Criptografia** de mensagens sensíveis
- **Consentimento** explícito para marketing
- **Direito à exclusão** (GDPR)
- **Audit trail** completo

### 3. Multi-tenancy

- **Isolamento** completo de dados
- **Configurações** por tenant
- **Rate limiting** individual
- **Billing** por consumo

## Roadmap de Implementação

### Fase 1 (MVP) - 4 semanas
1. ✅ **Estrutura básica do módulo**
2. ✅ **Entidades principais (Message, MessageTemplate)**
3. ✅ **Integração com Email ABP**
4. ✅ **Background jobs básicos**
5. ✅ **Interface com Interaction existente**

### Fase 2 (SMS) - 2 semanas
1. ✅ **Integração com Twilio SMS**
2. ✅ **Validação de telefones**
3. ✅ **Status tracking**
4. ✅ **Templates SMS**

### Fase 3 (WhatsApp) - 3 semanas
1. ✅ **Integração com UazAPI**
2. ✅ **Suporte a múltiplos formatos**
3. ✅ **Webhooks para status**
4. ✅ **Validação de números WhatsApp**

### Fase 4 (Avançado) - 4 semanas
1. ✅ **Templates avançados com editor**
2. ✅ **Campanhas e envio em lote**
3. ✅ **Dashboard e analytics**
4. ✅ **Automação avançada**

## Dependências e Pacotes NuGet

```xml
<!-- Comunicação -->
<PackageReference Include="Twilio" Version="6.15.0" />
<PackageReference Include="Vonage" Version="7.7.0" />

<!-- HTML e Templates -->
<PackageReference Include="HtmlAgilityPack" Version="1.11.54" />
<PackageReference Include="RazorEngine.NetCore" Version="3.1.0" />

<!-- HTTP Client -->
<PackageReference Include="Refit" Version="7.0.0" />
<PackageReference Include="Polly" Version="7.2.4" />

<!-- Serialização -->
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

## Configuração de Ambiente

### appsettings.json
```json
{
  "Communication": {
    "Email": {
      "DefaultFrom": "noreply@zencrm.com",
      "DefaultFromName": "ZenCrm"
    },
    "Sms": {
      "Provider": "Twilio",
      "Twilio": {
        "AccountSid": "",
        "AuthToken": "",
        "FromNumber": ""
      }
    },
    "WhatsApp": {
      "Provider": "UazApi",
      "UazApi": {
        "BaseUrl": "https://api.uazapi.com/v1",
        "InstanceId": "",
        "Token": "",
        "WebhookSecret": ""
      }
    },
    "BackgroundJobs": {
      "MaxRetries": 3,
      "RetryDelay": "00:05:00",
      "MaxProcessingTime": "00:10:00"
    }
  }
}
```

## Testes

### 1. Unit Tests
- Testes de serviços de comunicação
- Validação de templates
- Lógica de negócio

### 2. Integration Tests
- Integração com providers externos
- Background jobs
- Database operations

### 3. End-to-End Tests
- Fluxo completo de comunicação
- Webhooks
- Status tracking

## Considerações de Performance

### 1. Caching
- Cache de templates frequentemente usados
- Cache de configurações de providers
- Rate limiting

### 2. Async Processing
- Enfileiramento obrigatório para envios
- Processamento em lote para campanhas
- Webhooks assíncronos

### 3. Monitoring
- Métricas de delivery
- Performance por provider
- Alertas de falha

## Conclusão

Esta implementação fornecerá uma infraestrutura robusta e escalável para comunicação no ZenCrm, mantendo compatibilidade com a arquitetura ABP existente e provendo flexibilidade para expansões futuras.

**Next Steps:**
1. Aprovação da arquitetura proposta
2. Setup do projeto e dependências
3. Implementação Fase 1 (MVP)
4. Integração com Interaction existente
5. Testes e deploy
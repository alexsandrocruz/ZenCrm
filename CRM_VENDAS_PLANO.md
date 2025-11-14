# Planejamento: CRM de Vendas de Serviços com Funil de Atendimento

## 1. Arquitetura e Estrutura do Módulo

### Novo Módulo: `zencrm.sales`
- **Localização**: `modules/zencrm.sales/`
- **Estrutura em camadas**: Domain, Application, Web, EntityFrameworkCore
- **Integração**: Aproveitar módulos existentes (catalog, finance)

### Entidades Principais do Domínio

#### Lead (Aggregate Root)
```csharp
public class Lead : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Description { get; set; }
    public LeadStatus Status { get; set; }
    public decimal Value { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? AccountId { set; get; }
    public DateTime? FollowUpDate { set; get; }
}
```

#### Account (Aggregate Root)
```csharp
public class Account : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string DocumentNumber { get; set; } // CNPJ/CPF
    public AccountType Type { get; set; }
    public string Industry { get; set; }
    public decimal AnnualRevenue { get; set; }
    public string Website { get; set; }
}
```

#### Contact (Aggregate Root)
```csharp
public class Contact : AuditedAggregateRoot<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public Guid? AccountId { set; get; }
    public bool IsPrimary { get; set; }
}
```

#### Activity (Aggregate Root)
```csharp
public class Activity : AuditedAggregateRoot<Guid>
{
    public string Subject { get; set; }
    public string Description { get; set; }
    public ActivityType Type { get; set; }
    public ActivityStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? LeadId { set; get; }
    public Guid? AccountId { set; get; }
    public Guid? ContactId { set; get; }
    public Guid OwnerUserId { set; get; }
    public string Location { get; set; }
}
```

#### SalesPipeline (Aggregate Root)
```csharp
public class SalesPipeline : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public PipelineStage CurrentStage { get; set; }
    public decimal Probability { get; set; }
    public decimal ExpectedValue { get; set; }
    public DateTime ExpectedCloseDate { get; set; }
    public Guid LeadId { set; get; }
    public Guid OwnerUserId { set; get; }
}
```

## 2. Funil de Vendas (Sales Pipeline)

### Estágios do Funil
```csharp
public enum PipelineStage
{
    Lead = 1,          // Captação inicial
    Qualified = 2,     // Lead qualificado
    Proposal = 3,      // Proposta enviada
    Negotiation = 4,   // Negociação
    ClosedWon = 5,     // Ganho
    ClosedLost = 6     // Perdido
}
```

### Features do Funil
- Arrastar e soltar entre estágios
- Probabilidade automática por estágio
- Histórico de movimentação
- Previsão de fechamento
- Relatórios de conversão

## 3. Sistema de Atividades e Comunicação

### Tipos de Atividades
```csharp
public enum ActivityType
{
    PhoneCall = 1,
    Email = 2,
    SMS = 3,
    WhatsApp = 4,
    Meeting = 5,
    Task = 6,
    Note = 7
}
```

### Features
- Agendamento de atividades
- Registro de atividades realizadas
- Templates de emails/mensagens
- Integração com sistema de agenda
- Lembretes automáticos

## 4. Agenda e Equipe

### Agenda (Calendar)
- Integração com Activities
- Visão diária/semanal/mensal
- Filtro por usuário
- Identificação visual de tipos de atividades

### Equipe (Team)
- Atribuição de leads/contas
- Metas individuais e por equipe
- Dashboard de performance
- Transferência de leads

## 5. Implementação Passo a Passo

### Fase 1: Domínio Core
1. Criar módulo `zencrm.sales`
2. Definir entidades principais (Lead, Account, Contact)
3. Implementar repositórios e validações
4. Configurar permissões

### Fase 2: Application Layer
1. Application services para CRUD básico
2. DTOs e AutoMapper configurações
3. Serviços de negócio complexos (funil, atribuições)
4. Integração com Identity para usuários

### Fase 3: Web UI
1. Pages Razor para gestão de entidades
2. Dashboard com funil visual (Kanban)
3. Formulários de atividades
4. Calendário integrado

### Fase 4: Features Avançadas
1. Sistema de notificações
2. Relatórios e analytics
3. API REST completa
4. Integração com módulos existentes

## 6. Integrações

### Módulo Catalog
- Serviços disponíveis para vendas
- Precificação e pacotes

### Módulo Finance
- Faturamento de serviços vendidos
- Contas a receber

### ABP Identity
- Usuários do sistema como vendedores
- Roles e permissões por equipe

## 7. Prioridades de Desenvolvimento

1. **MVP (Mínimo Viável)**: Leads, contas, atividades básicas
2. **Funil Visual**: Kanban board para gestão de pipeline
3. **Agenda**: Calendário integrado com atividades
4. **Relatórios**: Dashboard de vendas e performance
5. **Automação**: Regras de negócio e notificações

## 8. Estrutura de Arquivos do Módulo

```
modules/zencrm.sales/
├── src/
│   ├── ZenCrm.Sales.Domain/
│   │   ├── Entities/
│   │   │   ├── Lead.cs
│   │   │   ├── Account.cs
│   │   │   ├── Contact.cs
│   │   │   ├── Activity.cs
│   │   │   └── SalesPipeline.cs
│   │   ├── Enums/
│   │   │   ├── LeadStatus.cs
│   │   │   ├── AccountType.cs
│   │   │   ├── ActivityType.cs
│   │   │   ├── ActivityStatus.cs
│   │   │   └── PipelineStage.cs
│   │   └── ZenCrmSalesDomainModule.cs
│   ├── ZenCrm.Sales.Application/
│   │   ├── Services/
│   │   │   ├── LeadAppService.cs
│   │   │   ├── AccountAppService.cs
│   │   │   ├── ContactAppService.cs
│   │   │   ├── ActivityAppService.cs
│   │   │   └── SalesPipelineAppService.cs
│   │   ├── Contracts/
│   │   │   ├── DTOs/
│   │   │   └── Interfaces/
│   │   └── ZenCrmSalesApplicationModule.cs
│   ├── ZenCrm.Sales.Web/
│   │   ├── Pages/
│   │   │   ├── Leads/
│   │   │   ├── Accounts/
│   │   │   ├── Contacts/
│   │   │   ├── Activities/
│   │   │   └── Pipeline/
│   │   └── ZenCrmSalesWebModule.cs
│   └── ZenCrm.Sales.EntityFrameworkCore/
│       ├── Configurations/
│       └── ZenCrmSalesEntityFrameworkCoreModule.cs
└── test/
    ├── ZenCrm.Sales.Domain.Tests/
    ├── ZenCrm.Sales.Application.Tests/
    └── ZenCrm.Sales.Web.Tests/
```

## 9. Padrões ABP a Seguir

### Entidades
- Herdar de `AuditedAggregateRoot<Guid>`
- Usar convenções do ABP para configuração
- Implementar validações nos setters

### Application Services
- Implementar `ICrudAppService` para CRUD básico
- Usar `[Authorize]` com permissões específicas
- Mapear objetos com `ObjectMapper`

### Permissões
```csharp
public static class SalesPermissions
{
    public const string GroupName = "ZenCrm.Sales";

    public static class Leads
    {
        public const string Default = GroupName + ".Leads";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    // Similar para Accounts, Contacts, Activities, etc.
}
```

Este planejamento segue as melhores práticas do ABP Framework demonstradas no exemplo BookStore, com DDD, CQRS, e arquitetura modular bem definida.
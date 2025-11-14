# Planejamento: CRM de Vendas de Serviços com Funil de Atendimento
## ABP Layered + Angular Architecture

**Localização do Projeto**: `/Users/alexsandrocruz/Documents/FabioRibeiro/ZenCrm/ZenCrm/`

## 1. Arquitetura ABP Layered + Angular

### Estrutura do Projeto
```
ZenCrm/
├── src/                           # Backend .NET 9
│   ├── ZenCrm.Domain.Shared/      # Contratos e enums compartilhados
│   ├── ZenCrm.Domain/            # Entidades e regras de negócio
│   ├── ZenCrm.Application/       # Application Services e use cases
│   ├── ZenCrm.Application.Contracts/  # Interfaces e DTOs
│   ├── ZenCrm.EntityFrameworkCore/     # EF Core e migrations
│   ├── ZenCrm.HttpApi/            # Controllers API
│   ├── ZenCrm.HttpApi.Client/    # Client proxies
│   ├── ZenCrm.HttpApi.Host/       # API host (Kestrel)
│   └── ZenCrm.DbMigrator/         # Database seeding
├── angular/                       # Frontend Angular 18
│   ├── src/app/
│   │   ├── proxy/                # API clients auto-gerados
│   │   ├── core/                 # ABP core services
│   │   └── features/             # Components CRM
└── test/                         # Projetos de teste
```

### Frontend: Angular 18 com Standalone Components
- **Proxy Services**: Auto-gerados pelo ABP em `angular/src/app/proxy/`
- **Component-based Architecture**: Componentes standalone modernos
- **Type Safety**: Full TypeScript com IntelliSense
- **Performance**: Lazy loading e otimização de bundle

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

## 8. Estrutura de Arquivos (DDD Layered)

### Backend (.NET 9)
```
src/ZenCrm.Domain/                           # Entidades de negócio
├── Leads/
│   ├── Lead.cs                             # Entidade principal
│   └── LeadManager.cs                      # Domain services
├── Accounts/
│   ├── Account.cs
│   └── AccountType.cs
├── Contacts/
│   ├── Contact.cs
│   └── ContactValidator.cs
├── Activities/
│   ├── Activity.cs
│   ├── ActivityType.cs
│   └── ActivityStatus.cs
└── SalesPipeline/
    ├── SalesPipeline.cs
    ├── PipelineStage.cs
    └── SalesPipelineManager.cs

src/ZenCrm.Application.Contracts/           # Contratos API
├── Leads/
│   ├── ILeadAppService.cs                  # Interface
│   ├── LeadDto.cs                          # DTO
│   └── CreateUpdateLeadDto.cs
├── Accounts/
│   ├── IAccountAppService.cs
│   ├── AccountDto.cs
│   └── CreateUpdateAccountDto
├── Contacts/
├── Activities/
└── SalesPipeline/

src/ZenCrm.Application/                     # Application Services
├── Leads/
│   └── LeadAppService.cs                   # Implementação CRUD
├── Accounts/
│   └── AccountAppService.cs
├── Contacts/
│   └── ContactAppService.cs
├── Activities/
│   └── ActivityAppService.cs
└── SalesPipeline/
    └── SalesPipelineAppService.cs

src/ZenCrm.EntityFrameworkCore/            # Database
├── Configurations/
│   ├── LeadConfiguration.cs
│   ├── AccountConfiguration.cs
│   ├── ContactConfiguration.cs
│   ├── ActivityConfiguration.cs
│   └── SalesPipelineConfiguration.cs
└── ZenCrmDbContext.cs                      # DbContext atualizado
```

### Frontend (Angular 18)
```
angular/src/app/
├── proxy/                                 # Auto-gerado pelo ABP
│   ├── leads/
│   │   ├── lead.service.ts               # API client
│   │   ├── models.ts                     # DTOs TypeScript
│   │   └── lead-status.enum.ts           # Enums
│   ├── accounts/
│   ├── contacts/
│   ├── activities/
│   └── sales-pipeline/
└── features/                             # Componentes CRM
    ├── leads/
    │   ├── lead-list.component.ts        # Listagem com filtro
    │   ├── lead-create.component.ts      # Formulário criação
    │   ├── lead-edit.component.ts        # Formulário edição
    │   ├── lead-detail.component.ts      # Detalhes
    │   └── lead-pipeline.component.ts    # Pipeline Kanban
    ├── accounts/
    ├── contacts/
    ├── activities/
    │   ├── activity-calendar.component.ts
    │   └── activity-timeline.component.ts
    ├── sales-pipeline/
    │   ├── pipeline-kanban.component.ts  # Funil visual
    │   └── pipeline-dashboard.component.ts
    └── dashboard/
        ├── sales-dashboard.component.ts
        └── metrics-chart.component.ts
```

## 9. Padrões ABP + Angular a Seguir

### Backend (.NET 9)

#### Entidades
```csharp
// Domain Layer
public class Lead : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public LeadStatus Status { get; set; }

    // Business logic methods
    public void ConvertToOpportunity()
    {
        if (Status != LeadStatus.Qualified)
            throw new BusinessException("Lead must be qualified first");

        Status = LeadStatus.Converted;
    }
}
```

#### Application Services
```csharp
// Application Layer
[Authorize(ZenCrmPermissions.Leads.Default)]
public class LeadAppService : ApplicationService, ILeadAppService
{
    private readonly IRepository<Lead, Guid> _leadRepository;

    public async Task<PagedResultDto<LeadDto>> GetListAsync(GetLeadsInput input)
    {
        var queryable = await _leadRepository.GetQueryableAsync();
        var query = queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter) || x.Email.Contains(input.Filter))
            .OrderBy(x => x.CreationTime);

        var leads = await AsyncExecuter.ToListAsync(query);
        return new PagedResultDto<LeadDto>(
            leads.Count,
            ObjectMapper.Map<List<Lead>, List<LeadDto>>(leads)
        );
    }
}
```

#### Permissões
```csharp
// Application.Contracts/Permissions/ZenCrmPermissions.cs
public static class ZenCrmPermissions
{
    public const string GroupName = "ZenCrm";

    public static class Leads
    {
        public const string Default = GroupName + ".Leads";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string ConvertToOpportunity = Default + ".ConvertToOpportunity";
    }

    public static class Accounts { /* Similar */ }
    public static class Contacts { /* Similar */ }
    public static class Activities { /* Similar */ }
}
```

#### EF Core Configuration
```csharp
// EntityFrameworkCore/Configurations/LeadConfiguration.cs
builder.Entity<Lead>(b =>
{
    b.ToTable(ZenCrmConsts.DbTablePrefix + "Leads", ZenCrmConsts.DbSchema);
    b.ConfigureByConvention(); // ABP convenções

    b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    b.Property(x => x.Email).IsRequired().HasMaxLength(256);
    b.Property(x => x.Phone).HasMaxLength(32);

    b.HasIndex(x => x.Email).IsUnique();
    b.HasIndex(x => x.Status);
});
```

### Frontend (Angular 18)

#### Componente Standalone
```typescript
// features/leads/lead-list.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { ListService } from '@abp/ng.core';
import { LeadService, LeadDto } from '@proxy/leads';

@Component({
  selector: 'app-lead-list',
  templateUrl: './lead-list.component.html',
  styleUrls: ['./lead-list.component.css'],
  imports: [
    FormsModule, ReactiveFormsModule,
    NgbPaginationModule, NgxDatatableModule,
    ListService // ABP ListService para paginação
  ],
  providers: [ListService]
})
export class LeadListComponent implements OnInit {
  private leadService = inject(LeadService);
  private list = inject(ListService);

  leads = { items: [], totalCount: 0 } as PagedResultDto<LeadDto>;

  ngOnInit(): void {
    const streamCreator = (query) => this.leadService.getList(query);
    this.list.hookToQuery(streamCreator).subscribe(response => {
      this.leads = response;
    });
  }

  delete(id: string): void {
    this.leadService.delete(id).subscribe(() => this.list.get());
  }
}
```

#### Template com ABP Components
```html
<!-- features/leads/lead-list.component.html -->
<div class="card">
  <div class="card-header">
    <h5>{{ '::Menu:Leads' | abpLocalization }}</h5>
    <div class="card-actions">
      <button class="btn btn-primary" routerLink="/leads/create">
        <i class="fas fa-plus me-2"></i>{{ '::NewLead' | abpLocalization }}
      </button>
    </div>
  </div>

  <div class="card-body">
    <ngx-datatable
      [rows]="leads.items"
      [count]="leads.totalCount"
      [columns]="columns"
      [externalPaging]="true"
      [offset]="list.page - 1"
      [limit]="list.maxResultCount"
      (page)="list.page = $event.page + 1">

      <ngx-datatable-column name="name" prop="name" [sortable]="true">
      </ngx-datatable-column>

      <ngx-datatable-column name="email" prop="email" [sortable]="true">
      </ngx-datatable-column>

      <ngx-datatable-column name="status" prop="status" [sortable]="true">
        <ng-template let-status="row" ngx-datatable-cell-template>
          <span class="badge badge-{{ status | statusColor }}">
            {{ status | translate }}
          </span>
        </ng-template>
      </ngx-datatable-column>

      <ngx-datatable-column name="actions">
        <ng-template let-row="row" ngx-datatable-cell-template>
          <button class="btn btn-sm btn-primary"
                  [routerLink]="['/leads', row.id]">
            <i class="fas fa-edit"></i>
          </button>
          <button class="btn btn-sm btn-danger"
                  (click)="delete(row.id)">
            <i class="fas fa-trash"></i>
          </button>
        </ng-template>
      </ngx-datatable-column>
    </ngx-datatable>
  </div>
</div>
```

### Integração Backend-Frontend

#### 1. Fluxo de Dados
```
Angular Component → ABP Proxy Service → HTTP API → Application Service → Repository → EF Core → Database
```

#### 2. Proxy Auto-Gerado
- Após implementar `LeadAppService` no backend
- ABP gera automaticamente `lead.service.ts` no frontend
- TypeScript types e métodos sincronizados

#### 3. Autenticação Integrada
```typescript
// ABP gerencia tokens automaticamente
this.leadService.create(leadDto).subscribe(
  response => {
    // Success - token incluído automaticamente
    this.router.navigate(['/leads', response.id]);
  },
  error => {
    // Error handling com ABP error handling
    this.message.error(error.error.error.message);
  }
);
```

## 10. Vantagens da Arquitetura Escolhida

### ✅ **ABP Layered + Angular**
- **Type Safety**: Full stack TypeScript/.NET synchronization
- **Performance**: Angular SPA com lazy loading otimizado
- **Developer Experience**: IntelliSense completo, auto-completion
- **Productivity**: Proxies gerados automaticamente
- **Scalability**: Arquitetura testada para SaaS enterprise
- **Maintainability**: Separação clara de responsabilidades
- **Multi-tenancy**: Suporte nativo do ABP
- **Modern Stack**: .NET 9 + Angular 18 + TypeScript

### 🔥 **Para CRM SaaS**
- **UX Profissional**: Comparable a HubSpot/Salesforce
- **SEO Ready**: Angular Universal para server rendering
- **Mobile First**: PWA capabilities incluídas
- **Real-time**: SignalR para notificações
- **Analytics**: Integração fácil com Google Analytics

Este planejamento moderniza a arquitetura mantendo as melhores práticas do ABP Framework e proporcionando uma base sólida para crescimento escalável do CRM SaaS.
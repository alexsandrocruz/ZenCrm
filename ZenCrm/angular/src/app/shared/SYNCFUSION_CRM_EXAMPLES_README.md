# Syncfusion CRM Examples - ZenCrm

Este diretório contém exemplos práticos de implementação de componentes Syncfusion específicos para o projeto ZenCrm.

## 📁 Estrutura dos Arquivos

```
syncfusion-crm-examples/
├── syncfusion-client-grid.component.ts    # Grid de clientes avançado
├── syncfusion-opportunity-form.component.ts # Formulário de oportunidades
├── syncfusion-sales-dashboard.component.ts # Dashboard de vendas
└── index.ts                               # Export e configurações
```

## 🚀 Exemplos Disponíveis

### 1. Client Grid (`SyncfusionClientGridComponent`)

**Caso de Uso**: Listagem e gerenciamento de clientes

**Features Implementadas**:
- ✅ Grid avançado com paginação, sorting, filtering
- ✅ Search em tempo real
- ✅ Filtros por status e indústria
- ✅ Ações inline (View, Edit)
- ✅ Export para Excel/PDF
- ✅ KPI cards com estatísticas
- ✅ Responsive design

**Como Usar**:
```typescript
import { SyncfusionClientGridComponent } from '../shared/syncfusion-crm-examples';

@Component({
  // ...
  imports: [SyncfusionClientGridComponent, ...]
})
export class YourComponent { }
```

**Template**:
```html
<app-syncfusion-client-grid></app-syncfusion-client-grid>
```

### 2. Opportunity Form (`SyncfusionOpportunityFormComponent`)

**Caso de Uso**: Formulário completo para criação de oportunidades

**Features Implementadas**:
- ✅ Formulário multi-seção
- ✅ Validação automática
- ✅ Cálculo de weighted value
- ✅ Rich text editor
- ✅ Multi-select para produtos e tags
- ✅ Auto-probability por stage
- ✅ Draft functionality

**Como Usar**:
```typescript
import { SyncfusionOpportunityFormComponent } from '../shared/syncfusion-crm-examples';

@Component({
  // ...
  imports: [SyncfusionOpportunityFormComponent, ...]
})
export class YourComponent { }
```

**Template**:
```html
<app-syncfusion-opportunity-form></app-syncfusion-opportunity-form>
```

### 3. Sales Dashboard (`SyncfusionSalesDashboardComponent`)

**Caso de Uso**: Dashboard de analytics e visualização de dados

**Features Implementadas**:
- ✅ KPI cards interativos
- ✅ Múltiplos tipos de gráficos
- ✅ Date range selector
- ✅ Real-time updates
- ✅ Activities timeline
- ✅ Responsive layout

**Como Usar**:
```typescript
import { SyncfusionSalesDashboardComponent } from '../shared/syncfusion-crm-examples';

@Component({
  // ...
  imports: [SyncfusionSalesDashboardComponent, ...]
})
export class YourComponent { }
```

**Template**:
```html
<app-syncfusion-simple-dashboard></app-syncfusion-simple-dashboard>
```

## 🔧 Configuração dos Módulos

Para usar todos os exemplos, importe os módulos necessários:

```typescript
import { SyncfusionCrmComplete } from '../shared/syncfusion-crm-modules';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ...SyncfusionCrmComplete, // Todos os módulos Syncfusion
    SyncfusionClientGridComponent,
    SyncfusionOpportunityFormComponent,
    SyncfusionSimpleDashboardComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class YourModule { }
```

## 📊 Componentes Syncfusion Utilizados

### Grid Components
- `ejs-grid` - Tabelas de dados avançadas
- `ej-pagesettings` - Configurações de paginação
- `ej-filtersettings` - Configurações de filtro
- `ej-editsettings` - Configurações de edição

### Form Components
- `ejs-textbox` - Campos de texto
- `ejs-numerictextbox` - Campos numéricos
- `ejs-dropdownlist` - Dropdowns simples
- `ejs-multiselect` - Multi-select
- `ejs-datepicker` - Seleção de data
- `ejs-richtexteditor` - Editor rich text
- `ejs-checkbox` - Checkboxes
- `ejs-button` - Botões

### Chart Components
- `ejs-chart` - Gráficos de linha, coluna, área
- `ejs-chart type="Funnel"` - Funnel charts
- `ejs-chart type="Pie"` - Gráficos de pizza

### Layout Components
- `ejs-card` - Cards para layouts
- `ejs-toolbar` - Barras de ferramentas

## 🎨 Customização e Theming

### Cores e Estilos
Os exemplos usam variáveis CSS que podem ser customizadas:

```scss
// Variáveis de cores principais
$syncfusion-primary: #007bff;
$syncfusion-success: #28a745;
$syncfusion-warning: #ffc107;
$syncfusion-info: #17a2b8;
$syncfusion-danger: #dc3545;

// Variáveis de spacing
$syncfusion-spacing-sm: 0.5rem;
$syncfusion-spacing-md: 1rem;
$syncfusion-spacing-lg: 1.5rem;
```

### Tema Personalizado
Para customizar o tema Syncfusion:

```typescript
// No seu componente ou globalmente
import { setCulture } from '@syncfusion/ej2-base';

// Configurações do tema
const themeConfig = {
  palette: {
    primary: '#007bff',
    success: '#28a745',
    warning: '#ffc107',
    info: '#17a2b8',
    danger: '#dc3545'
  }
};
```

## 🔄 Integração com ABP Framework

### Permissões
```typescript
// Adicionar verificações de permissão
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: 'clients',
    component: SyncfusionClientGridComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'ZenCrm.Clients'
    }
  }
];
```

### Localização
```typescript
// Usar pipe de localização do ABP
import { LocalizationPipe } from '@abp/ng.core';

// No template
{{ '::Menu:Clients' | abpLocalization }}
```

### APIs
```typescript
// Integração com serviços do ABP
import { RestService } from '@abp/ng.core';

constructor(private rest: RestService) {}

loadClients(): Observable<Client[]> {
  return this.rest.request<{ items: Client[] }>({
    method: 'GET',
    url: '/api/app/client'
  }).pipe(map(response => response.items));
}
```

## 📱 Responsividade

Os componentes são configurados para funcionar em diferentes tamanhos de tela:

### Breakpoints
- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

### Ajustes Responsivos
```css
/* Media queries para ajustes específicos */
@media (max-width: 768px) {
  .card-body {
    padding: 1rem;
  }

  .action-buttons {
    flex-direction: column;
  }
}
```

## 🧪 Testes

### Testes Unitários
Exemplo de como testar os componentes:

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SyncfusionClientGridComponent } from './syncfusion-client-grid.component';

describe('SyncfusionClientGridComponent', () => {
  let component: SyncfusionClientGridComponent;
  let fixture: ComponentFixture<SyncfusionClientGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SyncfusionClientGridComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(SyncfusionClientGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should filter clients correctly', () => {
    component.searchText = 'Acme';
    component.applyFilters();
    expect(component.filteredClients.length).toBeGreaterThan(0);
  });
});
```

### Testes E2E
Exemplo com Cypress:

```typescript
describe('Client Grid', () => {
  beforeEach(() => {
    cy.visit('/examples/client-grid');
  });

  it('should display client grid', () => {
    cy.get('app-syncfusion-client-grid').should('be.visible');
    cy.get('.e-grid').should('exist');
  });

  it('should filter by search', () => {
    cy.get('[formcontrolname="searchInput"]').type('Acme');
    cy.get('.e-gridcontent').should('contain', 'Acme');
  });
});
```

## 📚 Próximos Passos

### Roadmap de Implementação
1. **Phase 1**: Implementar Client Grid em produção
2. **Phase 2**: Adicionar Opportunity Form ao fluxo principal
3. **Phase 3**: Integrar Sales Dashboard com APIs reais
4. **Phase 4**: Adicionar componentes Kanban e Schedule
5. **Phase 5**: Implementar mobile apps com componentes otimizados

### Customizações Sugeridas
- **Validação Personalizada**: Adicionar validadores específicos do negócio
- **Templates de Email**: Integrar com templates Syncfusion para comunicações
- **Export Avançado**: Configurar exports customizados com branding
- **Offline Support**: Adicionar suporte offline com dados cacheados
- **Real-time Updates**: Implementar WebSocket para atualizações em tempo real

## 🔗 Links Úteis

- [Documentação Oficial Syncfusion](https://ej2.syncfusion.com/angular/documentation/)
- [API Reference](https://ej2.syncfusion.com/angular/documentation/api/)
- [Theme Studio](https://ej2.syncfusion.com/angular/themestudio/)
- [Online Demos](https://ej2.syncfusion.com/angular/demos/)

---

*Última atualização: November 2025*
*Versão: Syncfusion Essential JS 2 for Angular v20+*
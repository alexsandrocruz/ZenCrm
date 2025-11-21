# Syncfusion CRM Blocks Reference - ZenCrm Project

Este documento serve como um banco de referências completo dos componentes Syncfusion disponíveis e seu uso recomendado no projeto ZenCrm.

## 📋 Índice

- [Data Management Components](#data-management-components)
- [Form Components](#form-components)
- [Dashboard & Analytics](#dashboard--analytics)
- [Navigation & Layout](#navigation--layout)
- [Calendar & Scheduling](#calendar--scheduling)
- [Communication & Collaboration](#communication--collaboration)
- [File Management](#file-management)
- [Advanced UI Components](#advanced-ui-components)

---

## 📊 Data Management Components

### Grid (Data Table)
**Componente**: `ejs-grid`
**Caso de Uso CRM**: Listagem de clientes, leads, oportunidades, interações
**Features Essenciais**:
- Paginação, sorting, filtering
- Column resizing & reordering
- Export to Excel/PDF
- Cell editing
- Grouping & aggregation
- Search functionality
- Virtual scrolling para grandes datasets

**Exemplo Uso**:
```typescript
// Client list grid
<ejs-grid
  [dataSource]="clients"
  [allowPaging]="true"
  [allowSorting]="true"
  [allowFiltering]="true"
  [allowGrouping]="true"
  [pageSettings]="{ pageSize: 10 }"
  [filterSettings]="{ type: 'Menu' }"
  (actionComplete)="onGridActionComplete($event)">

  <e-columns>
    <e-column field="name" headerText="Client Name" width="150"></e-column>
    <e-column field="email" headerText="Email" width="200"></e-column>
    <e-column field="phone" headerText="Phone" width="150"></e-column>
    <e-column field="status" headerText="Status" width="100"></e-column>
    <e-column headerText="Actions" width="120" [commands]="gridCommands"></e-column>
  </e-columns>
</ejs-grid>
```

### TreeGrid
**Componente**: `ejs-treegrid`
**Caso de Uso CRM**: Estrutura hierárquica de organizações, projetos, categorias
**Features Essenciais**:
- Hierarchical data display
- Self-referential data binding
- CRUD operations
- Excel-like editing
- Aggregation at parent levels

---

## 📝 Form Components

### Form Builder
**Componente**: Combine `ejs-textbox`, `ejs-numerictextbox`, `ejs-datepicker`, etc.
**Caso de Uso CRM**: Formulários de cadastro de cliente, lead, oportunidade

**Client Registration Form Template**:
```html
<div class="client-form">
  <div class="row">
    <div class="col-md-6">
      <h5>Informações Pessoais</h5>
      <ejs-textbox name="firstName" [(ngModel)]="client.firstName" placeholder="First Name" floatLabelType="Auto"></ejs-textbox>
      <ejs-textbox name="lastName" [(ngModel)]="client.lastName" placeholder="Last Name" floatLabelType="Auto"></ejs-textbox>
      <ejs-textbox name="email" [(ngModel)]="client.email" placeholder="Email" floatLabelType="Auto"></ejs-textbox>
      <ejs-maskedtextbox name="phone" [(ngModel)]="client.phone" mask="(000) 000-0000" floatLabelType="Auto"></ejs-maskedtextbox>
    </div>
    <div class="col-md-6">
      <h5>Informações da Empresa</h5>
      <ejs-textbox name="company" [(ngModel)]="client.company" placeholder="Company" floatLabelType="Auto"></ejs-textbox>
      <ejs-textbox name="position" [(ngModel)]="client.position" placeholder="Position" floatLabelType="Auto"></ejs-textbox>
      <ejs-dropdownlist name="industry" [(ngModel)]="client.industry" [dataSource]="industries" floatLabelType="Auto"></ejs-dropdownlist>
      <ejs-numerictextbox name="annualRevenue" [value]="client.annualRevenue" (change)="onRevenueChange($event)" format="c2" floatLabelType="Auto"></ejs-numerictextbox>
    </div>
  </div>
</div>
```

### Rich Text Editor
**Componente**: `ejs-richtexteditor`
**Caso de Uso CRM**: Notas de interação, descrições de oportunidades, templates de email

### File Upload
**Componente**: `ejs-uploader`
**Caso de Uso CRM**: Upload de documentos de cliente, contratos, propostas

---

## 📈 Dashboard & Analytics

### Charts
**Componente**: `ejs-chart`
**Caso de Uso CRM**: Analytics de vendas, performance de equipe, funil de vendas

**Sales Dashboard Charts**:
```typescript
// Funnel Chart para Pipeline
<ejs-chart type='Funnel' [dataSource]="pipelineData">
  <e-series-collection>
    <e-series [dataSource]="pipelineData" xName='stage' yName='value' type='Funnel'></e-series>
  </e-series-collection>
</ejs-chart>

// Line Chart para Trends
<ejs-chart>
  <e-series-collection>
    <e-series type='Line' [dataSource]="salesTrends" xName='month' yName='revenue'></e-series>
  </e-series-collection>
</ejs-chart>

// Column Chart para Comparison
<ejs-chart type='Column'>
  <e-series-collection>
    <e-series [dataSource]="teamPerformance" xName='salesRep' yName='deals'></e-series>
  </e-series-collection>
</ejs-chart>
```

### KPI Cards
**Componente**: Custom components com `ejs-card` e métricas
**Caso de Uso CRM**: Dashboard de métricas principais

### Gauge Components
**Componente**: `ejs-circulargauge`, `ejs-lineargauge`
**Caso de Uso CRM**: Metas de vendas, satisfaction scores

---

## 🗺️ Navigation & Layout

### Sidebar Navigation
**Componente**: `ejs-sidebar` + `ejs-menu`
**Caso de Uso CRM**: Menu de navegação principal do sistema

### Tab Container
**Componente**: `ejs-tab`
**Caso de Uso CRM**: Organização de informações de cliente, vistas de detalhe

### Accordion
**Componente**: `ejs-accordion`
**Caso de Uso CRM**: FAQ, filtros avançados, seções expansíveis

### Toolbar
**Componente**: `ejs-toolbar`
**Caso de Uso CRM**: Barra de ações para grids e formulários

---

## 📅 Calendar & Scheduling

### Schedule (Calendar)
**Componente**: `ejs-schedule`
**Caso de Uso CRM**: Agendamento de reuniões, follow-ups, tarefas

**Appointment Management**:
```typescript
<ejs-schedule
  [currentView]="currentView"
  [eventSettings]="{ dataSource: appointments }"
  (actionComplete)="onScheduleActionComplete($event)">

  <e-views>
    <e-view option="Day"></e-view>
    <e-view option="Week"></e-view>
    <e-view option="Month"></e-view>
    <e-view option="TimelineMonth"></e-view>
  </e-views>
</ejs-schedule>
```

### DateRangePicker
**Componente**: `ejs-daterangepicker`
**Caso de Uso CRM**: Filtros de data para relatórios e listagens

---

## 💬 Communication & Collaboration

### Chat Interface
**Componente**: `ejs-listview` + custom chat UI
**Caso de Uso CRM**: Comunicação interna, chat com cliente

### Comments/Notes
**Componente**: `ejs-listview` + `ejs-richtexteditor`
**Caso de Uso CRM**: Sistema de notas e comentários em registros

### Timeline
**Componente**: `ejs-timeline` (custom implementation)
**Caso de Uso CRM**: Histórico de interações com cliente

---

## 📁 File Management

### FileManager
**Componente**: `ejs-filemanager`
**Caso de Uso CRM**: Gestão de documentos de cliente, templates, recursos

### Document Viewer
**Componente**: `ej-pdfviewer`
**Caso de Uso CRM**: Visualização de contratos, propostas

---

## 🎨 Advanced UI Components

### Kanban Board
**Componente**: `ejs-kanban`
**Caso de Uso CRM**: Pipeline visual de vendas, gestão de tarefas

**Sales Pipeline Kanban**:
```typescript
<ejs-kanban
  [dataSource]="opportunities"
  keyField="status"
  [cardSettings]="{ contentField: 'title', headerField: 'priority' }"
  [columns]="kanbanColumns">
</ejs-kanban>
```

### Gantt Chart
**Componente**: `ejs-gantt`
**Caso de Uso CRM**: Planejamento de projetos complexos, roadmap de implementação

### Diagram Builder
**Componente**: `ej-diagram`
**Caso de Uso CRM**: Mapeamento de processos, organogramas

---

## 🔧 Configuration Templates

### CRM Grid Configuration
```typescript
export const CRM_GRID_CONFIG = {
  clients: {
    columns: [
      { field: 'name', headerText: 'Client Name', width: 150 },
      { field: 'email', headerText: 'Email', width: 200 },
      { field: 'phone', headerText: 'Phone', width: 150 },
      { field: 'company', headerText: 'Company', width: 180 },
      { field: 'status', headerText: 'Status', width: 100 },
      { field: 'createdDate', headerText: 'Created', format: 'dd/MM/yyyy' }
    ],
    pageSize: 20,
    allowFiltering: true,
    allowSorting: true,
    allowGrouping: true
  },
  opportunities: {
    columns: [
      { field: 'title', headerText: 'Opportunity', width: 200 },
      { field: 'client', headerText: 'Client', width: 150 },
      { field: 'value', headerText: 'Value', format: 'C2' },
      { field: 'stage', headerText: 'Stage' },
      { field: 'probability', headerText: 'Probability %', format: 'N0' },
      { field: 'expectedCloseDate', headerText: 'Expected Close', format: 'dd/MM/yyyy' }
    ]
  }
};
```

### Form Field Configurations
```typescript
export const CRM_FORM_FIELDS = {
  client: {
    personal: [
      { type: 'textbox', field: 'firstName', label: 'First Name', required: true },
      { type: 'textbox', field: 'lastName', label: 'Last Name', required: true },
      { type: 'textbox', field: 'email', label: 'Email', required: true, validation: 'email' },
      { type: 'maskedtextbox', field: 'phone', label: 'Phone', mask: '(000) 000-0000' }
    ],
    company: [
      { type: 'textbox', field: 'company', label: 'Company' },
      { type: 'dropdownlist', field: 'industry', label: 'Industry', dataSource: 'industries' },
      { type: 'numerictextbox', field: 'revenue', label: 'Annual Revenue', format: 'c2' },
      { type: 'numerictextbox', field: 'employees', label: 'Employees' }
    ]
  }
};
```

---

## 🎯 Best Practices for ZenCrm

### 1. Performance Optimization
- Use virtual scrolling para grids com mais de 1000 registros
- Implementar lazy loading para dropdowns grandes
- Utilizar on-demand loading para componentes pesados

### 2. Responsive Design
- Configurar breakpoints para mobile/tablet/desktop
- Usar adaptive UI para diferentes tamanhos de tela
- Implementar touch-friendly interfaces

### 3. Accessibility
- Adicionar ARIA labels
- Suportar keyboard navigation
- Implementar high contrast themes

### 4. Theming
- Manter consistência com o tema LeptonX do ABP
- Usar variáveis CSS para cores e espaçamentos
- Suportar modo dark/light

---

## 🚀 Implementation Roadmap

### Phase 1: Core Components
- [x] Grid components (clients, leads, opportunities)
- [x] Form components (registration, editing)
- [x] Basic dashboard charts

### Phase 2: Advanced Features
- [ ] Kanban board para pipeline
- [ ] Schedule component para agendamentos
- [ ] Rich text editor para notas

### Phase 3: Enhanced UX
- [ ] File management system
- [ ] Advanced analytics dashboards
- [ ] Mobile-optimized interfaces

---

## 📚 Additional Resources

### Documentation Links
- Grid: https://ej2.syncfusion.com/angular/documentation/grid/
- Forms: https://ej2.syncfusion.com/angular/documentation/
- Charts: https://ej2.syncfusion.com/angular/documentation/chart/
- Schedule: https://ej2.syncfusion.com/angular/documentation/schedule/

### Component-Specific Examples
Cada componente incluído neste documento possui exemplos práticos que podem ser adaptados diretamente para o ZenCrm.

---

*Última atualização: November 2025*
*Versão: Syncfusion Essential JS 2 for Angular*
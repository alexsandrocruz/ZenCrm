import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionGridModules } from '../syncfusion-modules';
import { GridComponent, PageSettingsModel, FilterSettingsModel, EditSettingsModel } from '@syncfusion/ej2-angular-grids';

// Mock data - substituir com API real
interface Client {
  id: string;
  name: string;
  email: string;
  phone: string;
  company: string;
  industry: string;
  status: 'Active' | 'Inactive' | 'Prospect';
  annualRevenue: number;
  createdDate: Date;
  lastContactDate: Date;
}

@Component({
  selector: 'app-syncfusion-client-grid',
  standalone: true,
  imports: [CommonModule, ...SyncfusionGridModules],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <div class="row">
        <div class="col-md-12">
          <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
              <h5 class="mb-0">
                <i class="fas fa-users me-2"></i>
                Client Management
              </h5>
              <div>
                <ejs-button
                  content="Add Client"
                  cssClass="e-primary e-small me-2"
                  (click)="addClient()">
                </ejs-button>
                <ejs-button
                  content="Export to Excel"
                  cssClass="e-success e-small me-2"
                  (click)="exportToExcel()">
                </ejs-button>
                <ejs-button
                  content="Refresh"
                  cssClass="e-info e-small"
                  (click)="refreshGrid()">
                </ejs-button>
              </div>
            </div>
            <div class="card-body">
              <!-- Search Bar -->
              <div class="row mb-3">
                <div class="col-md-6">
                  <ejs-textbox
                    name="searchInput"
                    [(ngModel)]="searchText"
                    placeholder="Search clients..."
                    floatLabelType="Auto"
                    (change)="onSearchChange($event)">
                  </ejs-textbox>
                </div>
                <div class="col-md-3">
                  <ejs-dropdownlist
                    name="statusFilter"
                    [value]="selectedStatus"
                    (change)="onStatusFilterChange($event)"
                    [dataSource]="statusOptions"
                    [fields]="{ text: 'text', value: 'value' }"
                    placeholder="Filter by Status"
                    floatLabelType="Auto">
                  </ejs-dropdownlist>
                </div>
                <div class="col-md-3">
                  <ejs-dropdownlist
                    name="industryFilter"
                    [value]="selectedIndustry"
                    (change)="onIndustryFilterChange($event)"
                    [dataSource]="industryOptions"
                    [fields]="{ text: 'text', value: 'value' }"
                    placeholder="Filter by Industry"
                    floatLabelType="Auto">
                  </ejs-dropdownlist>
                </div>
              </div>

              <!-- Data Grid -->
              <ejs-grid
                #grid
                [dataSource]="filteredClients"
                [allowPaging]="true"
                [allowSorting]="true"
                [allowFiltering]="true"
                [allowGrouping]="true"
                [allowExcelExport]="true"
                [allowPdfExport]="true"
                [pageSettings]="pageSettings"
                [filterSettings]="filterSettings"
                [editSettings]="editSettings"
                [toolbar]="toolbarOptions"
                (actionBegin)="onActionBegin($event)"
                (actionComplete)="onActionComplete($event)"
                (recordClick)="onRecordClick($event)">

                <e-columns>
                  <e-column
                    field="name"
                    headerText="Client Name"
                    width="150"
                    [isPrimaryKey]="true">
                  </e-column>

                  <e-column
                    field="email"
                    headerText="Email"
                    width="200"
                    [allowEditing]="false">
                  </e-column>

                  <e-column
                    field="phone"
                    headerText="Phone"
                    width="150"
                    format="({0})">
                  </e-column>

                  <e-column
                    field="company"
                    headerText="Company"
                    width="180">
                  </e-column>

                  <e-column
                    field="industry"
                    headerText="Industry"
                    width="120"
                    [editType]="'dropdownedit'"
                    [dataSource]="industryOptions"
                    [fields]="{ text: 'text', value: 'value' }">
                  </e-column>

                  <e-column
                    field="status"
                    headerText="Status"
                    width="100"
                    [editType]="'dropdownedit'"
                    [dataSource]="statusOptions"
                    [fields]="{ text: 'text', value: 'value' }">
                  </e-column>

                  <e-column
                    field="annualRevenue"
                    headerText="Revenue"
                    width="120"
                    format="C2"
                    [textAlign]="'Right'">
                  </e-column>

                  <e-column
                    field="createdDate"
                    headerText="Created"
                    width="120"
                    format="dd/MM/yyyy"
                    [textAlign]="'Center'">
                  </e-column>

                  <e-column
                    field="lastContactDate"
                    headerText="Last Contact"
                    width="120"
                    format="dd/MM/yyyy"
                    [textAlign]="'Center'">
                  </e-column>

                  <e-column
                    headerText="Actions"
                    width="150"
                    [textAlign]="'Center'">
                    <ng-template #template let-data>
                      <div class="action-buttons">
                        <ejs-button
                          content="View"
                          cssClass="e-info e-small"
                          (click)="viewClient(data)">
                        </ejs-button>
                        <ejs-button
                          content="Edit"
                          cssClass="e-warning e-small"
                          (click)="editClient(data)">
                        </ejs-button>
                      </div>
                    </ng-template>
                  </e-column>
                </e-columns>
              </ejs-grid>
            </div>
          </div>
        </div>
      </div>

      <!-- Statistics Cards -->
      <div class="row mt-4">
        <div class="col-md-3">
          <div class="card bg-primary text-white">
            <div class="card-body">
              <h5 class="card-title">Total Clients</h5>
              <h2 class="card-text">{{ totalClients }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-success text-white">
            <div class="card-body">
              <h5 class="card-title">Active</h5>
              <h2 class="card-text">{{ activeClients }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-warning text-white">
            <div class="card-body">
              <h5 class="card-title">Prospects</h5>
              <h2 class="card-text">{{ prospectClients }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card bg-info text-white">
            <div class="card-body">
              <h5 class="card-title">Avg Revenue</h5>
              <h2 class="card-text">{{ averageRevenue | currency:'USD':'symbol':'1.0-0' }}</h2>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .action-buttons {
      display: flex;
      gap: 5px;
      justify-content: center;
    }

    .card {
      box-shadow: 0 0 15px rgba(0,0,0,0.1);
      margin-bottom: 20px;
    }

    .card-header {
      background-color: #f8f9fa;
      border-bottom: 1px solid #dee2e6;
    }

    .bg-primary { background-color: #007bff !important; }
    .bg-success { background-color: #28a745 !important; }
    .bg-warning { background-color: #ffc107 !important; }
    .bg-info { background-color: #17a2b8 !important; }
  `]
})
export class SyncfusionClientGridComponent implements OnInit {
  @ViewChild('grid') grid!: GridComponent;

  // Mock data - substituir com API real
  clients: Client[] = [
    {
      id: '1',
      name: 'Acme Corporation',
      email: 'contact@acme.com',
      phone: '5551234567',
      company: 'Acme Corporation',
      industry: 'Technology',
      status: 'Active',
      annualRevenue: 2500000,
      createdDate: new Date('2023-01-15'),
      lastContactDate: new Date('2024-11-20')
    },
    {
      id: '2',
      name: 'Global Industries',
      email: 'info@global.com',
      phone: '5559876543',
      company: 'Global Industries',
      industry: 'Manufacturing',
      status: 'Active',
      annualRevenue: 5000000,
      createdDate: new Date('2023-03-22'),
      lastContactDate: new Date('2024-11-18')
    },
    {
      id: '3',
      name: 'StartUp Solutions',
      email: 'hello@startup.com',
      phone: '5554567890',
      company: 'StartUp Solutions',
      industry: 'Technology',
      status: 'Prospect',
      annualRevenue: 500000,
      createdDate: new Date('2024-09-10'),
      lastContactDate: new Date('2024-11-15')
    }
  ];

  filteredClients: Client[] = [];

  // Grid settings
  pageSettings: PageSettingsModel = { pageSize: 10, pageCount: 5 };
  filterSettings: FilterSettingsModel = { type: 'Menu' };
  editSettings: EditSettingsModel = {
    allowEditing: true,
    allowAdding: true,
    allowDeleting: true,
    mode: 'Dialog'
  };
  toolbarOptions = ['Add', 'Edit', 'Delete', 'Update', 'Cancel', 'ExcelExport', 'PdfExport', 'Search'];

  // Filter options
  statusOptions = [
    { text: 'All', value: 'All' },
    { text: 'Active', value: 'Active' },
    { text: 'Inactive', value: 'Inactive' },
    { text: 'Prospect', value: 'Prospect' }
  ];

  industryOptions = [
    { text: 'All', value: 'All' },
    { text: 'Technology', value: 'Technology' },
    { text: 'Manufacturing', value: 'Manufacturing' },
    { text: 'Healthcare', value: 'Healthcare' },
    { text: 'Finance', value: 'Finance' },
    { text: 'Retail', value: 'Retail' }
  ];

  // Form bindings
  searchText = '';
  selectedStatus = 'All';
  selectedIndustry = 'All';

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.filteredClients = [...this.clients];
  }

  // Event handlers
  onSearchChange(event: any): void {
    this.applyFilters();
  }

  onStatusFilterChange(event: any): void {
    this.selectedStatus = event.value;
    this.applyFilters();
  }

  onIndustryFilterChange(event: any): void {
    this.selectedIndustry = event.value;
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredClients = this.clients.filter(client => {
      const matchesSearch = !this.searchText ||
        client.name.toLowerCase().includes(this.searchText.toLowerCase()) ||
        client.email.toLowerCase().includes(this.searchText.toLowerCase()) ||
        client.company.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesStatus = this.selectedStatus === 'All' || client.status === this.selectedStatus;
      const matchesIndustry = this.selectedIndustry === 'All' || client.industry === this.selectedIndustry;

      return matchesSearch && matchesStatus && matchesIndustry;
    });

    this.grid.refresh();
  }

  // Grid actions
  onActionBegin(args: any): void {
    // Handle grid action begin events
    console.log('Action begin:', args);
  }

  onActionComplete(args: any): void {
    // Handle grid action complete events
    console.log('Action complete:', args);
  }

  onRecordClick(args: any): void {
    // Handle record click
    console.log('Record clicked:', args.rowData);
  }

  // Button actions
  addClient(): void {
    this.router.navigate(['/crm/clients/create']);
  }

  viewClient(client: Client): void {
    this.router.navigate(['/crm/clients', client.id]);
  }

  editClient(client: Client): void {
    this.router.navigate(['/crm/clients', client.id, 'edit']);
  }

  exportToExcel(): void {
    this.grid.excelExport();
  }

  refreshGrid(): void {
    this.grid.refresh();
    console.log('Grid refreshed');
  }

  // Statistics getters
  get totalClients(): number {
    return this.filteredClients.length;
  }

  get activeClients(): number {
    return this.filteredClients.filter(c => c.status === 'Active').length;
  }

  get prospectClients(): number {
    return this.filteredClients.filter(c => c.status === 'Prospect').length;
  }

  get averageRevenue(): number {
    if (this.filteredClients.length === 0) return 0;
    const total = this.filteredClients.reduce((sum, client) => sum + client.annualRevenue, 0);
    return total / this.filteredClients.length;
  }
}
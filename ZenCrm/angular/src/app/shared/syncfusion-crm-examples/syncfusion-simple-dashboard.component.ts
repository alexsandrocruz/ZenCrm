import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionCommonModules } from '../syncfusion-modules';

@Component({
  selector: 'app-syncfusion-simple-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ...SyncfusionCommonModules
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <div class="row mb-4">
        <div class="col-md-12">
          <h2>
            <i class="fas fa-chart-line me-2"></i>
            Simple Dashboard Demo
          </h2>
        </div>
      </div>

      <!-- KPI Cards -->
      <div class="row mb-4">
        <div class="col-md-3">
          <div class="card bg-primary text-white">
            <div class="card-body">
              <h5>Total Revenue</h5>
              <h3>{{ totalRevenue | currency:'USD':'symbol' }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card bg-success text-white">
            <div class="card-body">
              <h5>New Deals</h5>
              <h3>{{ newDeals }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card bg-warning text-white">
            <div class="card-body">
              <h5>Pipeline Value</h5>
              <h3>{{ pipelineValue | currency:'USD':'symbol' }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card bg-info text-white">
            <div class="card-body">
              <h5>Win Rate</h5>
              <h3>{{ winRate }}%</h3>
            </div>
          </div>
        </div>
      </div>

      <!-- Demo Controls -->
      <div class="row mb-4">
        <div class="col-md-12">
          <div class="card">
            <div class="card-header">
              <h5>Syncfusion Components Demo</h5>
            </div>
            <div class="card-body">
              <div class="row">
                <div class="col-md-6">
                  <h6>Form Controls</h6>
                  <div class="mb-3">
                    <ejs-textbox
                      name="demoText"
                      [(ngModel)]="demoText"
                      placeholder="Type here..."
                      floatLabelType="Auto">
                    </ejs-textbox>
                  </div>
                  <div class="mb-3">
                    <ejs-dropdownlist
                      name="demoDropdown"
                      [value]="selectedOption"
                      (change)="onDropdownChange($event)"
                      [dataSource]="dropdownOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Select option"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                </div>
                <div class="col-md-6">
                  <h6>Display Data</h6>
                  <p><strong>Text:</strong> {{ demoText }}</p>
                  <p><strong>Selected:</strong> {{ selectedOption }}</p>
                  <p><strong>Counter:</strong> {{ counter }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="row">
        <div class="col-md-12">
          <div class="card">
            <div class="card-body">
              <div class="d-flex gap-2">
                <ejs-button
                  content="Increment Counter"
                  cssClass="e-primary"
                  (click)="incrementCounter()">
                </ejs-button>
                <ejs-button
                  content="Reset Form"
                  cssClass="e-secondary"
                  (click)="resetForm()">
                </ejs-button>
                <ejs-button
                  content="Show Message"
                  cssClass="e-success"
                  (click)="showMessage()">
                </ejs-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
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
export class SyncfusionSimpleDashboardComponent implements OnInit {
  demoText = '';
  selectedOption = '';
  counter = 0;

  totalRevenue = 456000;
  newDeals = 47;
  pipelineValue = 1325000;
  winRate = 68;

  dropdownOptions = [
    { text: 'Option 1', value: 'option1' },
    { text: 'Option 2', value: 'option2' },
    { text: 'Option 3', value: 'option3' }
  ];

  constructor() {}

  ngOnInit(): void {
    console.log('Simple dashboard initialized');
  }

  onDropdownChange(event: any): void {
    this.selectedOption = event.value;
  }

  incrementCounter(): void {
    this.counter++;
  }

  resetForm(): void {
    this.demoText = '';
    this.selectedOption = '';
    this.counter = 0;
  }

  showMessage(): void {
    alert('Syncfusion components are working!');
  }
}
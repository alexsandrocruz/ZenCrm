import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionCommonModules } from './syncfusion-modules';

@Component({
  selector: 'app-syncfusion-demo',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ...SyncfusionCommonModules
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <h2>Syncfusion Components Demo</h2>

      <!-- Buttons Section -->
      <div class="row mb-4">
        <div class="col-md-12">
          <h3>Buttons</h3>
          <div class="d-flex gap-2 flex-wrap">
            <ejs-button content="Primary Button" cssClass="e-primary"></ejs-button>
            <ejs-button content="Success Button" cssClass="e-success"></ejs-button>
            <ejs-button content="Warning Button" cssClass="e-warning"></ejs-button>
            <ejs-button content="Danger Button" cssClass="e-danger" isToggle="true"></ejs-button>
          </div>
        </div>
      </div>

      <!-- Form Controls Section -->
      <div class="row mb-4">
        <div class="col-md-6">
          <h3>Input Controls</h3>
          <div class="mb-3">
            <label>Text Box:</label>
            <ejs-textbox
              name="textBox"
              [(ngModel)]="textBoxValue"
              placeholder="Enter text here..."
              floatLabelType="Auto">
            </ejs-textbox>
          </div>

          <div class="mb-3">
            <label>Numeric Text Box:</label>
            <ejs-numerictextbox
              [value]="numericValue"
              (change)="onNumericValueChange($event)"
              [min]="0"
              [max]="100"
              [step]="1"
              format="n2"
              floatLabelType="Auto">
            </ejs-numerictextbox>
          </div>

          <div class="mb-3">
            <label>Date Picker:</label>
            <ejs-datepicker
              [value]="selectedDate"
              (change)="onDateChange($event)"
              format="dd/MM/yyyy"
              placeholder="Select date..."
              floatLabelType="Auto">
            </ejs-datepicker>
          </div>
        </div>

        <div class="col-md-6">
          <h3>Selection Controls</h3>
          <div class="mb-3">
            <label>Radio Buttons:</label>
            <div class="d-flex gap-3">
              <ejs-radiobutton
                label="Option 1"
                name="radioGroup"
                [(ngModel)]="radioValue"
                value="option1">
              </ejs-radiobutton>
              <ejs-radiobutton
                label="Option 2"
                name="radioGroup"
                [(ngModel)]="radioValue"
                value="option2">
              </ejs-radiobutton>
            </div>
          </div>

          <div class="mb-3">
            <label>Check Box:</label>
            <ejs-checkbox
              name="checked"
              label="Accept terms and conditions"
              [(ngModel)]="checked">
            </ejs-checkbox>
          </div>

          <div class="mb-3">
            <label>Switch:</label>
            <ejs-switch
              [checked]="switchValue"
              (change)="onSwitchChange($event)">
            </ejs-switch>
          </div>
        </div>
      </div>

      <!-- Dropdown Section -->
      <div class="row mb-4">
        <div class="col-md-6">
          <h3>Dropdown</h3>
          <div class="mb-3">
            <label>Select Country:</label>
            <ejs-dropdownlist
              [value]="selectedCountry"
              (change)="onCountryChange($event)"
              [dataSource]="countries"
              [fields]="{ text: 'name', value: 'code' }"
              placeholder="Select a country..."
              floatLabelType="Auto">
            </ejs-dropdownlist>
          </div>
        </div>
      </div>

      <!-- Current Values Display -->
      <div class="row mb-4">
        <div class="col-md-12">
          <h3>Current Values</h3>
          <div class="card">
            <div class="card-body">
              <p><strong>Text Box:</strong> {{ textBoxValue }}</p>
              <p><strong>Numeric Value:</strong> {{ numericValue }}</p>
              <p><strong>Selected Date:</strong> {{ selectedDate | date:'dd/MM/yyyy' }}</p>
              <p><strong>Radio Selection:</strong> {{ radioValue }}</p>
              <p><strong>Checkbox Checked:</strong> {{ checked }}</p>
              <p><strong>Switch Value:</strong> {{ switchValue }}</p>
              <p><strong>Selected Country:</strong> {{ getCountryName(selectedCountry) }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="row mb-4">
        <div class="col-md-12">
          <ejs-button
            content="Show Dialog"
            cssClass="e-primary"
            (click)="showDialog()">
          </ejs-button>
          <ejs-button
            content="Reset Form"
            cssClass="e-secondary"
            (click)="resetForm()">
          </ejs-button>
        </div>
      </div>

      <!-- Dialog -->
      <ejs-dialog
        #dialog
        [visible]="dialogVisible"
        [header]="dialogHeader"
        [isModal]="true"
        [allowDragging]="true"
        [showCloseIcon]="true"
        (close)="onDialogClose()">
        <div class="p-3">
          <p>Syncfusion components are working correctly in ZenCrm!</p>
          <p>This is a demonstration of various Syncfusion UI components.</p>
          <ejs-button
            content="Close"
            cssClass="e-primary"
            (click)="closeDialog()">
          </ejs-button>
        </div>
      </ejs-dialog>
    </div>
  `,
  styles: [`
    .container-fluid {
      max-width: 1200px;
    }

    h3 {
      color: #495057;
      border-bottom: 2px solid #e9ecef;
      padding-bottom: 8px;
      margin-bottom: 20px;
    }

    .mb-3 {
      margin-bottom: 1rem;
    }

    .card {
      background-color: #f8f9fa;
      border: 1px solid #dee2e6;
    }

    .card-body p {
      margin-bottom: 0.5rem;
    }

    .d-flex {
      display: flex;
    }

    .gap-2 {
      gap: 0.5rem;
    }

    .gap-3 {
      gap: 1rem;
    }

    .flex-wrap {
      flex-wrap: wrap;
    }
  `]
})
export class SyncfusionDemoComponent {
  // Form values
  textBoxValue: string = '';
  numericValue: number = 0;
  selectedDate: Date | null = null;
  radioValue: string = 'option1';
  checked: boolean = false;
  switchValue: boolean = true;
  selectedCountry: string = '';

  // Dialog properties
  dialogVisible: boolean = false;
  dialogHeader: string = 'Syncfusion Integration Status';

  // Dropdown data
  countries = [
    { name: 'United States', code: 'US' },
    { name: 'Brazil', code: 'BR' },
    { name: 'United Kingdom', code: 'UK' },
    { name: 'Canada', code: 'CA' },
    { name: 'Germany', code: 'DE' },
    { name: 'France', code: 'FR' },
    { name: 'Spain', code: 'ES' },
    { name: 'Italy', code: 'IT' },
    { name: 'Portugal', code: 'PT' },
    { name: 'Japan', code: 'JP' }
  ];

  constructor() {}

  // Event handlers for Syncfusion components
  onNumericValueChange(event: any): void {
    this.numericValue = event.value;
  }

  onDateChange(event: any): void {
    this.selectedDate = event.value;
  }

  onCountryChange(event: any): void {
    this.selectedCountry = event.value;
  }

  onSwitchChange(event: any): void {
    this.switchValue = event.checked;
  }

  // Methods
  showDialog(): void {
    this.dialogVisible = true;
  }

  closeDialog(): void {
    this.dialogVisible = false;
  }

  onDialogClose(): void {
    this.dialogVisible = false;
  }

  resetForm(): void {
    this.textBoxValue = '';
    this.numericValue = 0;
    this.selectedDate = null;
    this.radioValue = 'option1';
    this.checked = false;
    this.switchValue = true;
    this.selectedCountry = '';
  }

  getCountryName(code: string): string {
    const country = this.countries.find(c => c.code === code);
    return country ? country.name : 'Not selected';
  }
}
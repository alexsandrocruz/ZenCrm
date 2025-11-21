import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { SyncfusionFormModules } from '../syncfusion-modules';

// Form data interfaces
interface Opportunity {
  title: string;
  clientId: string;
  contactId: string;
  value: number;
  currency: string;
  stage: string;
  probability: number;
  expectedCloseDate: Date;
  description: string;
  products: string[];
  tags: string[];
  source: string;
  priority: string;
  assignedTo: string;
}

@Component({
  selector: 'app-syncfusion-opportunity-form',
  standalone: true,
  imports: [CommonModule, ...SyncfusionFormModules],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <div class="container-fluid mt-4">
      <div class="row">
        <div class="col-md-12">
          <div class="card">
            <div class="card-header">
              <h5 class="mb-0">
                <i class="fas fa-chart-line me-2"></i>
                Create New Opportunity
              </h5>
            </div>
            <div class="card-body">
              <form (ngSubmit)="submitOpportunity()">
                <!-- Basic Information -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Basic Information</h6>
                    <hr>
                  </div>
                  <div class="col-md-8">
                    <ejs-textbox
                      name="title"
                      [(ngModel)]="opportunity.title"
                      placeholder="Opportunity Title"
                      floatLabelType="Auto"
                      required>
                    </ejs-textbox>
                  </div>
                  <div class="col-md-4">
                    <ejs-dropdownlist
                      name="priority"
                      [value]="opportunity.priority"
                      (change)="onPriorityChange($event)"
                      [dataSource]="priorityOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Priority"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                </div>

                <!-- Client Information -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Client Information</h6>
                    <hr>
                  </div>
                  <div class="col-md-6">
                    <ejs-dropdownlist
                      name="clientId"
                      [value]="opportunity.clientId"
                      (change)="onClientChange($event)"
                      [dataSource]="clients"
                      [fields]="{ text: 'name', value: 'id' }"
                      placeholder="Select Client"
                      floatLabelType="Auto"
                      required>
                    </ejs-dropdownlist>
                  </div>
                  <div class="col-md-6">
                    <ejs-dropdownlist
                      name="contactId"
                      [value]="opportunity.contactId"
                      (change)="onContactChange($event)"
                      [dataSource]="contacts"
                      [fields]="{ text: 'name', value: 'id' }"
                      placeholder="Select Contact"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                </div>

                <!-- Financial Information -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Financial Information</h6>
                    <hr>
                  </div>
                  <div class="col-md-4">
                    <ejs-numerictextbox
                      name="value"
                      [value]="opportunity.value"
                      (change)="onValueChange($event)"
                      placeholder="Opportunity Value"
                      format="c2"
                      floatLabelType="Auto"
                      [min]="0">
                    </ejs-numerictextbox>
                  </div>
                  <div class="col-md-4">
                    <ejs-dropdownlist
                      name="currency"
                      [value]="opportunity.currency"
                      (change)="onCurrencyChange($event)"
                      [dataSource]="currencyOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Currency"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                  <div class="col-md-4">
                    <ejs-numerictextbox
                      name="probability"
                      [value]="opportunity.probability"
                      (change)="onProbabilityChange($event)"
                      placeholder="Probability %"
                      format="n0"
                      floatLabelType="Auto"
                      [min]="0"
                      [max]="100">
                    </ejs-numerictextbox>
                  </div>
                </div>

                <!-- Pipeline Information -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Pipeline Information</h6>
                    <hr>
                  </div>
                  <div class="col-md-4">
                    <ejs-dropdownlist
                      name="stage"
                      [value]="opportunity.stage"
                      (change)="onStageChange($event)"
                      [dataSource]="stageOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Pipeline Stage"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                  <div class="col-md-4">
                    <ejs-datepicker
                      name="expectedCloseDate"
                      [value]="opportunity.expectedCloseDate"
                      (change)="onExpectedCloseDateChange($event)"
                      placeholder="Expected Close Date"
                      format="dd/MM/yyyy"
                      floatLabelType="Auto">
                    </ejs-datepicker>
                  </div>
                  <div class="col-md-4">
                    <ejs-dropdownlist
                      name="source"
                      [value]="opportunity.source"
                      (change)="onSourceChange($event)"
                      [dataSource]="sourceOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Lead Source"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                </div>

                <!-- Assignment -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Assignment</h6>
                    <hr>
                  </div>
                  <div class="col-md-6">
                    <ejs-dropdownlist
                      name="assignedTo"
                      [value]="opportunity.assignedTo"
                      (change)="onAssignedToChange($event)"
                      [dataSource]="salesReps"
                      [fields]="{ text: 'name', value: 'id' }"
                      placeholder="Assign To"
                      floatLabelType="Auto">
                    </ejs-dropdownlist>
                  </div>
                </div>

                <!-- Products -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Products/Services</h6>
                    <hr>
                  </div>
                  <div class="col-md-12">
                    <ejs-multiselect
                      name="products"
                      [value]="opportunity.products"
                      (change)="onProductsChange($event)"
                      [dataSource]="productOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Select Products/Services"
                      floatLabelType="Auto">
                    </ejs-multiselect>
                  </div>
                </div>

                <!-- Description -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Description</h6>
                    <hr>
                  </div>
                  <div class="col-md-12">
                    <ejs-richtexteditor
                      name="description"
                      [(value)]="opportunity.description"
                      [toolbarSettings]="toolbarSettings"
                      placeholder="Describe the opportunity, challenges, and solutions...">
                    </ejs-richtexteditor>
                  </div>
                </div>

                <!-- Tags -->
                <div class="row mb-4">
                  <div class="col-12">
                    <h6>Tags</h6>
                    <hr>
                  </div>
                  <div class="col-md-12">
                    <ejs-multiselect
                      name="tags"
                      [value]="opportunity.tags"
                      (change)="onTagsChange($event)"
                      [dataSource]="tagOptions"
                      [fields]="{ text: 'text', value: 'value' }"
                      placeholder="Add Tags"
                      [allowFiltering]="true"
                      floatLabelType="Auto">
                    </ejs-multiselect>
                  </div>
                </div>

                <!-- Form Actions -->
                <div class="row">
                  <div class="col-md-12">
                    <div class="d-flex justify-content-between">
                      <div>
                        <ejs-button
                          content="Save as Draft"
                          cssClass="e-secondary"
                          (click)="saveAsDraft()">
                        </ejs-button>
                      </div>
                      <div>
                        <ejs-button
                          content="Cancel"
                          cssClass="e-light"
                          (click)="cancel()">
                        </ejs-button>
                        <ejs-button
                          content="Create Opportunity"
                          cssClass="e-primary"
                          type="submit">
                        </ejs-button>
                      </div>
                    </div>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>

      <!-- Opportunity Summary Card -->
      <div class="row mt-4" *ngIf="opportunity.value > 0">
        <div class="col-md-12">
          <div class="card border-primary">
            <div class="card-header bg-primary text-white">
              <h6 class="mb-0">Opportunity Summary</h6>
            </div>
            <div class="card-body">
              <div class="row">
                <div class="col-md-3">
                  <strong>Expected Value:</strong>
                  <span class="d-block h5 text-primary">{{ opportunity.value | currency:opportunity.currency }}</span>
                </div>
                <div class="col-md-3">
                  <strong>Probability:</strong>
                  <div class="progress mt-2">
                    <div class="progress-bar" [style.width.%]="opportunity.probability">
                      {{ opportunity.probability }}%
                    </div>
                  </div>
                </div>
                <div class="col-md-3">
                  <strong>Weighted Value:</strong>
                  <span class="d-block h5 text-success">{{ weightedValue | currency:opportunity.currency }}</span>
                </div>
                <div class="col-md-3">
                  <strong>Expected Close:</strong>
                  <span class="d-block">{{ opportunity.expectedCloseDate | date:'longDate' }}</span>
                </div>
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

    .bg-primary {
      background-color: #007bff !important;
    }

    hr {
      margin-top: 0.5rem;
      margin-bottom: 1rem;
    }

    h6 {
      color: #495057;
      font-weight: 600;
    }

    .progress {
      height: 25px;
    }

    .d-flex {
      gap: 10px;
    }
  `]
})
export class SyncfusionOpportunityFormComponent implements OnInit {
  // Form data
  opportunity: Opportunity = {
    title: '',
    clientId: '',
    contactId: '',
    value: 0,
    currency: 'USD',
    stage: '',
    probability: 25,
    expectedCloseDate: new Date(),
    description: '',
    products: [],
    tags: [],
    source: '',
    priority: 'Medium',
    assignedTo: ''
  };

  // Dropdown options
  priorityOptions = [
    { text: 'Low', value: 'Low' },
    { text: 'Medium', value: 'Medium' },
    { text: 'High', value: 'High' },
    { text: 'Critical', value: 'Critical' }
  ];

  stageOptions = [
    { text: 'Qualification', value: 'Qualification' },
    { text: 'Needs Analysis', value: 'Needs Analysis' },
    { text: 'Value Proposition', value: 'Value Proposition' },
    { text: 'Proposal', value: 'Proposal' },
    { text: 'Negotiation', value: 'Negotiation' },
    { text: 'Closed Won', value: 'Closed Won' },
    { text: 'Closed Lost', value: 'Closed Lost' }
  ];

  currencyOptions = [
    { text: 'USD - US Dollar', value: 'USD' },
    { text: 'EUR - Euro', value: 'EUR' },
    { text: 'GBP - British Pound', value: 'GBP' },
    { text: 'BRL - Brazilian Real', value: 'BRL' }
  ];

  sourceOptions = [
    { text: 'Website', value: 'Website' },
    { text: 'Referral', value: 'Referral' },
    { text: 'Cold Call', value: 'Cold Call' },
    { text: 'Email', value: 'Email' },
    { text: 'Social Media', value: 'Social Media' },
    { text: 'Trade Show', value: 'Trade Show' },
    { text: 'Partner', value: 'Partner' }
  ];

  // Mock data - substituir com API real
  clients = [
    { id: '1', name: 'Acme Corporation' },
    { id: '2', name: 'Global Industries' },
    { id: '3', name: 'StartUp Solutions' }
  ];

  contacts = [
    { id: '1', name: 'John Smith - Acme Corp' },
    { id: '2', name: 'Sarah Johnson - Global Industries' },
    { id: '3', name: 'Mike Davis - StartUp Solutions' }
  ];

  salesReps = [
    { id: '1', name: 'Alice Wilson' },
    { id: '2', name: 'Bob Thompson' },
    { id: '3', name: 'Carol Martinez' }
  ];

  productOptions = [
    { text: 'CRM Software', value: 'CRM Software' },
    { text: 'ERP Solution', value: 'ERP Solution' },
    { text: 'Consulting Services', value: 'Consulting Services' },
    { text: 'Training Programs', value: 'Training Programs' },
    { text: 'Support Package', value: 'Support Package' }
  ];

  tagOptions = [
    { text: 'High-Value', value: 'High-Value' },
    { text: 'Enterprise', value: 'Enterprise' },
    { text: 'Strategic', value: 'Strategic' },
    { text: 'Urgent', value: 'Urgent' },
    { text: 'Long-term', value: 'Long-term' },
    { text: 'Competitor', value: 'Competitor' }
  ];

  // Rich text editor settings
  toolbarSettings = {
    items: ['Bold', 'Italic', 'Underline', '|', 'Formats', 'Alignments', 'OrderedList', 'UnorderedList', '|', 'CreateLink', 'Image', '|', 'SourceCode']
  };

  // Calculated property
  get weightedValue(): number {
    return (this.opportunity.value * this.opportunity.probability) / 100;
  }

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Initialize form with default values
  }

  // Event handlers for form fields
  onPriorityChange(event: any): void {
    this.opportunity.priority = event.value;
  }

  onClientChange(event: any): void {
    this.opportunity.clientId = event.value;
    // Load contacts for selected client
    this.loadContactsForClient(event.value);
  }

  onContactChange(event: any): void {
    this.opportunity.contactId = event.value;
  }

  onValueChange(event: any): void {
    this.opportunity.value = event.value;
  }

  onCurrencyChange(event: any): void {
    this.opportunity.currency = event.value;
  }

  onProbabilityChange(event: any): void {
    this.opportunity.probability = event.value;
  }

  onStageChange(event: any): void {
    this.opportunity.stage = event.value;
    // Auto-adjust probability based on stage
    this.adjustProbabilityByStage(event.value);
  }

  onExpectedCloseDateChange(event: any): void {
    this.opportunity.expectedCloseDate = event.value;
  }

  onSourceChange(event: any): void {
    this.opportunity.source = event.value;
  }

  onAssignedToChange(event: any): void {
    this.opportunity.assignedTo = event.value;
  }

  onProductsChange(event: any): void {
    this.opportunity.products = event.value;
  }

  onTagsChange(event: any): void {
    this.opportunity.tags = event.value;
  }

  // Helper methods
  loadContactsForClient(clientId: string): void {
    // Mock implementation - replace with actual API call
    console.log('Loading contacts for client:', clientId);
  }

  adjustProbabilityByStage(stage: string): void {
    const stageProbabilities = {
      'Qualification': 25,
      'Needs Analysis': 40,
      'Value Proposition': 60,
      'Proposal': 75,
      'Negotiation': 90,
      'Closed Won': 100,
      'Closed Lost': 0
    };

    if (stageProbabilities[stage as keyof typeof stageProbabilities]) {
      this.opportunity.probability = stageProbabilities[stage as keyof typeof stageProbabilities];
    }
  }

  // Form actions
  submitOpportunity(): void {
    console.log('Submitting opportunity:', this.opportunity);
    // Add form validation here
    // Call API to save opportunity
    // Redirect to opportunity details page
  }

  saveAsDraft(): void {
    console.log('Saving as draft:', this.opportunity);
    // Save opportunity as draft
  }

  cancel(): void {
    this.router.navigate(['/crm/opportunities']);
  }
}
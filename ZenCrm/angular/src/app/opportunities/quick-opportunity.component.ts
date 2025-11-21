import { Component, inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LocalizationService } from '@abp/ng.core';
import { NgbModalModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe } from '@abp/ng.core';
import { priorityOptions } from '../proxy/sales';
import { SalesOpportunityService } from '../proxy/sales';
import { SalesLeadService } from '../proxy/sales';
import { CreateUpdateSalesOpportunityDto } from '../proxy/sales';

@Component({
  selector: 'app-quick-opportunity',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    LocalizationPipe
  ],
  template: `
    <ng-container>
      <ng-template #content let-closed="closed">
        <div class="modal-header">
          <h5 class="modal-title">{{ '::Opportunity:QuickOpportunity' | abpLocalization }}</h5>
          <button type="button" class="btn-close" (click)="closed()"></button>
        </div>
        <div class="modal-body">
          <form [formGroup]="form" (ngSubmit)="save()">
            <div class="mb-3">
              <label class="form-label">{{ '::Opportunity:Name' | abpLocalization }} *</label>
              <input type="text" class="form-control" formControlName="name" required />
            </div>

            <div class="row">
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">{{ '::Opportunity:ExpectedClose' | abpLocalization }} *</label>
                  <input type="date" class="form-control" formControlName="expectedCloseDate" required />
                </div>
              </div>
              <div class="col-md-6">
                <div class="mb-3">
                  <label class="form-label">{{ '::Opportunity:EstimatedValue' | abpLocalization }} *</label>
                  <input type="number" class="form-control" formControlName="estimatedValue" min="0.01" step="0.01" required />
                </div>
              </div>
            </div>

            <div class="mb-3">
              <label class="form-label">{{ '::Opportunity:Priority' | abpLocalization }} *</label>
              <select class="form-select" formControlName="priority" required>
                @for(priority of priorityOptions; track priority.value) {
                  <option [ngValue]="priority.value">{{ priority.key }}</option>
                }
              </select>
            </div>

            <div class="mb-3">
              <label class="form-label">{{ '::Opportunity:Description' | abpLocalization }}</label>
              <textarea class="form-control" formControlName="description" rows="3"></textarea>
            </div>

            <div class="form-check">
              <input class="form-check-input" type="checkbox" id="isActive" formControlName="isActive">
              <label class="form-check-label" for="isActive">{{ '::Opportunity:Active' | abpLocalization }}</label>
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" (click)="closed()">{{ '::Close' | abpLocalization }}</button>
          <button type="button" class="btn btn-primary" (click)="save()" [disabled]="form.invalid || isSaving">
            <span *ngIf="!isSaving">{{ '::Save' | abpLocalization }}</span>
            <span *ngIf="isSaving">{{ '::Saving' | abpLocalization }}...</span>
          </button>
        </div>
      </ng-template>
    </ng-container>
  `,
  styles: [`
    .modal {
      display: block !important;
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0,0,0,0.5);
      z-index: 1050;
    }
    .modal-content {
      position: relative;
      background: white;
      margin: 50px auto;
      max-width: 600px;
      border-radius: 8px;
      box-shadow: 0 4px 6px rgba(0,0,0,0.1);
    }
  `]
})
export class QuickOpportunityComponent {
  private readonly fb = inject(FormBuilder);
  private readonly localization = inject(LocalizationService);
  private readonly salesOpportunityService = inject(SalesOpportunityService);
  private readonly modalService = inject(NgbModal);

  form: FormGroup;
  isModalOpen = false;
  isSaving = false;
  modalRef: any;

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      expectedCloseDate: ['', Validators.required],
      estimatedValue: [0, [Validators.required, Validators.min(0.01)]],
      priority: [1, Validators.required],
      description: [''],
      isActive: [true]
    });

    // Set default expected close date (today + 45 days)
    this.setDefaultExpectedCloseDate();
  }

  setDefaultExpectedCloseDate(): void {
    const today = new Date();
    const futureDate = new Date(today);
    futureDate.setDate(today.getDate() + 45);

    this.form.patchValue({
      expectedCloseDate: futureDate.toISOString().split('T')[0],
      priority: 1, // Normal priority
      isActive: true
    });
  }

  open(): void {
    this.isModalOpen = true;
    this.resetForm();

    this.modalRef = this.modalService.open('', {
      centered: true,
      size: 'lg'
    });

    // Replace modal content with our template
    const modalElement = document.querySelector('.modal-content');
    if (modalElement) {
      modalElement.innerHTML = `
        <div class="modal-header">
          <h5 class="modal-title">${this.localization.instant('::Opportunity:QuickOpportunity')}</h5>
          <button type="button" class="btn-close" onclick="this.closest('.modal').style.display='none'"></button>
        </div>
        <div class="modal-body">
          <div id="quick-opportunity-form-container"></div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" onclick="this.closest('.modal').style.display='none'">${this.localization.instant('::Close')}</button>
          <button type="button" class="btn btn-primary" onclick="window.quickOpportunityComponent.save()">${this.localization.instant('::Save')}</button>
        </div>
      `;

      // Make component available globally for onclick handlers
      (window as any).quickOpportunityComponent = this;

      // Append form to container
      setTimeout(() => {
        const container = document.getElementById('quick-opportunity-form-container');
        if (container) {
          this.createFormInModal(container);
        }
      }, 100);
    }
  }

  createFormInModal(container: HTMLElement): void {
    const formHtml = `
      <form>
        <div class="mb-3">
          <label class="form-label">${this.localization.instant('::Opportunity:Name')} *</label>
          <input type="text" class="form-control" id="quick-name" required />
        </div>

        <div class="row">
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">${this.localization.instant('::Opportunity:ExpectedClose')} *</label>
              <input type="date" class="form-control" id="quick-date" required />
            </div>
          </div>
          <div class="col-md-6">
            <div class="mb-3">
              <label class="form-label">${this.localization.instant('::Opportunity:EstimatedValue')} *</label>
              <input type="number" class="form-control" id="quick-value" min="0.01" step="0.01" required />
            </div>
          </div>
        </div>

        <div class="mb-3">
          <label class="form-label">${this.localization.instant('::Opportunity:Priority')} *</label>
          <select class="form-select" id="quick-priority" required>
            <option value="1">Normal</option>
            <option value="2">High</option>
            <option value="3">Critical</option>
          </select>
        </div>

        <div class="mb-3">
          <label class="form-label">${this.localization.instant('::Opportunity:Description')}</label>
          <textarea class="form-control" id="quick-description" rows="3"></textarea>
        </div>
      </form>
    `;

    container.innerHTML = formHtml;

    // Set default values
    const nameInput = document.getElementById('quick-name') as HTMLInputElement;
    const dateInput = document.getElementById('quick-date') as HTMLInputElement;
    const valueInput = document.getElementById('quick-value') as HTMLInputElement;

    if (nameInput) nameInput.value = 'New Quick Opportunity';
    if (dateInput) dateInput.value = this.form.get('expectedCloseDate')?.value || '';
    if (valueInput) valueInput.value = '1000';
  }

  close(): void {
    this.isModalOpen = false;
    if (this.modalRef) {
      this.modalRef.close();
    }
  }

  resetForm(): void {
    this.form.reset();
    this.setDefaultExpectedCloseDate();
  }

  save(): void {
    const nameInput = document.getElementById('quick-name') as HTMLInputElement;
    const dateInput = document.getElementById('quick-date') as HTMLInputElement;
    const valueInput = document.getElementById('quick-value') as HTMLInputElement;
    const priorityInput = document.getElementById('quick-priority') as HTMLSelectElement;
    const descriptionInput = document.getElementById('quick-description') as HTMLTextAreaElement;

    if (!nameInput?.value || !dateInput?.value || !valueInput?.value) {
      alert(this.localization.instant('::Opportunity:FillRequiredFields') || 'Please fill all required fields');
      return;
    }

    this.isSaving = true;

    const opportunityData: CreateUpdateSalesOpportunityDto = {
      name: nameInput.value,
      expectedCloseDate: dateInput.value,
      estimatedValue: parseFloat(valueInput.value),
      priority: parseInt(priorityInput?.value || '1'),
      description: descriptionInput?.value || '',
      isActive: true
    };

    this.salesOpportunityService.create(opportunityData).subscribe({
      next: (result) => {
        this.isSaving = false;
        this.close();

        // Return to list page without confirmation dialog
        window.location.reload();
      },
      error: (error) => {
        this.isSaving = false;
        console.error('Error creating opportunity:', error);
        alert(this.localization.instant('::Opportunity:Error') || 'Error creating opportunity');
      }
    });
  }
}
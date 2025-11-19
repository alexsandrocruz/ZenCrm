import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import {
  InteractionDto,
  CreateUpdateInteractionDto
} from '../proxy/sales/models';
import { InteractionType } from '../proxy/sales/interaction-type.enum';
import { InteractionStatus } from '../proxy/sales/interaction-status.enum';
import { Priority } from '../proxy/sales/priority.enum';
import { interactionTypeOptions } from '../proxy/sales/interaction-type.enum';
import { interactionStatusOptions } from '../proxy/sales/interaction-status.enum';
import { priorityOptions } from '../proxy/sales/priority.enum';
import { SimpleInteractionService } from '../services/simple-interaction.service';
import { SimpleClientService } from '../services/simple-client.service';
import { CustomerService } from '../proxy/sales/customer.service';
import { CustomerDto } from '../proxy/sales/models';
import { ClientDto } from '../proxy/sales/models';

@Component({
  selector: 'app-interaction-form',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, LocalizationPipe],
  template: `
    <div class="modal-header">
      <h5 class="modal-title">
        {{ interaction ? ('::Interaction:EditInteraction' | abpLocalization) : ('::Interaction:NewInteraction' | abpLocalization) }}
      </h5>
      <button type="button" class="btn-close" (click)="activeModal.dismiss()"></button>
    </div>
    <div class="modal-body">
      <form [formGroup]="interactionForm" (ngSubmit)="saveInteraction()">
        <!-- Informações Básicas -->
        <div class="row mb-3">
          <div class="col-12">
            <label for="subject" class="form-label">{{ '::Interaction:Subject' | abpLocalization }} *</label>
            <input type="text" id="subject" class="form-control" formControlName="subject"
                   [class.is-invalid]="isFieldInvalid('subject')"
                   placeholder="{{ '::Interaction:SubjectPlaceholder' | abpLocalization }}">
            @if(isFieldInvalid('subject')) {
              <div class="invalid-feedback d-block">
                {{ getErrorMessage('subject') }}
              </div>
            }
          </div>
        </div>

        <div class="row mb-3">
          <div class="col-md-4">
            <label for="type" class="form-label">{{ '::Interaction:Type' | abpLocalization }} *</label>
            <select id="type" class="form-select" formControlName="type"
                    [class.is-invalid]="isFieldInvalid('type')">
              <option value="">{{ '::Interaction:SelectType' | abpLocalization }}</option>
              @for(option of typeOptions; track option.value) {
                <option [value]="option.value">{{ '::Enum:InteractionType.' + option.key | abpLocalization }}</option>
              }
            </select>
            @if(isFieldInvalid('type')) {
              <div class="invalid-feedback d-block">
                {{ getErrorMessage('type') }}
              </div>
            }
          </div>
          <div class="col-md-4">
            <label for="status" class="form-label">{{ '::Interaction:Status' | abpLocalization }}</label>
            <select id="status" class="form-select" formControlName="status">
              @for(option of statusOptions; track option.value) {
                <option [value]="option.value">{{ '::Enum:InteractionStatus.' + option.key | abpLocalization }}</option>
              }
            </select>
          </div>
          <div class="col-md-4">
            <label for="priority" class="form-label">{{ '::Interaction:Priority' | abpLocalization }}</label>
            <select id="priority" class="form-select" formControlName="priority">
              <option value="">{{ '::Interaction:SelectPriority' | abpLocalization }}</option>
              @for(option of priorityOptions; track option.value) {
                <option [value]="option.value">{{ '::Enum:Priority.' + option.key | abpLocalization }}</option>
              }
            </select>
          </div>
        </div>

        <!-- Relacionamentos -->
        <div class="row mb-3">
          <div class="col-md-4">
            <label for="clientId" class="form-label">{{ '::Interaction:Client' | abpLocalization }}</label>
            <select id="clientId" class="form-select" formControlName="clientId" (change)="onClientChange()">
              <option value="">{{ '::Interaction:SelectClient' | abpLocalization }}</option>
              @for(client of clients; track client.id) {
                <option [value]="client.id">{{ client.name }}</option>
              }
            </select>
          </div>
          <div class="col-md-4">
            <label for="customerId" class="form-label">{{ '::Interaction:Customer' | abpLocalization }}</label>
            <select id="customerId" class="form-select" formControlName="customerId" [disabled]="!selectedClientId">
              <option value="">{{ '::Interaction:SelectCustomer' | abpLocalization }}</option>
              @for(customer of customers; track customer.id) {
                <option [value]="customer.id">{{ customer.firstName }} {{ customer.lastName }}</option>
              }
            </select>
          </div>
          <div class="col-md-4">
            <label for="ownerUserId" class="form-label">{{ '::Interaction:Owner' | abpLocalization }}</label>
            <input type="text" id="ownerUserId" class="form-control" formControlName="ownerUserId"
                   placeholder="{{ '::Interaction:OwnerPlaceholder' | abpLocalization }}">
          </div>
        </div>

        <!-- Datas e Horas -->
        <div class="row mb-3">
          <div class="col-md-6">
            <label for="scheduledDate" class="form-label">{{ '::Interaction:ScheduledDate' | abpLocalization }} *</label>
            <input type="datetime-local" id="scheduledDate" class="form-control" formControlName="scheduledDate"
                   [class.is-invalid]="isFieldInvalid('scheduledDate')">
            @if(isFieldInvalid('scheduledDate')) {
              <div class="invalid-feedback d-block">
                {{ getErrorMessage('scheduledDate') }}
              </div>
            }
          </div>
          <div class="col-md-6">
            <label for="durationMinutes" class="form-label">{{ '::Interaction:DurationMinutes' | abpLocalization }}</label>
            <input type="number" id="durationMinutes" class="form-control" formControlName="durationMinutes"
                   min="0" placeholder="{{ '::Interaction:DurationPlaceholder' | abpLocalization }}">
          </div>
        </div>

        <!-- Data/Hora Início e Fim -->
        <div class="row mb-3">
          <div class="col-md-6">
            <label for="startDate" class="form-label">{{ '::Interaction:StartDate' | abpLocalization }}</label>
            <input type="datetime-local" id="startDate" class="form-control" formControlName="startDate">
          </div>
          <div class="col-md-6">
            <label for="endDate" class="form-label">{{ '::Interaction:EndDate' | abpLocalization }}</label>
            <input type="datetime-local" id="endDate" class="form-control" formControlName="endDate">
          </div>
        </div>

        <!-- Localização -->
        <div class="row mb-3">
          <div class="col-12">
            <label for="location" class="form-label">{{ '::Interaction:Location' | abpLocalization }}</label>
            <input type="text" id="location" class="form-control" formControlName="location"
                   placeholder="{{ '::Interaction:LocationPlaceholder' | abpLocalization }}">
          </div>
        </div>

        <!-- Descrição -->
        <div class="row mb-3">
          <div class="col-12">
            <label for="description" class="form-label">{{ '::Interaction:Description' | abpLocalization }}</label>
            <textarea id="description" class="form-control" formControlName="description" rows="3"
                      placeholder="{{ '::Interaction:DescriptionPlaceholder' | abpLocalization }}"></textarea>
          </div>
        </div>

        <!-- Resultado -->
        @if(interaction && (interaction.status === 3 || interaction.status === 4)) {
          <div class="row mb-3">
            <div class="col-12">
              <label for="outcome" class="form-label">{{ '::Interaction:Outcome' | abpLocalization }}</label>
              <textarea id="outcome" class="form-control" formControlName="outcome" rows="2"
                        placeholder="{{ '::Interaction:OutcomePlaceholder' | abpLocalization }}"></textarea>
            </div>
          </div>
        }

        <!-- Opções Adicionais -->
        <div class="row mb-3">
          <div class="col-md-6">
            <div class="form-check">
              <input type="checkbox" id="isAllDay" class="form-check-input" formControlName="isAllDay">
              <label class="form-check-label" for="isAllDay">
                {{ '::Interaction:IsAllDay' | abpLocalization }}
              </label>
            </div>
          </div>
          <div class="col-md-6">
            <div class="form-check">
              <input type="checkbox" id="requiresReminder" class="form-check-input" formControlName="requiresReminder">
              <label class="form-check-label" for="requiresReminder">
                {{ '::Interaction:RequiresReminder' | abpLocalization }}
              </label>
            </div>
          </div>
        </div>

        <!-- Data do Lembrete -->
        @if(interactionForm.value.requiresReminder) {
          <div class="row mb-3">
            <div class="col-md-6">
              <label for="reminderDate" class="form-label">{{ '::Interaction:ReminderDate' | abpLocalization }}</label>
              <input type="datetime-local" id="reminderDate" class="form-control" formControlName="reminderDate">
            </div>
          </div>
        }

        <!-- Dados Adicionais -->
        <div class="row mb-3">
          <div class="col-12">
            <label for="additionalData" class="form-label">{{ '::Interaction:AdditionalData' | abpLocalization }}</label>
            <textarea id="additionalData" class="form-control" formControlName="additionalData" rows="2"
                      placeholder="{{ '::Interaction:AdditionalDataPlaceholder' | abpLocalization }}"></textarea>
          </div>
        </div>
      </form>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss()">
        {{ '::Cancel' | abpLocalization }}
      </button>
      <button type="button" class="btn btn-primary" (click)="saveInteraction()" [disabled]="interactionForm.invalid || isSaving">
        @if(isSaving) {
          <span class="spinner-border spinner-border-sm me-2"></span>
          {{ '::Interaction:Saving' | abpLocalization }}...
        } @else {
          {{ interaction ? ('::Save' | abpLocalization) : ('::Create' | abpLocalization) }}
        }
      </button>
    </div>
  `,
  styles: [`
    .form-label {
      font-weight: 500;
      color: #495057;
      font-size: 0.875rem;
    }

    .form-control, .form-select {
      border-color: #ced4da;
      font-size: 0.875rem;
    }

    .form-control:focus, .form-select:focus {
      border-color: #007bff;
      box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
    }

    .form-check {
      margin-bottom: 0.5rem;
    }

    .form-check-label {
      font-size: 0.875rem;
      color: #495057;
    }

    .invalid-feedback {
      font-size: 0.775rem;
    }

    .spinner-border-sm {
      width: 1rem;
      height: 1rem;
    }
  `]
})
export class InteractionFormComponent implements OnInit {
  @Input() interaction: InteractionDto | null = null;

  activeModal = inject(NgbActiveModal);
  interactionService = inject(SimpleInteractionService);
  clientService = inject(SimpleClientService);
  customerService = inject(CustomerService);
  localization = inject(LocalizationService);
  fb = inject(FormBuilder);

  interactionForm: FormGroup;
  isSaving = false;

  // Opções
  readonly typeOptions = interactionTypeOptions;
  readonly statusOptions = interactionStatusOptions;
  readonly priorityOptions = priorityOptions;

  // Dados para selects
  clients: ClientDto[] = [];
  customers: CustomerDto[] = [];
  selectedClientId: string | null = null;

  constructor() {
    this.interactionForm = this.fb.group({
      subject: ['', [Validators.required, Validators.maxLength(200)]],
      description: [''],
      type: [null, Validators.required],
      status: [InteractionStatus.Scheduled],
      priority: [Priority.Normal],
      scheduledDate: ['', Validators.required],
      startDate: [''],
      endDate: [''],
      durationMinutes: [0],
      location: [''],
      outcome: [''],
      clientId: [''],
      customerId: [''],
      ownerUserId: [''],
      isAllDay: [false],
      requiresReminder: [false],
      reminderDate: [''],
      additionalData: ['']
    });
  }

  ngOnInit(): void {
    this.loadClients();

    if (this.interaction) {
      this.populateForm();
    }

    // Set default scheduled date to tomorrow if creating new interaction
    if (!this.interaction) {
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      tomorrow.setHours(10, 0, 0, 0);
      this.interactionForm.patchValue({
        scheduledDate: tomorrow.toISOString().slice(0, 16)
      });
    }
  }

  populateForm(): void {
    if (!this.interaction) return;

    this.interactionForm.patchValue({
      subject: this.interaction.subject,
      description: this.interaction.description,
      type: this.interaction.type,
      status: this.interaction.status,
      priority: this.interaction.priority,
      scheduledDate: this.interaction.scheduledDate ? new Date(this.interaction.scheduledDate).toISOString().slice(0, 16) : '',
      startDate: this.interaction.startDate ? new Date(this.interaction.startDate).toISOString().slice(0, 16) : '',
      endDate: this.interaction.endDate ? new Date(this.interaction.endDate).toISOString().slice(0, 16) : '',
      durationMinutes: this.interaction.durationMinutes,
      location: this.interaction.location,
      outcome: this.interaction.outcome,
      clientId: this.interaction.clientId,
      customerId: this.interaction.customerId,
      ownerUserId: this.interaction.ownerUserId,
      isAllDay: this.interaction.isAllDay,
      requiresReminder: this.interaction.requiresReminder,
      reminderDate: this.interaction.reminderDate ? new Date(this.interaction.reminderDate).toISOString().slice(0, 16) : '',
      additionalData: this.interaction.additionalData
    });

    if (this.interaction.clientId) {
      this.selectedClientId = this.interaction.clientId;
      this.loadCustomers(this.interaction.clientId);
    }
  }

  loadClients(): void {
    this.clientService.getList({ skipCount: 0, maxResultCount: 1000 }).subscribe({
      next: (result) => {
        this.clients = result.items || [];
      },
      error: (error) => {
        console.error('Error loading clients:', error);
      }
    });
  }

  onClientChange(): void {
    const clientId = this.interactionForm.value.clientId;
    this.selectedClientId = clientId;

    if (clientId) {
      this.loadCustomers(clientId);
    } else {
      this.customers = [];
      this.interactionForm.patchValue({ customerId: '' });
    }
  }

  loadCustomers(clientId: string): void {
    this.customerService.getList({ skipCount: 0, maxResultCount: 1000, clientId, includeInactive: true }).subscribe({
      next: (result) => {
        this.customers = result.items || [];
      },
      error: (error) => {
        console.error('Error loading customers:', error);
      }
    });
  }

  saveInteraction(): void {
    if (this.interactionForm.invalid) return;

    this.isSaving = true;

    const formValue = this.interactionForm.value;

    // Remove undefined fields
    Object.keys(formValue).forEach(key => {
      if (formValue[key] === undefined || formValue[key] === '') {
        delete formValue[key];
      }
    });

    const saveObservable = this.interaction
      ? this.interactionService.update(this.interaction.id, formValue as CreateUpdateInteractionDto)
      : this.interactionService.create(formValue as CreateUpdateInteractionDto);

    saveObservable.pipe(
      // finalize(() => this.isSaving = false)
    ).subscribe({
      next: (result) => {
        this.isSaving = false;
        this.activeModal.close(result);
      },
      error: (error) => {
        this.isSaving = false;
        console.error('Error saving interaction:', error);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.interactionForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }

  getErrorMessage(fieldName: string): string {
    const field = this.interactionForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) {
      return this.localization.instant('::Interaction:FieldRequired');
    }
    if (field.errors['maxlength']) {
      return this.localization.instant('::Interaction:FieldTooLong', field.errors['maxlength'].requiredLength);
    }
    return this.localization.instant('::Interaction:FieldInvalid');
  }
}
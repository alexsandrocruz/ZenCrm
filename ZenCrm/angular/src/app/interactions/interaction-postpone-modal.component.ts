import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'app-interaction-postpone-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, LocalizationPipe],
  template: `
    <div class="modal-header">
      <h5 class="modal-title">{{ '::Interaction:PostponeInteraction' | abpLocalization }}</h5>
      <button type="button" class="btn-close" (click)="activeModal.dismiss()"></button>
    </div>
    <div class="modal-body">
      <p>{{ '::Interaction:EnterNewScheduledDateFor' | abpLocalization }}: <strong>{{ interactionSubject }}</strong></p>

      <form [formGroup]="postponeForm">
        <div class="mb-3">
          <label for="newDate" class="form-label">{{ '::Interaction:NewScheduledDate' | abpLocalization }}</label>
          <input
            type="datetime-local"
            id="newDate"
            class="form-control"
            formControlName="newDate"
            [class.is-invalid]="isFieldInvalid('newDate')"
          />
          @if (isFieldInvalid('newDate')) {
            <div class="invalid-feedback">
              {{ getErrorMessage('newDate') }}
            </div>
          }
        </div>

        <div class="mb-3">
          <label for="reason" class="form-label">{{ '::Interaction:PostponeReason' | abpLocalization }} ({{ '::Optional' | abpLocalization }})</label>
          <textarea
            id="reason"
            class="form-control"
            formControlName="reason"
            rows="3"
            [placeholder]="'::Interaction:EnterPostponeReason' | abpLocalization"
            [class.is-invalid]="isFieldInvalid('reason')"
          ></textarea>
          @if (isFieldInvalid('reason')) {
            <div class="invalid-feedback">
              {{ getErrorMessage('reason') }}
            </div>
          }
        </div>
      </form>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss()">
        {{ '::Cancel' | abpLocalization }}
      </button>
      <button type="button" class="btn btn-warning" (click)="postponeInteraction()" [disabled]="postponeForm.invalid">
        <i class="fa fa-clock me-1"></i>
        {{ '::Interaction:Postpone' | abpLocalization }}
      </button>
    </div>
  `,
  styles: [`
    .form-label {
      font-weight: 600;
      margin-bottom: 0.5rem;
    }

    .form-control {
      border-radius: 0.375rem;
    }

    .form-control:focus {
      border-color: #86b7fe;
      box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
    }

    .invalid-feedback {
      display: block;
    }
  `]
})
export class InteractionPostponeModalComponent {
  activeModal = inject(NgbActiveModal);
  fb = inject(FormBuilder);

  interactionSubject: string = '';
  postponeForm: FormGroup;

  constructor() {
    this.postponeForm = this.fb.group({
      newDate: ['', [Validators.required]],
      reason: ['', [Validators.maxLength(500)]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.postponeForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }

  getErrorMessage(fieldName: string): string {
    const field = this.postponeForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) {
      return this.getFieldRequiredMessage(fieldName);
    }
    if (field.errors['maxlength']) {
      return this.getFieldMaxLengthMessage(fieldName);
    }
    return 'Invalid value.';
  }

  private getFieldRequiredMessage(fieldName: string): string {
    const messages: { [key: string]: string } = {
      newDate: '::Interaction:NewDateRequired'
    };
    return messages[fieldName] || 'This field is required.';
  }

  private getFieldMaxLengthMessage(fieldName: string): string {
    const messages: { [key: string]: string } = {
      reason: '::Interaction:ReasonMaxLength'
    };
    const field = this.postponeForm.get(fieldName);
    const maxLength = field?.errors?.['maxlength']?.requiredLength || 500;
    return (messages[fieldName] || 'Maximum {0} characters allowed.').replace('{0}', maxLength.toString());
  }

  postponeInteraction(): void {
    if (this.postponeForm.valid) {
      const newDate = this.postponeForm.get('newDate')?.value;
      const reason = this.postponeForm.get('reason')?.value;

      // Convert datetime-local to Date object
      const date = new Date(newDate);
      if (!isNaN(date.getTime())) {
        this.activeModal.close({
          newDate: date,
          reason: reason || null
        });
      }
    }
  }
}
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'app-interaction-outcome-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, LocalizationPipe],
  template: `
    <div class="modal-header">
      <h5 class="modal-title">{{ '::Interaction:CompleteInteraction' | abpLocalization }}</h5>
      <button type="button" class="btn-close" (click)="activeModal.dismiss()"></button>
    </div>
    <div class="modal-body">
      <p>{{ '::Interaction:EnterOutcomeFor' | abpLocalization }}: <strong>{{ interactionSubject }}</strong></p>

      <form [formGroup]="outcomeForm">
        <div class="mb-3">
          <label for="outcome" class="form-label">{{ '::Interaction:Outcome' | abpLocalization }}</label>
          <textarea
            id="outcome"
            class="form-control"
            formControlName="outcome"
            rows="4"
            [placeholder]="'::Interaction:EnterOutcomeDescription' | abpLocalization"
            [class.is-invalid]="isFieldInvalid('outcome')"
          ></textarea>
          @if (isFieldInvalid('outcome')) {
            <div class="invalid-feedback">
              {{ getErrorMessage('outcome') }}
            </div>
          }
        </div>
      </form>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss()">
        {{ '::Cancel' | abpLocalization }}
      </button>
      <button type="button" class="btn btn-primary" (click)="saveOutcome()" [disabled]="outcomeForm.invalid">
        <i class="fa fa-check me-1"></i>
        {{ '::Interaction:Complete' | abpLocalization }}
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
export class InteractionOutcomeModalComponent {
  activeModal = inject(NgbActiveModal);
  fb = inject(FormBuilder);

  interactionSubject: string = '';
  outcomeForm: FormGroup;

  constructor() {
    this.outcomeForm = this.fb.group({
      outcome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(1000)]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.outcomeForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }

  getErrorMessage(fieldName: string): string {
    const field = this.outcomeForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) {
      return this.getFieldRequiredMessage(fieldName);
    }
    if (field.errors['minlength']) {
      return this.getFieldMinLengthMessage(fieldName);
    }
    if (field.errors['maxlength']) {
      return this.getFieldMaxLengthMessage(fieldName);
    }
    return 'Invalid value.';
  }

  private getFieldRequiredMessage(fieldName: string): string {
    const messages: { [key: string]: string } = {
      outcome: '::Interaction:OutcomeRequired'
    };
    return messages[fieldName] || 'This field is required.';
  }

  private getFieldMinLengthMessage(fieldName: string): string {
    const messages: { [key: string]: string } = {
      outcome: '::Interaction:OutcomeMinLength'
    };
    const field = this.outcomeForm.get(fieldName);
    const requiredLength = field?.errors?.['minlength']?.requiredLength || 3;
    return (messages[fieldName] || 'Minimum {0} characters required.').replace('{0}', requiredLength.toString());
  }

  private getFieldMaxLengthMessage(fieldName: string): string {
    const messages: { [key: string]: string } = {
      outcome: '::Interaction:OutcomeMaxLength'
    };
    const field = this.outcomeForm.get(fieldName);
    const maxLength = field?.errors?.['maxlength']?.requiredLength || 1000;
    return (messages[fieldName] || 'Maximum {0} characters allowed.').replace('{0}', maxLength.toString());
  }

  saveOutcome(): void {
    if (this.outcomeForm.valid) {
      const outcome = this.outcomeForm.get('outcome')?.value;
      this.activeModal.close(outcome);
    }
  }
}
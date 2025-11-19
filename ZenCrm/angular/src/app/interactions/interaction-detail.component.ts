import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { InteractionDto } from '../proxy/sales/models';
import { InteractionType } from '../proxy/sales/interaction-type.enum';
import { InteractionStatus } from '../proxy/sales/interaction-status.enum';
import { interactionTypeOptions, interactionStatusOptions, priorityOptions } from '../proxy/sales';
import { SimpleInteractionService } from '../services/simple-interaction.service';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-interaction-detail',
  standalone: true,
  imports: [CommonModule, LocalizationPipe],
  template: `
    <div class="modal-header">
      <h5 class="modal-title">{{ interaction?.subject || '::Interaction:InteractionDetails' | abpLocalization }}</h5>
      <button type="button" class="btn-close" (click)="activeModal.dismiss()"></button>
    </div>
    <div class="modal-body">
      @if(interaction) {
        <!-- Informações Principais -->
        <div class="row mb-3">
          <div class="col-md-6">
            <h6 class="text-muted">{{ '::Interaction:Subject' | abpLocalization }}</h6>
            <p class="fw-bold">{{ interaction.subject || '-' }}</p>
          </div>
          <div class="col-md-6">
            <h6 class="text-muted">{{ '::Interaction:Status' | abpLocalization }}</h6>
            <span class="badge {{ getStatusBadgeClass(interaction.status) }}">
              {{ getStatusDisplay(interaction.status) }}
            </span>
          </div>
        </div>

        <div class="row mb-3">
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Type' | abpLocalization }}</h6>
            <p>{{ getTypeDisplay(interaction.type) }}</p>
          </div>
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Priority' | abpLocalization }}</h6>
            <span class="badge {{ getPriorityBadgeClass(interaction.priority) }}">
              {{ getPriorityDisplay(interaction.priority) }}
            </span>
          </div>
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Duration' | abpLocalization }}</h6>
            <p>{{ interaction.durationMinutes || 0 }} {{ '::Interaction:Minutes' | abpLocalization }}</p>
          </div>
        </div>

        <!-- Datas -->
        <div class="row mb-3">
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:ScheduledDate' | abpLocalization }}</h6>
            <p>{{ formatDateTime(interaction.scheduledDate) }}</p>
          </div>
          @if(interaction.startDate) {
            <div class="col-md-4">
              <h6 class="text-muted">{{ '::Interaction:StartDate' | abpLocalization }}</h6>
              <p>{{ formatDateTime(interaction.startDate) }}</p>
            </div>
          }
          @if(interaction.endDate) {
            <div class="col-md-4">
              <h6 class="text-muted">{{ '::Interaction:EndDate' | abpLocalization }}</h6>
              <p>{{ formatDateTime(interaction.endDate) }}</p>
            </div>
          }
        </div>

        <!-- Relacionamentos -->
        <div class="row mb-3">
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Client' | abpLocalization }}</h6>
            <p>{{ interaction.clientName || '-' }}</p>
          </div>
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Customer' | abpLocalization }}</h6>
            <p>{{ interaction.customerName || '-' }}</p>
          </div>
          <div class="col-md-4">
            <h6 class="text-muted">{{ '::Interaction:Owner' | abpLocalization }}</h6>
            <p>{{ interaction.ownerUserName || '-' }}</p>
          </div>
        </div>

        <!-- Localização -->
        @if(interaction.location) {
          <div class="row mb-3">
            <div class="col-12">
              <h6 class="text-muted">{{ '::Interaction:Location' | abpLocalization }}</h6>
              <p>{{ interaction.location }}</p>
            </div>
          </div>
        }

        <!-- Descrição -->
        @if(interaction.description) {
          <div class="row mb-3">
            <div class="col-12">
              <h6 class="text-muted">{{ '::Interaction:Description' | abpLocalization }}</h6>
              <div class="border rounded p-3 bg-light">
                {{ interaction.description }}
              </div>
            </div>
          </div>
        }

        <!-- Outcome/Resultado -->
        @if(interaction.outcome) {
          <div class="row mb-3">
            <div class="col-12">
              <h6 class="text-muted">{{ '::Interaction:Outcome' | abpLocalization }}</h6>
              <div class="border rounded p-3 bg-light">
                {{ interaction.outcome }}
              </div>
            </div>
          </div>
        }

        <!-- Lembretes -->
        @if(interaction.requiresReminder) {
          <div class="row mb-3">
            <div class="col-12">
              <h6 class="text-muted">{{ '::Interaction:Reminder' | abpLocalization }}</h6>
              <div class="alert alert-info">
                <i class="fa fa-bell me-2"></i>
                @if(interaction.reminderDate) {
                  {{ '::Interaction:ReminderSetFor' | abpLocalization }} {{ formatDateTime(interaction.reminderDate) }}
                } @else {
                  {{ '::Interaction:ReminderEnabled' | abpLocalization }}
                }
              </div>
            </div>
          </div>
        }

        <!-- Informações de Sistema -->
        <div class="row">
          <div class="col-md-6">
            <h6 class="text-muted">{{ '::Interaction:CreationTime' | abpLocalization }}</h6>
            <p class="small">{{ formatDateTime(interaction.creationTime) }}</p>
          </div>
          <div class="col-md-6">
            <h6 class="text-muted">{{ '::Interaction:LastModificationTime' | abpLocalization }}</h6>
            <p class="small">{{ formatDateTime(interaction.lastModificationTime) }}</p>
          </div>
        </div>

        <!-- Actions disponíveis -->
        @if(showActions) {
          <div class="border-top pt-3 mt-3">
            <h6 class="text-muted mb-3">{{ '::Interaction:Actions' | abpLocalization }}</h6>
            <div class="d-flex flex-wrap gap-2">
              @if(interaction.status === InteractionStatus.Scheduled) {
                <button type="button" class="btn btn-success" (click)="startInteraction()">
                  <i class="fa fa-play me-1"></i>
                  {{ '::Interaction:Start' | abpLocalization }}
                </button>
                <button type="button" class="btn btn-primary" (click)="completeInteraction()">
                  <i class="fa fa-check me-1"></i>
                  {{ '::Interaction:Complete' | abpLocalization }}
                </button>
                <button type="button" class="btn btn-warning" (click)="postponeInteraction()">
                  <i class="fa fa-clock me-1"></i>
                  {{ '::Interaction:Postpone' | abpLocalization }}
                </button>
                <button type="button" class="btn btn-danger" (click)="cancelInteraction()">
                  <i class="fa fa-times me-1"></i>
                  {{ '::Interaction:Cancel' | abpLocalization }}
                </button>
              } @else if(interaction.status === InteractionStatus.InProgress) {
                <button type="button" class="btn btn-primary" (click)="completeInteraction()">
                  <i class="fa fa-check me-1"></i>
                  {{ '::Interaction:Complete' | abpLocalization }}
                </button>
                <button type="button" class="btn btn-warning" (click)="postponeInteraction()">
                  <i class="fa fa-clock me-1"></i>
                  {{ '::Interaction:Postpone' | abpLocalization }}
                </button>
                <button type="button" class="btn btn-danger" (click)="cancelInteraction()">
                  <i class="fa fa-times me-1"></i>
                  {{ '::Interaction:Cancel' | abpLocalization }}
                </button>
              } @else if(interaction.status === InteractionStatus.Completed) {
                <button type="button" class="btn btn-secondary" disabled>
                  <i class="fa fa-check me-1"></i>
                  {{ '::Interaction:Completed' | abpLocalization }}
                </button>
              }
            </div>
          </div>
        }
      } @else {
        <div class="text-center py-4">
          <i class="fa fa-exclamation-triangle fa-2x text-warning mb-3"></i>
          <h6>{{ '::Interaction:InteractionNotFound' | abpLocalization }}</h6>
        </div>
      }
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="activeModal.dismiss()">
        {{ '::Close' | abpLocalization }}
      </button>
    </div>
  `,
  styles: [`
    h6 {
      font-size: 0.875rem;
      font-weight: 600;
      margin-bottom: 0.25rem;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    p {
      margin-bottom: 0.5rem;
    }

    .border {
      border-color: #e9ecef !important;
    }

    .bg-light {
      background-color: #f8f9fa !important;
    }

    .badge {
      font-size: 0.75rem;
      padding: 0.375rem 0.75rem;
      font-weight: 500;
    }

    .btn {
      font-size: 0.875rem;
      padding: 0.5rem 1rem;
    }

    i {
      font-size: 0.875rem;
    }

    .small {
      font-size: 0.8rem;
      color: #6c757d;
    }
  `]
})
export class InteractionDetailComponent {
  @Input() interaction: InteractionDto | null = null;
  @Input() showActions: boolean = true;

  activeModal = inject(NgbActiveModal);
  interactionService = inject(SimpleInteractionService);
  localization = inject(LocalizationService);
  confirmation = inject(ConfirmationService);

  readonly typeOptions = interactionTypeOptions;
  readonly statusOptions = interactionStatusOptions;
  readonly priorityOptions = priorityOptions;

  getStatusBadgeClass(status?: InteractionStatus): string {
    switch (status) {
      case InteractionStatus.Scheduled:
        return 'bg-secondary';
      case InteractionStatus.InProgress:
        return 'bg-primary';
      case InteractionStatus.Completed:
        return 'bg-success';
      case InteractionStatus.Cancelled:
        return 'bg-danger';
      case InteractionStatus.Postponed:
        return 'bg-warning text-dark';
      case InteractionStatus.Failed:
        return 'bg-danger';
      case InteractionStatus.Pending:
        return 'bg-info';
      default:
        return 'bg-secondary';
    }
  }

  getPriorityBadgeClass(priority?: number): string {
    switch (priority) {
      case 1: // Low
        return 'bg-secondary';
      case 2: // Normal
        return 'bg-primary';
      case 3: // High
        return 'bg-warning text-dark';
      case 4: // Critical
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  getTypeDisplay(type?: InteractionType): string {
    if (!type) return '-';
    const option = this.typeOptions.find(opt => opt.value === type);
    return option ? this.localization.instant(`::Enum:InteractionType.${option.key}`) : type.toString();
  }

  getStatusDisplay(status?: InteractionStatus): string {
    if (!status) return '-';
    const option = this.statusOptions.find(opt => opt.value === status);
    return option ? this.localization.instant(`::Enum:InteractionStatus.${option.key}`) : status.toString();
  }

  getPriorityDisplay(priority?: number): string {
    if (!priority) return '-';
    const option = this.priorityOptions.find(opt => opt.value === priority);
    return option ? this.localization.instant(`::Enum:Priority.${option.key}`) : priority.toString();
  }

  formatDateTime(dateString?: string): string {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  startInteraction(): void {
    if (!this.interaction) return;

    this.confirmation.warn(
      '::Interaction:StartConfirmationMessage',
      '::AreYouSure',
      { messageLocalizationParams: [this.interaction.subject] }
    ).subscribe(status => {
      if (status) {
        this.interactionService.start(this.interaction.id).subscribe({
          next: (result) => {
            this.interaction = result;
            // Adicionar visualização de sucesso ou fechar modal
          },
          error: (error) => {
            console.error('Error starting interaction:', error);
          }
        });
      }
    });
  }

  async completeInteraction(): Promise<void> {
    if (!this.interaction) return;

    const outcome = prompt(this.localization.instant('::Interaction:EnterOutcome'));
    if (outcome !== null) {
      this.interactionService.complete(this.interaction.id, outcome).subscribe({
        next: (result) => {
          this.interaction = result;
        },
        error: (error) => {
          console.error('Error completing interaction:', error);
        }
      });
    }
  }

  async postponeInteraction(): Promise<void> {
    if (!this.interaction) return;

    const newDateString = prompt(this.localization.instant('::Interaction:EnterNewScheduledDate'));
    if (newDateString) {
      const newDate = new Date(newDateString);
      if (!isNaN(newDate.getTime())) {
        this.interactionService.postpone(this.interaction.id, newDate).subscribe({
          next: (result) => {
            this.interaction = result;
          },
          error: (error) => {
            console.error('Error postponing interaction:', error);
          }
        });
      } else {
        alert(this.localization.instant('::Interaction:InvalidDateFormat'));
      }
    }
  }

  cancelInteraction(): void {
    if (!this.interaction) return;

    this.confirmation.warn(
      '::Interaction:CancelConfirmationMessage',
      '::AreYouSure',
      { messageLocalizationParams: [this.interaction.subject] }
    ).subscribe(status => {
      if (status) {
        this.interactionService.cancel(this.interaction.id).subscribe({
          next: (result) => {
            this.interaction = result;
          },
          error: (error) => {
            console.error('Error canceling interaction:', error);
          }
        });
      }
    });
  }
}
import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { NgbModalModule, NgbModal, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import {
  InteractionDto,
  GetInteractionsInput,
  CreateUpdateInteractionDto
} from '../proxy/sales/models';
import { InteractionType } from '../proxy/sales/interaction-type.enum';
import { InteractionStatus } from '../proxy/sales/interaction-status.enum';
import { Priority } from '../proxy/sales/priority.enum';
import { interactionTypeOptions } from '../proxy/sales/interaction-type.enum';
import { interactionStatusOptions } from '../proxy/sales/interaction-status.enum';
import { priorityOptions } from '../proxy/sales/priority.enum';
import { SimpleInteractionService } from '../services/simple-interaction.service';
import { InteractionFormComponent } from './interaction-form.component';
import { InteractionDetailComponent } from './interaction-detail.component';

@Component({
  selector: 'app-client-interactions',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgbModalModule, NgbDropdownModule, LocalizationPipe],
  template: `
    <div class="tab-content">
      <!-- Header -->
      <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h6 class="mb-1">{{ '::Interaction:ClientInteractions' | abpLocalization }}</h6>
          <small class="text-muted">{{ clientName }}</small>
        </div>
        <button type="button" class="btn btn-primary btn-sm" (click)="createInteraction()">
          <i class="fa fa-plus me-1"></i>
          {{ '::Interaction:NewInteraction' | abpLocalization }}
        </button>
      </div>

      <!-- Filtros Rápidos -->
      <form [formGroup]="filterForm" class="row mb-3">
        <div class="col-md-3">
          <label class="form-label">{{ '::Interaction:Filter' | abpLocalization }}</label>
          <input type="text" class="form-control form-control-sm" formControlName="filter"
                 placeholder="{{ '::Interaction:FilterPlaceholder' | abpLocalization }}"
                 (input)="loadInteractions()">
        </div>
        <div class="col-md-2">
          <label class="form-label">{{ '::Interaction:Type' | abpLocalization }}</label>
          <select class="form-select form-select-sm" formControlName="type" (change)="loadInteractions()">
            <option value="">{{ '::Interaction:AllTypes' | abpLocalization }}</option>
            @for(option of typeOptions; track option.value) {
              <option [value]="option.value">{{ '::Enum:InteractionType.' + option.key | abpLocalization }}</option>
            }
          </select>
        </div>
        <div class="col-md-2">
          <label class="form-label">{{ '::Interaction:Status' | abpLocalization }}</label>
          <select class="form-select form-select-sm" formControlName="status" (change)="loadInteractions()">
            <option value="">{{ '::Interaction:AllStatuses' | abpLocalization }}</option>
            @for(option of statusOptions; track option.value) {
              <option [value]="option.value">{{ '::Enum:InteractionStatus.' + option.key | abpLocalization }}</option>
            }
          </select>
        </div>
        <div class="col-md-2">
          <label class="form-label">{{ '::Interaction:Priority' | abpLocalization }}</label>
          <select class="form-select form-select-sm" formControlName="priority" (change)="loadInteractions()">
            <option value="">{{ '::Interaction:AllPriorities' | abpLocalization }}</option>
            @for(option of priorityOptions; track option.value) {
              <option [value]="option.value">{{ '::Enum:Priority.' + option.key | abpLocalization }}</option>
            }
          </select>
        </div>
        <div class="col-md-3">
          <label class="form-label">&nbsp;</label>
          <div class="d-flex gap-2">
            <button type="button" class="btn btn-outline-secondary btn-sm" (click)="loadInteractions()">
              <i class="fa fa-sync me-1"></i>
              {{ '::Interaction:Refresh' | abpLocalization }}
            </button>
            <button type="button" class="btn btn-outline-secondary btn-sm" (click)="clearFilters()">
              {{ '::Interaction:ClearFilters' | abpLocalization }}
            </button>
          </div>
        </div>
      </form>

      <!-- Loading -->
      @if(isLoading) {
        <div class="text-center py-5">
          <div class="spinner-border spinner-border-sm text-primary" role="status">
            <span class="visually-hidden">{{ '::Interaction:Loading' | abpLocalization }}...</span>
          </div>
          <p class="mt-2 mb-0">{{ '::Interaction:Loading' | abpLocalization }}...</p>
        </div>
      }

      <!-- Lista de Interações -->
      @if(!isLoading && interactions.length > 0) {
        <div class="interaction-list">
          @for(interaction of interactions; track interaction.id) {
            <div class="card interaction-card mb-3"
                 [ngClass]="getStatusClass(interaction.status)"
                 [ngClass]="{ 'overdue': isOverdue(interaction) }">
              <div class="card-body">
                <div class="interaction-header">
                  <div class="interaction-title">{{ interaction.subject }}</div>
                  <div ngbDropdown>
                    <button class="btn btn-sm btn-outline-secondary dropdown-toggle" ngbDropdownToggle>
                      <i class="fa fa-ellipsis-v"></i>
                    </button>
                    <ul ngbDropdownMenu class="dropdown-menu dropdown-menu-end">
                      <li>
                        <a class="dropdown-item" (click)="viewInteraction(interaction)">
                          <i class="fa fa-eye me-2"></i>{{ '::Interaction:ViewDetails' | abpLocalization }}
                        </a>
                      </li>
                      <li>
                        <a class="dropdown-item" (click)="editInteraction(interaction)">
                          <i class="fa fa-edit me-2"></i>{{ '::Edit' | abpLocalization }}
                        </a>
                      </li>
                      @if(interaction.status === 1) {
                        <li><hr class="dropdown-divider"></li>
                        <li>
                          <a class="dropdown-item" (click)="startInteraction(interaction)">
                            <i class="fa fa-play me-2"></i>{{ '::Interaction:Start' | abpLocalization }}
                          </a>
                        </li>
                        <li>
                          <a class="dropdown-item" (click)="completeInteraction(interaction)">
                            <i class="fa fa-check me-2"></i>{{ '::Interaction:Complete' | abpLocalization }}
                          </a>
                        </li>
                        <li>
                          <a class="dropdown-item" (click)="cancelInteraction(interaction)">
                            <i class="fa fa-times me-2"></i>{{ '::Interaction:Cancel' | abpLocalization }}
                          </a>
                        </li>
                      }
                    </ul>
                  </div>
                </div>

                <div class="interaction-meta">
                  <span class="badge {{ getStatusBadgeClass(interaction.status) }}">
                    {{ getStatusDisplay(interaction.status) }}
                  </span>
                  <span class="badge {{ getPriorityBadgeClass(interaction.priority) }}">
                    {{ getPriorityDisplay(interaction.priority) }}
                  </span>
                  <span class="badge bg-light text-dark">
                    {{ getTypeDisplay(interaction.type) }}
                  </span>
                  @if(interaction.durationMinutes && interaction.durationMinutes > 0) {
                    <span class="text-muted">
                      <i class="fa fa-clock"></i> {{ interaction.durationMinutes }}m
                    </span>
                  }
                  @if(interaction.location) {
                    <span class="text-muted">
                      <i class="fa fa-map-marker-alt"></i> {{ interaction.location }}
                    </span>
                  }
                </div>

                @if(interaction.description) {
                  <div class="interaction-description">
                    {{ interaction.description | slice:0:150 }}{{ interaction.description.length > 150 ? '...' : '' }}
                  </div>
                }

                <div class="interaction-meta">
                  <span class="text-muted">
                    <i class="fa fa-calendar"></i> {{ formatDateTime(interaction.scheduledDate) }}
                  </span>
                  @if(interaction.ownerUserName) {
                    <span class="text-muted">
                      <i class="fa fa-user"></i> {{ interaction.ownerUserName }}
                    </span>
                  }
                  @if(interaction.customerName) {
                    <span class="text-muted">
                      <i class="fa fa-user-tie"></i> {{ interaction.customerName }}
                    </span>
                  }
                </div>

                @if(interaction.outcome) {
                  <div class="mt-2">
                    <small class="text-muted">{{ '::Interaction:Outcome' | abpLocalization }}:</small>
                    <div class="border rounded p-2 bg-light mt-1">
                      {{ interaction.outcome }}
                    </div>
                  </div>
                }
              </div>
            </div>
          }
        </div>

        <!-- Paginação -->
        <div class="d-flex justify-content-between align-items-center mt-3">
          <div class="text-muted">
            {{ totalCount }} {{ '::Interaction:Records' | abpLocalization }}
          </div>
          <nav>
            <ul class="pagination pagination-sm mb-0">
              <li class="page-item" [class.disabled]="currentPage === 1">
                <button class="page-link" (click)="onPageChange(currentPage - 1)">
                  <span aria-hidden="true">&laquo;</span>
                </button>
              </li>
              @for(page of getVisiblePages(); track page) {
                <li class="page-item" [class.active]="page === currentPage">
                  <button class="page-link" (click)="onPageChange(page)">{{ page }}</button>
                </li>
              }
              <li class="page-item" [class.disabled]="currentPage === totalPages">
                <button class="page-link" (click)="onPageChange(currentPage + 1)">
                  <span aria-hidden="true">&raquo;</span>
                </button>
              </li>
            </ul>
          </nav>
        </div>
      }

      <!-- Sem interações -->
      @if(!isLoading && interactions.length === 0) {
        <div class="text-center py-5">
          <i class="fa fa-comments fa-3x text-muted mb-3"></i>
          <h6 class="text-muted">{{ '::Interaction:NoInteractionsForClient' | abpLocalization }}</h6>
          <p class="text-muted">{{ '::Interaction:NoInteractionsForClientDescription' | abpLocalization }}</p>
          <button type="button" class="btn btn-primary" (click)="createInteraction()">
            <i class="fa fa-plus me-1"></i>
            {{ '::Interaction:CreateFirstInteraction' | abpLocalization }}
          </button>
        </div>
      }
    </div>
  `,
  styles: [`
    .interaction-list {
      max-height: 600px;
      overflow-y: auto;
    }

    .interaction-card {
      transition: all 0.3s ease;
      border-left: 4px solid transparent;

      &:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        transform: translateY(-2px);
      }

      &.overdue {
        background-color: #fff5f5;
        border-left-color: #dc3545;
      }
    }

    .interaction-header {
      display: flex;
      justify-content: space-between;
      align-items: start;
      margin-bottom: 0.5rem;
    }

    .interaction-title {
      font-weight: 600;
      color: #333;
      margin-bottom: 0.25rem;
      flex: 1;
      margin-right: 1rem;
    }

    .interaction-meta {
      display: flex;
      gap: 0.75rem;
      font-size: 0.75rem;
      color: #6c757d;
      flex-wrap: wrap;
      margin-bottom: 0.5rem;

      i {
        font-size: 0.625rem;
        margin-right: 0.25rem;
      }
    }

    .interaction-description {
      color: #666;
      font-size: 0.875rem;
      line-height: 1.4;
      margin-bottom: 0.5rem;
    }

    .badge {
      font-size: 0.625rem;
      padding: 0.25rem 0.5rem;
      font-weight: 500;
    }

    .btn-sm {
      padding: 0.25rem 0.5rem;
      font-size: 0.775rem;
    }

    .form-control-sm, .form-select-sm {
      font-size: 0.875rem;
    }

    .spinner-border-sm {
      width: 1.5rem;
      height: 1.5rem;
    }

    .bg-light {
      background-color: #f8f9fa !important;
      border-color: #e9ecef !important;
    }

    .page-link {
      font-size: 0.875rem;
      padding: 0.25rem 0.5rem;
    }

    /* Dropdown fixes */
    .dropdown-menu {
      z-index: 1050 !important;
      min-width: 180px;
      box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
      border: 1px solid rgba(0, 0, 0, 0.15);
    }

    /* Ensure dropdown appears above other elements */
    .dropdown {
      position: relative;
      z-index: 1040;
    }

    /* Fix dropdown positioning */
    [ngbDropdownMenu] {
      position: absolute !important;
      top: 100% !important;
      right: 0 !important;
      left: auto !important;
      z-index: 1050 !important;
      margin-top: 0.25rem;
      transform: translateX(0);
    }

    /* Ensure dropdown items are properly displayed */
    .dropdown-item {
      padding: 0.5rem 1rem;
      font-size: 0.875rem;
      white-space: nowrap;
    }

    /* Fix card overflow */
    .card-body {
      overflow: visible !important;
    }

    .interaction-card {
      overflow: visible !important;
    }
  `]
})
export class ClientInteractionsComponent implements OnInit {
  @Input() clientId!: string;
  @Input() clientName: string = '';

  interactionService = inject(SimpleInteractionService);
  localization = inject(LocalizationService);
  fb = inject(FormBuilder);
  private modalService = inject(NgbModal);
  private confirmation = inject(ConfirmationService);

  interactions: InteractionDto[] = [];
  isLoading = false;
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

  filterForm: FormGroup;

  readonly typeOptions = interactionTypeOptions;
  readonly statusOptions = interactionStatusOptions;
  readonly priorityOptions = priorityOptions;

  // Expose enum values for template use
  readonly InteractionStatus = InteractionStatus;

  constructor() {
    this.filterForm = this.fb.group({
      filter: [''],
      type: [null],
      status: [null],
      priority: [null]
    });
  }

  ngOnInit(): void {
    if (this.clientId) {
      this.loadInteractions();
    }
  }

  loadInteractions(): void {
    if (!this.clientId) return;

    this.isLoading = true;

    const input: GetInteractionsInput = {
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting: 'scheduledDate DESC',
      clientId: this.clientId,
      filter: this.filterForm.value.filter || undefined,
      type: this.filterForm.value.type || undefined,
      status: this.filterForm.value.status || undefined,
      priority: this.filterForm.value.priority || undefined,
      includeCompleted: true,
      includeCancelled: true
    };

    this.interactionService.getByClient(this.clientId, input)
      .subscribe({
        next: (result) => {
          this.interactions = result.items || [];
          this.totalCount = result.totalCount || 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading interactions:', error);
          this.interactions = [];
          this.totalCount = 0;
          this.totalPages = 0;
          this.isLoading = false;
        }
      });
  }

  clearFilters(): void {
    this.filterForm.reset({
      filter: '',
      type: null,
      status: null,
      priority: null
    });
    this.currentPage = 1;
    this.loadInteractions();
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadInteractions();
  }

  getVisiblePages(): number[] {
    const pages: number[] = [];
    const startPage = Math.max(1, this.currentPage - 2);
    const endPage = Math.min(this.totalPages, this.currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  createInteraction(): void {
    const modalRef = this.modalService.open(InteractionFormComponent, {
      size: 'lg',
      centered: true
    });

    // Pre-populate with client data
    const newInteraction: CreateUpdateInteractionDto = {
      subject: '',
      clientId: this.clientId,
      type: InteractionType.PhoneCall,
      status: InteractionStatus.Scheduled,
      priority: 2, // Normal
      scheduledDate: new Date().toISOString(),
      durationMinutes: 30,
      isAllDay: false,
      requiresReminder: false
    };

    modalRef.componentInstance.interaction = null;
    modalRef.componentInstance.clientId = this.clientId;

    modalRef.result.then(
      (result) => {
        if (result) {
          this.loadInteractions();
        }
      },
      () => {
        // Modal dismissed
      }
    );
  }

  getTypeDisplay(type?: InteractionType): string {
    if (!type) return '-';
    const option = this.typeOptions.find(opt => opt.value === type);
    return option ? this.localization.instant(`::Enum:InteractionType.${option.key}`) : type.toString();
  }

  getStatusDisplay(status?: number): string {
    if (!status) return '-';
    const option = this.statusOptions.find(opt => opt.value === status);
    return option ? this.localization.instant(`::Enum:InteractionStatus.${option.key}`) : status.toString();
  }

  getPriorityDisplay(priority?: number): string {
    if (!priority) return '-';
    const option = this.priorityOptions.find(opt => opt.value === priority);
    return option ? this.localization.instant(`::Enum:Priority.${option.key}`) : priority.toString();
  }

  getStatusBadgeClass(status?: number): string {
    switch (status) {
      case 1: // InteractionStatus.Scheduled
        return 'bg-secondary';
      case 2: // InteractionStatus.InProgress
        return 'bg-primary';
      case 3: // InteractionStatus.Completed
        return 'bg-success';
      case 4: // InteractionStatus.Cancelled
        return 'bg-danger';
      case 5: // InteractionStatus.Postponed
        return 'bg-warning text-dark';
      case 6: // InteractionStatus.Failed
        return 'bg-danger';
      case 7: // InteractionStatus.Pending
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

  getStatusClass(status?: number): string {
    switch (status) {
      case 1: // InteractionStatus.Scheduled
        return 'scheduled';
      case 2: // InteractionStatus.InProgress
        return 'in-progress';
      case 3: // InteractionStatus.Completed
        return 'completed';
      case 4: // InteractionStatus.Cancelled
        return 'cancelled';
      default:
        return '';
    }
  }

  isOverdue(interaction: InteractionDto): boolean {
    if (!interaction.scheduledDate) return false;
    if (interaction.status === 3 || interaction.status === 4) return false; // Completed || Cancelled

    const scheduledDate = new Date(interaction.scheduledDate);
    const now = new Date();
    return scheduledDate < now;
  }

  // Action Methods
  viewInteraction(interaction: InteractionDto): void {
    const modalRef = this.modalService.open(InteractionDetailComponent, {
      size: 'lg',
      centered: true
    });

    modalRef.componentInstance.interaction = interaction;
    modalRef.componentInstance.showActions = true;
  }

  editInteraction(interaction: InteractionDto): void {
    const modalRef = this.modalService.open(InteractionFormComponent, {
      size: 'lg',
      centered: true
    });

    modalRef.componentInstance.interaction = interaction;
    modalRef.componentInstance.clientId = this.clientId;

    modalRef.result.then(
      (result) => {
        if (result) {
          this.loadInteractions();
        }
      },
      () => {
        // Modal dismissed
      }
    );
  }

  deleteInteraction(interaction: InteractionDto): void {
    if (confirm(this.localization.instant('::Interaction:DeleteConfirmationMessage', interaction.subject))) {
      this.interactionService.delete(interaction.id).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error deleting interaction:', error);
          alert('Error deleting interaction');
        }
      });
    }
  }

  startInteraction(interaction: InteractionDto): void {
    this.interactionService.start(interaction.id).subscribe({
      next: () => {
        this.loadInteractions();
      },
      error: (error) => {
        console.error('Error starting interaction:', error);
        alert('Error starting interaction');
      }
    });
  }

  completeInteraction(interaction: InteractionDto): void {
    const outcome = prompt(this.localization.instant('::Interaction:EnterOutcomeFor', interaction.subject));
    if (outcome !== null && outcome.trim() !== '') {
      this.interactionService.complete(interaction.id, outcome.trim()).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error completing interaction:', error);
          alert('Error completing interaction');
        }
      });
    }
  }

  cancelInteraction(interaction: InteractionDto): void {
    if (confirm(this.localization.instant('::Interaction:CancelConfirmationMessage', interaction.subject))) {
      this.interactionService.cancel(interaction.id).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error canceling interaction:', error);
          alert('Error canceling interaction');
        }
      });
    }
  }

  formatDateTime(dateString?: string): string {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}
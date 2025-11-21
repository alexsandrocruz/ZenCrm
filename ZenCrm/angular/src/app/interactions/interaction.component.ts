import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbDropdownModule, NgbPaginationModule, NgbAlertModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe, LocalizationService } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

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

@Component({
  selector: 'app-interactions',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    NgbDropdownModule,
    NgbPaginationModule,
    NgbAlertModule,
    LocalizationPipe
  ],
  templateUrl: './interaction.component.html',
  styleUrls: ['./interaction.component.scss']
})
export class InteractionComponent implements OnInit {
  private readonly interactionService = inject(SimpleInteractionService);
  private readonly modalService = inject(NgbModal);
  private readonly localization = inject(LocalizationService);
  private readonly fb = inject(FormBuilder);

  interactions: InteractionDto[] = [];
  isLoading = false;
  totalCount = 0;

  filterForm: FormGroup;

  // Filtros
  readonly typeOptions = interactionTypeOptions;
  readonly statusOptions = interactionStatusOptions;
  readonly priorityOptions = priorityOptions;

  // Paginação
  currentPage = 1;
  pageSize = 10;

  // Colunas da tabela
  columns = [
    { name: this.localization.instant('::Interaction:Subject'), prop: 'subject' },
    { name: this.localization.instant('::Interaction:Type'), prop: 'type' },
    { name: this.localization.instant('::Interaction:Status'), prop: 'status' },
    { name: this.localization.instant('::Interaction:Priority'), prop: 'priority' },
    { name: this.localization.instant('::Interaction:ScheduledDate'), prop: 'scheduledDate' },
    { name: this.localization.instant('::Interaction:Client'), prop: 'clientName' },
    { name: this.localization.instant('::Interaction:Customer'), prop: 'customerName' },
    { name: this.localization.instant('::Interaction:Owner'), prop: 'ownerUserName' }
  ];

  constructor() {
    this.filterForm = this.fb.group({
      filter: [''],
      type: [null],
      status: [null],
      priority: [null],
      includeCompleted: [true],
      includeCancelled: [true]
    });
  }

  ngOnInit(): void {
    this.loadInteractions();
  }

  loadInteractions(): void {
    this.isLoading = true;

    const input: GetInteractionsInput = {
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting: 'scheduledDate DESC',
      filter: this.filterForm.value.filter || undefined,
      type: this.filterForm.value.type || undefined,
      status: this.filterForm.value.status || undefined,
      priority: this.filterForm.value.priority || undefined,
      includeCompleted: this.filterForm.value.includeCompleted,
      includeCancelled: this.filterForm.value.includeCancelled
    };

    this.interactionService.getList(input)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (result) => {
          this.interactions = result.items || [];
          this.totalCount = result.totalCount || 0;
        },
        error: (error) => {
          console.error('Error loading interactions:', error);
          this.interactions = [];
          this.totalCount = 0;
        }
      });
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadInteractions();
  }

  clearFilters(): void {
    this.filterForm.reset({
      filter: '',
      type: null,
      status: null,
      priority: null,
      includeCompleted: true,
      includeCancelled: true
    });
    this.loadInteractions();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadInteractions();
  }

  createInteraction(): void {
    // TODO: Implement modal for creating interaction
    console.log('Create interaction');
  }

  editInteraction(interaction: InteractionDto): void {
    // TODO: Implement modal for editing interaction
    console.log('Edit interaction:', interaction);
  }

  viewInteraction(interaction: InteractionDto): void {
    // TODO: Implement modal for viewing interaction
    console.log('View interaction:', interaction);
  }

  deleteInteraction(interaction: InteractionDto): void {
    if (confirm(this.localization.instant('::Interaction:DeleteConfirmationMessage', interaction.subject))) {
      this.interactionService.delete(interaction.id).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error deleting interaction:', error);
        }
      });
    }
  }

  startInteraction(interaction: InteractionDto): void {
    if (confirm(this.localization.instant('::Interaction:StartConfirmationMessage', interaction.subject))) {
      this.interactionService.start(interaction.id).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error starting interaction:', error);
        }
      });
    }
  }

  completeInteraction(interaction: InteractionDto): void {
    const outcome = prompt(this.localization.instant('::Interaction:EnterOutcome'));
    if (outcome !== null) {
      this.interactionService.complete(interaction.id, outcome).subscribe({
        next: () => {
          this.loadInteractions();
        },
        error: (error) => {
          console.error('Error completing interaction:', error);
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
        }
      });
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

  formatDateTime(dateString?: string): string {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}
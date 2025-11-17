import { CommonModule, formatDate } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ListService, LocalizationPipe, LocalizationService, PagedResultDto } from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  ModalCloseDirective,
  ModalComponent,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
} from '@abp/ng.theme.shared';
import { NgbDropdownModule, NgbModal, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  ClientDto,
  CreateUpdateSalesOpportunityDto,
  GetSalesLeadsInput,
  GetSalesOpportunitiesInput,
  PipelineStage,
  Priority,
  SalesLeadDto,
  SalesOpportunityDto,
  pipelineStageOptions,
  priorityOptions,
} from '../proxy/sales';
import { ClientService } from '../proxy/sales';
import { SalesLeadService } from '../proxy/sales';
import { SalesOpportunityService } from '../proxy/sales';
import { UserSelectionModalComponent } from '../clients/user-selection-modal.component';

import type { UserData } from '@abp/ng.identity/proxy';

@Component({
  selector: 'app-opportunities',
  templateUrl: './opportunities.component.html',
  styleUrls: ['./opportunities.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    NgxDatatableModule,
    NgbDropdownModule,
    NgbModalModule,
    ModalComponent,
    ModalCloseDirective,
    NgxDatatableListDirective,
    NgxDatatableDefaultDirective,
    LocalizationPipe,
    UserSelectionModalComponent,
  ],
  providers: [ListService],
})
export class OpportunitiesComponent implements OnInit {
  public readonly list = inject(ListService);
  private readonly fb = inject(FormBuilder);
  private readonly opportunityService = inject(SalesOpportunityService);
  private readonly confirmation = inject(ConfirmationService);
  private readonly clientService = inject(ClientService);
  private readonly salesLeadService = inject(SalesLeadService);
  private readonly modalService = inject(NgbModal);
  private readonly localization = inject(LocalizationService);

  opportunities = { items: [], totalCount: 0 } as PagedResultDto<SalesOpportunityDto>;
  selectedOpportunity = {} as SalesOpportunityDto;
  form: FormGroup;
  isModalOpen = false;

  filter = '';
  stageFilter?: PipelineStage;
  priorityFilter?: Priority;
  clientFilter: string | null = null;

  stageOptions = pipelineStageOptions;
  priorityOptions = priorityOptions;
  clientsLookup: ClientDto[] = [];
  salesLeads: SalesLeadDto[] = [];
  ownerDisplayName = '';

  ngOnInit(): void {
    const streamCreator = (query: GetSalesOpportunitiesInput) =>
      this.opportunityService.getList({
        ...query,
        filter: this.filter?.trim() || undefined,
        stage: this.stageFilter,
        priority: this.priorityFilter,
        clientId: this.clientFilter || undefined,
      });

    this.list.hookToQuery(streamCreator).subscribe(response => {
      this.opportunities = response;
    });

    this.loadLookups();
  }

  private loadLookups(): void {
    this.clientService
      .getList({ skipCount: 0, maxResultCount: 100, sorting: 'name' })
      .subscribe(result => (this.clientsLookup = result.items));

    const leadInput: GetSalesLeadsInput = {
      skipCount: 0,
      maxResultCount: 100,
      sorting: 'creationTime DESC',
      includeInactive: false,
      converted: false,
    };

    this.salesLeadService.getList(leadInput).subscribe(result => (this.salesLeads = result.items));
  }

  createOpportunity(): void {
    this.selectedOpportunity = {} as SalesOpportunityDto;
    this.ownerDisplayName = '';
    this.buildForm();
    this.isModalOpen = true;
  }

  editOpportunity(id: string): void {
    this.opportunityService.get(id).subscribe(opportunity => {
      this.selectedOpportunity = opportunity;
      this.ownerDisplayName = opportunity.ownerUserName || '';
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  deleteOpportunity(id: string): void {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.opportunityService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm(): void {
    this.form = this.fb.group({
      name: [this.selectedOpportunity.name || '', [Validators.required, Validators.maxLength(256)]],
      description: [this.selectedOpportunity.description || '', [Validators.maxLength(2000)]],
      estimatedValue: [
        this.selectedOpportunity.estimatedValue ?? 0,
        [Validators.required, Validators.min(0)],
      ],
      priority: [this.selectedOpportunity.priority ?? Priority.Normal, Validators.required],
      expectedCloseDate: [
        this.formatDateInput(this.selectedOpportunity.expectedCloseDate),
        Validators.required,
      ],
      salesLeadId: [this.selectedOpportunity.salesLeadId || null, Validators.required],
      clientId: [this.selectedOpportunity.clientId || null],
      ownerUserId: [this.selectedOpportunity.ownerUserId || null, Validators.required],
      competitor: [this.selectedOpportunity.competitor || '', Validators.maxLength(512)],
      parentOpportunityId: [this.selectedOpportunity.parentOpportunityId || null],
      isActive: [this.selectedOpportunity.isActive ?? true],
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const payload: CreateUpdateSalesOpportunityDto = {
      name: formValue.name,
      description: formValue.description || undefined,
      estimatedValue: Number(formValue.estimatedValue || 0),
      priority: formValue.priority,
      expectedCloseDate: formValue.expectedCloseDate,
      salesLeadId: formValue.salesLeadId,
      clientId: formValue.clientId || undefined,
      ownerUserId: formValue.ownerUserId,
      competitor: formValue.competitor || undefined,
      parentOpportunityId: formValue.parentOpportunityId || undefined,
      isActive: formValue.isActive ?? true,
    };

    const request$ = this.selectedOpportunity.id
      ? this.opportunityService.update(this.selectedOpportunity.id, payload)
      : this.opportunityService.create(payload);

    request$.subscribe(() => {
      this.isModalOpen = false;
      this.list.get();
    });
  }

  clearFilters(): void {
    this.filter = '';
    this.stageFilter = undefined;
    this.priorityFilter = undefined;
    this.clientFilter = null;
    this.list.get();
  }

  trackByOpportunity(_index: number, item: SalesOpportunityDto): string {
    return item.id;
  }

  getStageLabel(stage?: PipelineStage): string {
    const option = this.stageOptions.find(x => x.value === stage);
    if (!option) {
      return 'N/A';
    }

    return this.translateEnumKey('PipelineStage', option.key);
  }

  getPriorityLabel(priority?: Priority): string {
    const option = this.priorityOptions.find(x => x.value === priority);
    if (!option) {
      return 'N/A';
    }

    return this.translateEnumKey('Priority', option.key);
  }

  private translateEnumKey(enumName: string, key: string): string {
    const localizationKey = `::Enum:${enumName}.${key}`;
    const localizedValue = this.localization.instant(localizationKey);
    return localizedValue && localizedValue !== localizationKey ? localizedValue : key;
  }

  getStageBadgeClass(stage?: PipelineStage): string {
    switch (stage) {
      case PipelineStage.Won:
        return 'bg-success';
      case PipelineStage.Lost:
        return 'bg-danger';
      case PipelineStage.Negotiation:
      case PipelineStage.Closing:
        return 'bg-primary';
      case PipelineStage.Proposal:
      case PipelineStage.ProposalSent:
        return 'bg-info';
      default:
        return 'bg-secondary';
    }
  }

  onFilterChange(): void {
    this.list.get();
  }

  openOwnerSelection(): void {
    if (!this.form) {
      return;
    }

    const modalRef = this.modalService.open(UserSelectionModalComponent, {
      size: 'lg',
      centered: true,
    });

    modalRef.componentInstance.title = 'Select Opportunity Owner';
    modalRef.componentInstance.selectedUserId = this.form.get('ownerUserId')?.value;

    modalRef.result
      .then((user: UserData) => {
        if (user) {
          this.form.get('ownerUserId')?.setValue(user.id);
          this.ownerDisplayName = this.getUserDisplayName(user);
        }
      })
      .catch(() => undefined);
  }

  clearOwnerSelection(): void {
    this.form.get('ownerUserId')?.setValue(null);
    this.ownerDisplayName = '';
  }

  private getUserDisplayName(user: UserData): string {
    if (!user) {
      return '';
    }
    if (user.name) {
      return `${user.name} (${user.userName || user.email || ''})`.trim();
    }
    return user.userName || user.email || '';
  }

  private formatDateInput(value?: string): string {
    return value ? formatDate(value, 'yyyy-MM-dd', 'en-US') : '';
  }
}

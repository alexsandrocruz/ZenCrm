import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { NgbDatepickerModule, NgbDropdownModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  ListService,
  PagedResultDto,
  LocalizationPipe,
  PermissionDirective,
  AutofocusDirective
} from '@abp/ng.core';
import {
  ConfirmationService,
  Confirmation,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
  ModalCloseDirective,
  ModalComponent
} from '@abp/ng.theme.shared';
import {
  SalesLeadDto,
  CreateUpdateSalesLeadDto,
  GetSalesLeadsInput,
  LeadSource,
  LeadStatus,
  leadSourceOptions,
  leadStatusOptions
} from '../proxy/sales';
import { SalesLeadService } from '../proxy/sales';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSelectionModalComponent } from '../clients/user-selection-modal.component';
import { UserSearchService } from '../services/user-search.service';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, startWith, tap } from 'rxjs/operators';

@Component({
  selector: 'app-sales-lead',
  templateUrl: './sales-lead.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    NgbDatepickerModule,
    NgxDatatableModule,
    NgbDropdownModule,
    NgbModalModule,
    ModalComponent,
    AutofocusDirective,
    NgxDatatableListDirective,
    NgxDatatableDefaultDirective,
    PermissionDirective,
    ModalCloseDirective,
    LocalizationPipe,
    NgxMaskDirective,
    UserSelectionModalComponent
  ],
  providers: [ListService, provideNgxMask()],
})
export class SalesLeadComponent implements OnInit {
  public readonly list = inject(ListService);
  private salesLeadService = inject(SalesLeadService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);
  private router = inject(Router);
  private modalService = inject(NgbModal);
  private userSearchService = inject(UserSearchService);

  leads = { items: [], totalCount: 0 } as PagedResultDto<SalesLeadDto>;
  selectedLead = {} as SalesLeadDto;
  form: FormGroup;
  leadSources = leadSourceOptions;
  leadStatuses = leadStatusOptions;
  isModalOpen = false;

  // User search properties
  userSearchQuery = '';
  userSearchSubject = new Subject<string>();
  filteredUsers$: Observable<any[]> = of([]);

  ngOnInit() {
    const leadStreamCreator = (query: GetSalesLeadsInput) => this.salesLeadService.getList(query);

    this.list.hookToQuery(leadStreamCreator).subscribe(response => {
      this.leads = response;
    });

    this.setupUserSearch();
  }

  createLead() {
    this.selectedLead = {} as SalesLeadDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editLead(id: string) {
    this.salesLeadService.get(id).subscribe(lead => {
      this.selectedLead = lead;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.salesLeadService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  assignToUser(id: string) {
    const lead = this.leads.items.find(l => l.id === id);
    if (!lead) {
      console.error('Lead not found:', id);
      return;
    }

    const modalRef = this.modalService.open(UserSelectionModalComponent, {
      size: 'md',
      centered: true
    });

    modalRef.componentInstance.title = 'Assign Lead to User';
    modalRef.componentInstance.selectedUserId = lead.assignedUserId;

    modalRef.result.then((selectedUser) => {
      if (selectedUser) {
        this.salesLeadService.assignToUser(id, selectedUser.id).subscribe({
          next: () => {
            this.list.get();
            console.log(`Lead ${lead.firstName} ${lead.lastName} assigned to user ${selectedUser.name}`);
          },
          error: (err) => {
            console.error('Error assigning lead to user:', err);
          }
        });
      }
    }).catch(() => {
      console.log('User assignment cancelled');
    });
  }

  convertToOpportunity(id: string) {
    this.confirmation.warn('::AreYouSureToConvertToOpportunity', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.salesLeadService.convertToOpportunity(id).subscribe(() => {
          this.list.get();
          console.log(`Lead converted to opportunity successfully`);
        });
      }
    });
  }

  updateStatus(id: string, status: LeadStatus) {
    this.salesLeadService.updateStatus(id, status).subscribe(() => {
      this.list.get();
      console.log(`Lead status updated to ${status}`);
    });
  }

  buildForm() {
    this.form = this.fb.group({
      firstName: [this.selectedLead.firstName || '', Validators.required],
      lastName: [this.selectedLead.lastName || '', Validators.required],
      email: [this.selectedLead.email || '', [Validators.email, Validators.maxLength(256)]],
      phone: [this.selectedLead.phone || '', Validators.maxLength(32)],
      mobilePhone: [this.selectedLead.mobilePhone || '', Validators.maxLength(32)],
      company: [this.selectedLead.company || '', Validators.maxLength(256)],
      jobTitle: [this.selectedLead.jobTitle || '', Validators.maxLength(256)],
      source: [this.selectedLead.source || null],
      status: [this.selectedLead.status || null],
      description: [this.selectedLead.description || ''],
      assignedUserId: [this.selectedLead.assignedUserId || ''],
      estimatedValue: [this.selectedLead.estimatedValue || 0],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const formValue = this.form.value;
    const requestData: CreateUpdateSalesLeadDto = {
      ...formValue,
      // Converter enums de string para número
      source: formValue.source ? parseInt(formValue.source) : undefined,
      status: formValue.status ? parseInt(formValue.status) : undefined,
      estimatedValue: parseFloat(formValue.estimatedValue) || 0,
    };

    console.log('🔍 SalesLead - Dados do formulário:', formValue);
    console.log('🔍 SalesLead - requestData enviado:', requestData);
    console.log('🔍 SalesLead - Estamos criando ou atualizando?', this.selectedLead.id ? 'Atualizando' : 'Criando');

    let request = this.salesLeadService.create(requestData);
    if (this.selectedLead.id) {
      request = this.salesLeadService.update(this.selectedLead.id, requestData);
    }

    request.subscribe({
      next: (response) => {
        console.log('✅ SalesLead - Sucesso:', response);
        this.isModalOpen = false;
        this.form.reset();
        this.list.get();
      },
      error: (error) => {
        console.error('❌ SalesLead - Erro detalhado:', error);
        console.error('❌ SalesLead - Status:', error.status);
        console.error('❌ SalesLead - Error:', error.error);
      }
    });
  }

  getSourceLabel(source: LeadSource | null | undefined): string {
    if (source === null || source === undefined) {
      return '-';
    }
    const src = this.leadSources.find(s => s.value === source);
    return src ? src.key || src.value.toString() : source.toString();
  }

  getStatusLabel(status: LeadStatus | null | undefined): string {
    if (status === null || status === undefined) {
      return '-';
    }
    const stat = this.leadStatuses.find(s => s.value === status);
    return stat ? stat.key || stat.value.toString() : status.toString();
  }

  // User search methods
  setupUserSearch(): void {
    this.filteredUsers$ = this.userSearchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        if (!query || query.trim().length < 2) {
          return of([]);
        }
        return this.userSearchService.searchUsers(query.trim());
      })
    );
  }

  onUserInput(event: any): void {
    const value = event.target ? event.target.value : event;
    this.userSearchQuery = value;
    this.userSearchSubject.next(value);

    if (!value || value.trim() === '') {
      this.form.get('assignedUserId')?.setValue(null);
    }
  }

  onUserKeyup(event: any): void {
    this.onUserInput(event);
  }

  onUserSelect(event: any): void {
    const selectedValue = event.target.value;

    if (!selectedValue || selectedValue.trim() === '') {
      this.form.get('assignedUserId')?.setValue(null);
      this.userSearchQuery = '';
      return;
    }

    this.filteredUsers$.subscribe(users => {
      const matchedUser = users.find(user =>
        user.displayName === selectedValue ||
        user.userName === selectedValue ||
        `${user.name} ${user.surname}` === selectedValue ||
        user.email === selectedValue
      );

      if (matchedUser) {
        this.form.get('assignedUserId')?.setValue(matchedUser.id);
        this.userSearchQuery = selectedValue;
        console.log('Lead user selected:', matchedUser);
      } else {
        this.form.get('assignedUserId')?.setValue(null);
        this.userSearchQuery = '';
        console.log('No matching user found for:', selectedValue);
      }
    });
  }

  getUserDisplayName(): string {
    const userId = this.form.get('assignedUserId')?.value;
    if (!userId) return '';
    return this.userSearchQuery || '';
  }
}
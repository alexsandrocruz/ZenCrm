import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { formatDate } from '@angular/common';
import { NgbDatepickerModule, NgbDateStruct, NgbDropdownModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
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
  ClientService,
  ClientDto,
  CreateUpdateClientDto,
  GetClientsInput,
  clientTypeOptions,
  clientIndustryOptions
} from '../proxy/clients';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSelectionModalComponent } from './user-selection-modal.component';
import { UserSearchService } from '../services/user-search.service';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, startWith, tap } from 'rxjs/operators';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
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
export class ClientComponent implements OnInit {
  public readonly list = inject(ListService);
  private clientService = inject(ClientService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);
  private router = inject(Router);
  private modalService = inject(NgbModal);
  private userSearchService = inject(UserSearchService);

  clients = { items: [], totalCount: 0 } as PagedResultDto<ClientDto>;
  selectedClient = {} as ClientDto;
  form: FormGroup;
  clientTypes = clientTypeOptions;
  clientIndustries = clientIndustryOptions;
  isModalOpen = false;

  // User search properties
  userSearchQuery = '';
  userSearchSubject = new Subject<string>();
  filteredUsers$: Observable<any[]> = of([]);

  ngOnInit() {
    const clientStreamCreator = (query: GetClientsInput) => this.clientService.getList(query);

    this.list.hookToQuery(clientStreamCreator).subscribe(response => {
      this.clients = response;
    });

    this.setupUserSearch();
  }

  createClient() {
    this.selectedClient = {} as ClientDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editClient(id: string) {
    this.clientService.get(id).subscribe(client => {
      this.selectedClient = client;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.clientService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  assignToUser(id: string) {
    const client = this.clients.items.find(c => c.id === id);
    if (!client) {
      console.error('Client not found:', id);
      return;
    }

    const modalRef = this.modalService.open(UserSelectionModalComponent, {
      size: 'md',
      centered: true
    });

    modalRef.componentInstance.title = 'Assign Client to User';
    modalRef.componentInstance.selectedUserId = client.assignedUserId;

    modalRef.result.then((selectedUser) => {
      if (selectedUser) {
        // Update the client with the selected user
        const updateData = { ...client, assignedUserId: selectedUser.id };
        this.clientService.update(id, updateData).subscribe({
          next: () => {
            // Refresh the list to show updated assignment
            this.list.get();
            console.log(`Client ${client.name} assigned to user ${selectedUser.name}`);
          },
          error: (err) => {
            console.error('Error assigning client to user:', err);
          }
        });
      }
    }).catch(() => {
      // Modal was dismissed
      console.log('User assignment cancelled');
    });
  }

  viewClientDetails(id: string) {
    this.router.navigate(['/crm/clients', id]);
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedClient.name || '', Validators.required],
      clientType: [this.selectedClient.clientType || null, Validators.required],
      industry: [this.selectedClient.industry || null],
      website: [this.selectedClient.website || ''],
      phone: [this.selectedClient.phone || ''],
      email: [this.selectedClient.email || ''],
      address: [this.selectedClient.address || ''],
      city: [this.selectedClient.city || ''],
      state: [this.selectedClient.state || ''],
      country: [this.selectedClient.country || ''],
      postalCode: [this.selectedClient.postalCode || ''],
      description: [this.selectedClient.description || ''],
      annualRevenue: [this.selectedClient.annualRevenue || null],
      numberOfEmployees: [this.selectedClient.numberOfEmployees || null],
      assignedUserId: [this.selectedClient.assignedUserId || ''],
      isActive: [this.selectedClient.isActive ?? true],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const formValue = this.form.value;

    // Converter annualRevenue para decimal ou null se vazio/zero
    const annualRevenue = formValue.annualRevenue && formValue.annualRevenue !== 0 && formValue.annualRevenue !== '0'
      ? parseFloat(formValue.annualRevenue.toString().replace(/\./g, '').replace(',', '.'))
      : null;

    // Converter numberOfEmployees para int ou null se vazio/zero
    const numberOfEmployees = formValue.numberOfEmployees && formValue.numberOfEmployees !== 0 && formValue.numberOfEmployees !== '0'
      ? parseInt(formValue.numberOfEmployees.toString())
      : null;

    const requestData: CreateUpdateClientDto = {
      ...formValue,
      annualRevenue: annualRevenue,
      numberOfEmployees: numberOfEmployees,
    };

    let request = this.clientService.create(requestData);
    if (this.selectedClient.id) {
      request = this.clientService.update(this.selectedClient.id, requestData);
    }

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  getClientTypeLabel(clientType: number | null | undefined): string {
    if (clientType === null || clientType === undefined) {
      return '-';
    }
    const type = this.clientTypes.find(t => t.value === clientType);
    return type ? type.label : clientType.toString();
  }

  getIndustryLabel(industry: number | null | undefined): string {
    if (industry === null || industry === undefined) {
      return '-';
    }
    const ind = this.clientIndustries.find(i => i.value === industry);
    return ind ? ind.label : '-';
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

    // Se o input for limpo, limpa o assignedUserId
    if (!value || value.trim() === '') {
      this.form.get('assignedUserId')?.setValue(null);
    }
  }

  onUserKeyup(event: any): void {
    // Pode ser usado para atalhos de teclado se necessário
  }

  onUserSelect(event: any): void {
    // Para datalist, precisamos encontrar o usuário pelo value
    const selectedValue = event.target.value;

    // Se o campo foi limpo, limpa o ID também
    if (!selectedValue || selectedValue.trim() === '') {
      this.form.get('assignedUserId')?.setValue(null);
      this.userSearchQuery = '';
      return;
    }

    // Buscar usuário correspondente nos resultados
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
        console.log('Client user selected:', matchedUser);
      } else {
        // Se não encontrar correspondência, limpa
        this.form.get('assignedUserId')?.setValue(null);
        this.userSearchQuery = '';
        console.log('No matching user found for:', selectedValue);
      }
    });
  }

  getUserDisplayName(): string {
    const userId = this.form.get('assignedUserId')?.value;
    if (!userId) return '';

    // Para simplificar, vamos usar o valor atual do input
    return this.userSearchQuery || '';
  }
}
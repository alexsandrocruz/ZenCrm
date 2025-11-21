import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { NgbDropdownModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  ListService,
  PagedResultDto,
  LocalizationPipe,
  LocalizationService,
  PermissionDirective,
  AutofocusDirective
} from '@abp/ng.core';
import { AsyncPipe } from '@angular/common';
import {
  ConfirmationService,
  Confirmation,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
  ModalCloseDirective,
  ModalComponent
} from '@abp/ng.theme.shared';
import {
  CustomerService,
  CustomerDto,
  CreateUpdateCustomerDto,
  GetCustomersInput
} from '../proxy/sales';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { ClientSearchService } from '../services/client-search.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserSelectionModalComponent } from '../clients/user-selection-modal.component';
import { UserSearchService } from '../services/user-search.service';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, startWith, tap } from 'rxjs/operators';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
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
    AsyncPipe,
    UserSelectionModalComponent
  ],
  providers: [ListService, provideNgxMask()],
})
export class CustomerComponent implements OnInit {
  public readonly list = inject(ListService);
  private customerService = inject(CustomerService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);
  private clientSearchService = inject(ClientSearchService);
  private modalService = inject(NgbModal);
  private userSearchService = inject(UserSearchService);
  private localization = inject(LocalizationService);

  customers = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  selectedCustomer = {} as CustomerDto;
  form: FormGroup;
  isModalOpen = false;

  // Client search properties
  clientSearchQuery = '';
  clientSearchSubject = new Subject<string>();
  filteredClients$: Observable<{ id: string, name: string }[]> = of([]);
  selectedClientName = '';

  // User search properties
  userSearchQuery = '';
  userSearchSubject = new Subject<string>();
  filteredUsers$: Observable<any[]> = of([]);
  selectedUserName = '';

  ngOnInit() {
    const customerStreamCreator = (query: GetCustomersInput) => this.customerService.getList(query);

    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.customers = response;
    });

    this.setupClientSearch();
    this.setupUserSearch();
  }

  createCustomer() {
    this.selectedCustomer = {} as CustomerDto;
    this.buildForm();
    this.initializeClientSearch();
    this.isModalOpen = true;
  }

  editCustomer(id: string) {
    this.customerService.get(id).subscribe(customer => {
      this.selectedCustomer = customer;
      this.buildForm();
      this.initializeClientSearch();
      this.isModalOpen = true;
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.customerService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  assignToUser(id: string) {
    const customer = this.customers.items.find(c => c.id === id);
    if (!customer) {
      console.error('Customer not found:', id);
      return;
    }

    const modalRef = this.modalService.open(UserSelectionModalComponent, {
      size: 'md',
      centered: true
    });

    modalRef.componentInstance.title = this.localization.instant('::Customer:AssignCustomerToUser');
    modalRef.componentInstance.selectedUserId = customer.assignedUserId;

    modalRef.result.then((selectedUser) => {
      if (selectedUser) {
        // Update the customer with the selected user
        const updateData: CreateUpdateCustomerDto = {
          firstName: customer.firstName,
          lastName: customer.lastName,
          email: customer.email,
          phone: customer.phone,
          mobilePhone: customer.mobilePhone,
          jobTitle: customer.jobTitle,
          department: customer.department,
          notes: customer.notes,
          isActive: customer.isActive,
          isPrimaryContact: customer.isPrimaryContact,
          isKeyDecisionMaker: customer.isKeyDecisionMaker,
          clientId: customer.clientId,
          assignedUserId: selectedUser.id
        };
        this.customerService.update(id, updateData).subscribe({
          next: () => {
            // Refresh the list to show updated assignment
            this.list.get();
            console.log(`Customer ${customer.firstName} ${customer.lastName} assigned to user ${selectedUser.name || selectedUser.userName}`);
          },
          error: (err) => {
            console.error('Error assigning customer to user:', err);
          }
        });
      }
    }).catch(() => {
      // Modal was dismissed
      console.log('User assignment cancelled');
    });
  }

  setupClientSearch() {
    console.log('Setting up client search...');

    this.filteredClients$ = this.clientSearchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        console.log('Searching for clients with query:', query);
        this.clientSearchQuery = query;
        return this.clientSearchService.searchClientsByName(query);
      }),
      tap(results => console.log('Search results in setupClientSearch:', results))
    );
  }

  onClientInput(event: Event) {
    const input = event.target as HTMLInputElement;
    this.selectedClientName = input.value;
    console.log('onClientInput - value:', input.value);

    // Trigger the search
    this.clientSearchSubject.next(input.value);

    // Handle client selection after a delay to allow search results to update
    setTimeout(() => {
      this.findAndSetClient(input.value);
    }, 350);
  }

  onClientKeyup(event: KeyboardEvent) {
    const input = event.target as HTMLInputElement;
    console.log('onClientKeyup - value:', input.value);

    // Trigger the search
    this.clientSearchSubject.next(input.value);
  }

  private findAndSetClient(clientName: string) {
    // Find matching client in the current search results
    this.filteredClients$.subscribe(clients => {
      const matchedClient = clients.find(c => c.name === clientName);

      if (matchedClient) {
        console.log('Setting clientId to:', matchedClient.id);
        this.form.get('clientId')?.setValue(matchedClient.id);
        this.selectedClientName = matchedClient.name;
      } else {
        console.log('No matching client found for:', clientName);
        this.form.get('clientId')?.setValue(null);
      }
    }).unsubscribe();
  }

  onClientSelected(clientId: string) {
    if (clientId) {
      // Find selected client from the search results
      this.filteredClients$.subscribe(clients => {
        const selectedClient = clients.find(c => c.id === clientId);
        if (selectedClient) {
          this.selectedClientName = selectedClient.name;
          this.form.get('clientId')?.setValue(clientId);
        }
      }).unsubscribe();
    } else {
      this.selectedClientName = '';
      this.form.get('clientId')?.setValue(null);
    }
  }

  initializeClientSearch() {
    // Set initial client name if editing existing customer
    if (this.selectedCustomer.clientId) {
      // Load client name from the API
      // This would require adding a method to get client by ID
      this.selectedClientName = `Client ID: ${this.selectedCustomer.clientId}`;
    } else {
      this.selectedClientName = '';
      this.clientSearchQuery = '';
    }

    this.setupClientSearch();
    // Trigger initial empty search
    this.clientSearchSubject.next(this.selectedClientName);
  }

  setAsPrimaryContact(id: string) {
    this.customerService.setAsPrimaryContact(id, true).subscribe(() => this.list.get());
  }

  setAsDecisionMaker(id: string, isDecisionMaker: boolean) {
    this.customerService.setAsKeyDecisionMaker(id, isDecisionMaker).subscribe(() => this.list.get());
  }

  buildForm() {
    this.form = this.fb.group({
      firstName: [this.selectedCustomer.firstName || '', Validators.required],
      lastName: [this.selectedCustomer.lastName || '', Validators.required],
      email: [this.selectedCustomer.email || ''],
      phone: [this.selectedCustomer.phone || ''],
      mobilePhone: [this.selectedCustomer.mobilePhone || ''],
      jobTitle: [this.selectedCustomer.jobTitle || ''],
      department: [this.selectedCustomer.department || ''],
      clientId: [this.selectedCustomer.clientId || ''],
      isPrimaryContact: [this.selectedCustomer.isPrimaryContact || false],
      isKeyDecisionMaker: [this.selectedCustomer.isKeyDecisionMaker || false],
      notes: [this.selectedCustomer.notes || ''],
      assignedUserId: [this.selectedCustomer.assignedUserId || ''],
      isActive: [this.selectedCustomer.isActive ?? true],
    });
  }

  phoneValidator() {
    return (control: any) => {
      if (!control.value || control.value.trim() === '') {
        return null; // Campo é opcional, permite vazio
      }

      // Remove todos os caracteres não numéricos
      const phoneNumbers = control.value.replace(/\D/g, '');

      // Verifica se tem entre 10 e 11 dígitos (com ou sem DDD)
      if (phoneNumbers.length < 10 || phoneNumbers.length > 11) {
        return { invalidPhone: true };
      }

      return null;
    };
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const formValue = this.form.value;

    const requestData: CreateUpdateCustomerDto = {
      ...formValue,
    };

    let request = this.customerService.create(requestData);
    if (this.selectedCustomer.id) {
      request = this.customerService.update(this.selectedCustomer.id, requestData);
    }

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  getFullName(customer: CustomerDto): string {
    return `${customer.firstName} ${customer.lastName}`;
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
      this.selectedUserName = '';
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
      this.selectedUserName = '';
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
        this.selectedUserName = selectedValue;
      } else {
        // Se não encontrar correspondência, limpa
        this.form.get('assignedUserId')?.setValue(null);
        this.selectedUserName = '';
      }
    });
  }

  getUserDisplayName(): string {
    const userId = this.form.get('assignedUserId')?.value;
    if (!userId) return '';

    // Retorna o nome selecionado ou o valor atual do input
    return this.selectedUserName || this.userSearchQuery || '';
  }
}

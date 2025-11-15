import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
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
  CustomerService,
  CustomerDto,
  CreateUpdateCustomerDto,
  GetCustomersInput
} from '../proxy/customers';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    NgbDropdownModule,
    ModalComponent,
    AutofocusDirective,
    NgxDatatableListDirective,
    NgxDatatableDefaultDirective,
    PermissionDirective,
    ModalCloseDirective,
    LocalizationPipe
  ],
  providers: [ListService],
})
export class CustomerComponent implements OnInit {
  public readonly list = inject(ListService);
  private customerService = inject(CustomerService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  customers = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;
  selectedCustomer = {} as CustomerDto;
  form: FormGroup;
  isModalOpen = false;

  ngOnInit() {
    const customerStreamCreator = (query: GetCustomersInput) => this.customerService.getList(query);

    this.list.hookToQuery(customerStreamCreator).subscribe(response => {
      this.customers = response;
    });
  }

  createCustomer() {
    this.selectedCustomer = {} as CustomerDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCustomer(id: string) {
    this.customerService.get(id).subscribe(customer => {
      this.selectedCustomer = customer;
      this.buildForm();
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
    // TODO: Implement user selection modal
    console.log('Assign to user:', id);
  }

  setAsPrimaryContact(id: string) {
    this.customerService.setAsPrimaryContact(id).subscribe(() => this.list.get());
  }

  setAsDecisionMaker(id: string, isDecisionMaker: boolean) {
    this.customerService.setAsDecisionMaker(id, isDecisionMaker).subscribe(() => this.list.get());
  }

  buildForm() {
    this.form = this.fb.group({
      firstName: [this.selectedCustomer.firstName || '', Validators.required],
      lastName: [this.selectedCustomer.lastName || '', Validators.required],
      email: [this.selectedCustomer.email || ''],
      phone: [this.selectedCustomer.phone || ''],
      mobilePhone: [this.selectedCustomer.mobilePhone || ''],
      title: [this.selectedCustomer.title || ''],
      department: [this.selectedCustomer.department || ''],
      clientId: [this.selectedCustomer.clientId || ''],
      isPrimaryContact: [this.selectedCustomer.isPrimaryContact || false],
      isDecisionMaker: [this.selectedCustomer.isDecisionMaker || false],
      notes: [this.selectedCustomer.notes || ''],
      assignedUserId: [this.selectedCustomer.assignedUserId || ''],
      isActive: [this.selectedCustomer.isActive ?? true],
    });
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
}
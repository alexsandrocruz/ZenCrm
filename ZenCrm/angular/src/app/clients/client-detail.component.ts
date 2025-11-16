import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClientService, ClientDto } from '../proxy/clients';
import { ClientType, clientTypeOptions } from '../proxy/clients/client-type.enum';
import { ClientIndustry, clientIndustryOptions } from '../proxy/clients/client-industry.enum';
import { CustomerService, CustomerDto } from '../proxy/customers';
import { ListService } from '@abp/ng.core';
import {
  Confirmation,
  ConfirmationService,
  ModalCloseDirective,
  ModalComponent
} from '@abp/ng.theme.shared';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  imports: [
    ModalComponent,
    ModalCloseDirective
  ],
  providers: [ListService],
})
export class ClientDetailComponent implements OnInit {
  client: ClientDto;
  customers: CustomerDto[] = [];
  selectedCustomer: CustomerDto = {} as CustomerDto;
  isCustomerModalOpen = false;
  customerForm: FormGroup;

  isEditMode = false;
  form: FormGroup;

  clientTypes = clientTypeOptions;
  clientIndustries = clientIndustryOptions;

  activeTab = 'details';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private clientService: ClientService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private list: ListService,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.initForms();
    this.loadClient();
  }

  initForms(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(256)]],
      clientType: [ClientType.Business, Validators.required],
      industry: [ClientIndustry.Other],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      phone: ['', Validators.maxLength(32)],
      website: ['', Validators.maxLength(256)],
      address: ['', Validators.maxLength(1024)],
      city: ['', Validators.maxLength(256)],
      state: ['', Validators.maxLength(128)],
      postalCode: ['', Validators.maxLength(64)],
      country: ['', Validators.maxLength(128)],
      annualRevenue: [null],
      numberOfEmployees: [null],
      description: ['', Validators.maxLength(512)],
      isActive: [true],
      assignedUserId: [null],
    });

    this.customerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(128)]],
      lastName: ['', [Validators.required, Validators.maxLength(128)]],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      phone: ['', Validators.maxLength(32)],
      mobilePhone: ['', Validators.maxLength(32)],
      title: ['', Validators.maxLength(128)],
      department: ['', Validators.maxLength(256)],
      notes: ['', Validators.maxLength(512)],
      isActive: [true],
      isPrimaryContact: [false],
      isDecisionMaker: [false],
      clientId: [null],
      assignedUserId: [null],
    });
  }

  loadClient(): void {
    const clientId = this.route.snapshot.paramMap.get('id');
    if (clientId) {
      this.clientService.get(clientId).subscribe(client => {
        this.client = client;
        this.form.patchValue({
          name: client.name,
          clientType: client.clientType,
          industry: client.industry,
          email: client.email,
          phone: client.phone,
          website: client.website,
          address: client.address,
          city: client.city,
          state: client.state,
          postalCode: client.postalCode,
          country: client.country,
          annualRevenue: client.annualRevenue,
          numberOfEmployees: client.numberOfEmployees,
          description: client.description,
          isActive: client.isActive,
          assignedUserId: client.assignedUserId,
        });
        this.customerForm.patchValue({ clientId: client.id });
        this.loadCustomers();
      });
    }
  }

  loadCustomers(): void {
    if (this.client?.id) {
      this.customerService.getList({ clientId: this.client.id }).subscribe(customers => {
        this.customers = customers.items;
      });
    }
  }

  setTab(tab: string): void {
    this.activeTab = tab;
  }

  editClient(): void {
    this.isEditMode = true;
  }

  cancelEdit(): void {
    this.isEditMode = false;
    this.loadClient(); // Reload to reset form
  }

  saveClient(): void {
    if (!this.form.valid || !this.client) return;

    const clientData = { ...this.form.value };

    if (this.client.id) {
      this.clientService.update(this.client.id, clientData).subscribe(() => {
        this.isEditMode = false;
        this.loadClient();
      });
    }
  }

  createCustomer(): void {
    this.selectedCustomer = {} as CustomerDto;
    this.selectedCustomer.clientId = this.client?.id;
    this.isCustomerModalOpen = true;
    this.customerForm.reset({
      clientId: this.client?.id,
      isActive: true,
      isPrimaryContact: false,
      isDecisionMaker: false,
    });
  }

  editCustomer(customerId: string): void {
    this.customerService.get(customerId).subscribe(customer => {
      this.selectedCustomer = customer;
      this.customerForm.patchValue({
        firstName: customer.firstName,
        lastName: customer.lastName,
        email: customer.email,
        phone: customer.phone,
        mobilePhone: customer.mobilePhone,
        title: customer.title,
        department: customer.department,
        notes: customer.notes,
        isActive: customer.isActive,
        isPrimaryContact: customer.isPrimaryContact,
        isDecisionMaker: customer.isDecisionMaker,
        clientId: customer.clientId,
        assignedUserId: customer.assignedUserId,
      });
      this.isCustomerModalOpen = true;
    });
  }

  saveCustomer(): void {
    if (!this.customerForm.valid) return;

    const customerData = { ...this.customerForm.value };

    if (this.selectedCustomer.id) {
      this.customerService.update(this.selectedCustomer.id, customerData).subscribe(() => {
        this.isCustomerModalOpen = false;
        this.loadCustomers();
      });
    } else {
      this.customerService.create(customerData).subscribe(() => {
        this.isCustomerModalOpen = false;
        this.loadCustomers();
      });
    }
  }

  deleteCustomer(customerId: string): void {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.customerService.delete(customerId).subscribe(() => {
          this.loadCustomers();
        });
      }
    });
  }

  setAsPrimaryContact(customerId: string): void {
    this.customerService.setAsPrimaryContact(customerId).subscribe(() => {
      this.loadCustomers();
    });
  }

  setAsDecisionMaker(customerId: string, isDecisionMaker: boolean): void {
    this.customerService.setAsDecisionMaker(customerId, isDecisionMaker).subscribe(() => {
      this.loadCustomers();
    });
  }

  getClientTypeLabel(type: number): string {
    return this.clientTypes.find(t => t.value === type)?.label || type.toString();
  }

  getIndustryLabel(industry: number): string {
    return this.clientIndustries.find(i => i.value === industry)?.label || industry.toString();
  }

  getFullName(customer: CustomerDto): string {
    return `${customer.firstName} ${customer.lastName}`.trim();
  }

  goBack(): void {
    this.router.navigate(['/crm/clients']);
  }
}
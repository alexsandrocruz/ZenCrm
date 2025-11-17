import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import {
  ClientDto,
  ClientType,
  clientTypeOptions,
  ClientIndustry,
  clientIndustryOptions,
  CustomerService,
  CustomerDto
} from '../proxy/sales';
import { SimpleClientService } from '../services/simple-client.service';
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
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMaskDirective,
    ModalComponent,
    ModalCloseDirective
  ],
  providers: [ListService, provideNgxMask()],
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
    private clientService: SimpleClientService,
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
      isKeyDecisionMaker: [false],
      clientId: ['', Validators.required], // Changed from null to empty string to avoid validation issues
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
      this.customerService.getList({
        clientId: this.client.id,
        maxResultCount: 1000,
        skipCount: 0
      } as any).subscribe(customers => {
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
    // Ensure we have a valid client ID
    if (!this.client?.id) {
      console.error('Cannot create customer: No client ID available');
      return;
    }

    this.selectedCustomer = {} as CustomerDto;
    this.selectedCustomer.clientId = this.client.id;

    // Close modal first if it's open
    this.isCustomerModalOpen = false;

    // Use setTimeout to ensure modal state is reset
    setTimeout(() => {
      // Reset form with all required values
      this.customerForm.reset({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        mobilePhone: '',
        title: '',
        department: '',
        notes: '',
        isActive: true,
        isPrimaryContact: false,
        isKeyDecisionMaker: false,
        clientId: this.client.id,
        assignedUserId: null,
      });

      // Force form update and validation
      this.customerForm.updateValueAndValidity();

      // Open modal after form is ready
      this.isCustomerModalOpen = true;

      console.log('Form after initialization:', this.customerForm.status, this.customerForm.value);
      console.log('Is form valid?', this.customerForm.valid);
      console.log('Form errors:', this.customerForm.errors);
    }, 50);
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
        jobTitle: customer.jobTitle,
        department: customer.department,
        notes: customer.notes,
        isActive: customer.isActive,
        isPrimaryContact: customer.isPrimaryContact,
        isKeyDecisionMaker: customer.isKeyDecisionMaker,
        clientId: customer.clientId,
        assignedUserId: customer.assignedUserId,
      });
      this.isCustomerModalOpen = true;
    });
  }

  saveCustomer(): void {
    if (!this.customerForm.valid) {
      // Mark all controls as touched to show validation errors
      Object.keys(this.customerForm.controls).forEach(key => {
        const control = this.customerForm.get(key);
        control?.markAsTouched();
      });
      return;
    }

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
    // Find the current customer to get its current state
    const currentCustomer = this.customers.find(c => c.id === customerId);
    const newState = !currentCustomer?.isPrimaryContact;

    this.customerService.setAsPrimaryContact(customerId, newState).subscribe(() => {
      this.loadCustomers();
    });
  }

  setAsDecisionMaker(customerId: string, isKeyDecisionMaker: boolean): void {
    this.customerService.setAsKeyDecisionMaker(customerId, isKeyDecisionMaker).subscribe(() => {
      this.loadCustomers();
    });
  }

  getClientTypeLabel(type: number | undefined | null): string {
    if (type === undefined || type === null) {
      return '-';
    }
    const clientType = this.clientTypes.find(t => t.value === type);
    return clientType ? clientType.key || clientType.value.toString() : type.toString();
  }

  getIndustryLabel(industry: number | undefined | null): string {
    if (industry === undefined || industry === null) {
      return '-';
    }
    const clientIndustry = this.clientIndustries.find(i => i.value === industry);
    return clientIndustry ? clientIndustry.key || clientIndustry.value.toString() : industry.toString();
  }

  getFullName(customer: CustomerDto): string {
    return `${customer.firstName} ${customer.lastName}`.trim();
  }

  goBack(): void {
    this.router.navigate(['/crm/clients']);
  }
}

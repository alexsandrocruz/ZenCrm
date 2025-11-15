import {
  FormGroup,
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { formatDate } from '@angular/common';
import { NgbDatepickerModule, NgbDateStruct, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
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

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
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
export class ClientComponent implements OnInit {
  public readonly list = inject(ListService);
  private clientService = inject(ClientService);
  private fb = inject(FormBuilder);
  private confirmation = inject(ConfirmationService);

  clients = { items: [], totalCount: 0 } as PagedResultDto<ClientDto>;
  selectedClient = {} as ClientDto;
  form: FormGroup;
  clientTypes = clientTypeOptions;
  clientIndustries = clientIndustryOptions;
  isModalOpen = false;

  ngOnInit() {
    const clientStreamCreator = (query: GetClientsInput) => this.clientService.getList(query);

    this.list.hookToQuery(clientStreamCreator).subscribe(response => {
      this.clients = response;
    });
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
    // TODO: Implement user selection modal
    console.log('Assign to user:', id);
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
    const requestData: CreateUpdateClientDto = {
      ...formValue,
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

  getClientTypeLabel(clientType: number): string {
    const type = this.clientTypes.find(t => t.value === clientType);
    return type ? type.label : clientType.toString();
  }

  getIndustryLabel(industry: number): string {
    const ind = this.clientIndustries.find(i => i.value === industry);
    return ind ? ind.label : '-';
  }
}
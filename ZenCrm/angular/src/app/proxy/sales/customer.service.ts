import type { CreateUpdateCustomerDto, CustomerDto, GetCustomersInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  apiName = 'Default';
  

  addNotes = (id: string, notes: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/notes`,
      params: { notes },
    },
    { apiName: this.apiName,...config });
  

  assignToUser = (id: string, userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/assign-to-user/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  associateWithClient = (id: string, clientId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/associate-with-client/${clientId}`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateUpdateCustomerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: '/api/app/customer',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/customer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'GET',
      url: `/api/app/customer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getByClient = (clientId: string, input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: `/api/app/customer/by-client/${input.clientId}`,
      params: { clientId, filter: input.filter, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getByClientId = (clientId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<CustomerDto>>({
      method: 'GET',
      url: `/api/app/customer/by-client-id/${clientId}`,
    },
    { apiName: this.apiName,...config });
  

  getKeyDecisionMakers = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer/key-decision-makers',
      params: { filter: input.filter, clientId: input.clientId, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer',
      params: { filter: input.filter, clientId: input.clientId, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyCustomers = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer/my-customers',
      params: { filter: input.filter, clientId: input.clientId, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPrimaryContacts = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer/primary-contacts',
      params: { filter: input.filter, clientId: input.clientId, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getUnassignedCustomers = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer/unassigned-customers',
      params: { filter: input.filter, clientId: input.clientId, assignedUserId: input.assignedUserId, isActive: input.isActive, isPrimaryContact: input.isPrimaryContact, isKeyDecisionMaker: input.isKeyDecisionMaker, jobTitle: input.jobTitle, department: input.department, startDate: input.startDate, endDate: input.endDate, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  setAsKeyDecisionMaker = (id: string, isKeyDecisionMaker: boolean = true, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/set-as-key-decision-maker`,
      params: { isKeyDecisionMaker },
    },
    { apiName: this.apiName,...config });
  

  setAsPrimaryContact = (id: string, isPrimary: boolean = true, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/set-as-primary-contact`,
      params: { isPrimary },
    },
    { apiName: this.apiName,...config });
  

  setStatus = (id: string, isActive: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: `/api/app/customer/${id}/set-status`,
      params: { isActive },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateCustomerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'PUT',
      url: `/api/app/customer/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateLastContact = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>({
      method: 'PUT',
      url: `/api/app/customer/${id}/last-contact`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

import type { CustomerDto, CreateUpdateCustomerDto, GetCustomersInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  apiName = 'Default';

  create = (input: CreateUpdateCustomerDto) =>
    this.restService.request<any, CustomerDto>({
      method: 'POST',
      url: '/api/app/customer',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/customer/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, CustomerDto>({
      method: 'GET',
      url: `/api/app/customer/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetCustomersInput) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>({
      method: 'GET',
      url: '/api/app/customer',
      params: {
        sorting: input.sorting,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        filter: input.filter,
        clientId: input.clientId,
        assignedUserId: input.assignedUserId,
        isDecisionMaker: input.isDecisionMaker,
        isPrimaryContact: input.isPrimaryContact
      },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateCustomerDto) =>
    this.restService.request<any, CustomerDto>({
      method: 'PUT',
      url: `/api/app/customer/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  assignToUser = (id: string, userId: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/customer/${id}/assign-to-user`,
      params: { userId },
    },
    { apiName: this.apiName });

  setAsPrimaryContact = (id: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/customer/${id}/set-as-primary-contact`,
    },
    { apiName: this.apiName });

  setAsDecisionMaker = (id: string, isDecisionMaker: boolean) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/customer/${id}/set-as-decision-maker`,
      params: { isDecisionMaker },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
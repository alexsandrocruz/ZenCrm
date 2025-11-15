import type { SalesLeadDto, CreateUpdateSalesLeadDto, GetSalesLeadsInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SalesLeadService {
  apiName = 'Default';

  create = (input: CreateUpdateSalesLeadDto) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: '/api/app/sales-lead',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/sales-lead/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'GET',
      url: `/api/app/sales-lead/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetSalesLeadsInput) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead',
      params: {
        sorting: input.sorting,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        filter: input.filter,
        leadStatus: input.leadStatus,
        leadSource: input.leadSource,
        assignedUserId: input.assignedUserId,
        clientId: input.clientId,
        customerId: input.customerId
      },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateSalesLeadDto) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  assignToUser = (id: string, userId: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-lead/${id}/assign-to-user`,
      params: { userId },
    },
    { apiName: this.apiName });

  convertToOpportunity = (id: string, opportunityName: string, estimatedValue?: number) =>
    this.restService.request<any, string>({
      method: 'POST',
      url: `/api/app/sales/sales-lead/${id}/convert-to-opportunity`,
      params: { opportunityName, estimatedValue },
    },
    { apiName: this.apiName });

  updateStatus = (id: string, status: number) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-lead/${id}/update-status`,
      params: { status },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
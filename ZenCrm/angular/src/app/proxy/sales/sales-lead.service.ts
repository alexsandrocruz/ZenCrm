import type { LeadSource } from './lead-source.enum';
import type { LeadStatus } from './lead-status.enum';
import type { CreateUpdateSalesLeadDto, GetSalesLeadsInput, SalesLeadDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SalesLeadService {
  apiName = 'Default';

  constructor(private restService: RestService) {
    console.log('SalesLeadService initialized with API URL:', 'https://localhost:44340');
  }

  assignToUser = (id: string, userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: `/api/app/sales-lead/${id}/assign-to-user/${userId}`,
    },
    { apiName: this.apiName,...config });

  convertToOpportunity = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: `/api/app/sales-lead/${id}/convert-to-opportunity`,
    },
    { apiName: this.apiName,...config });

  create = (input: CreateUpdateSalesLeadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: '/api/app/sales-lead',
      body: input,
    },
    { apiName: this.apiName,...config });

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/sales-lead/${id}`,
    },
    { apiName: this.apiName,...config });

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'GET',
      url: `/api/app/sales-lead/${id}`,
    },
    { apiName: this.apiName,...config });

  getList = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) => {
    console.log('SalesLeadService.getList() called with input:', input);
    return this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead',
      params: { filter: input.filter, source: input.source, status: input.status, assignedUserId: input.assignedUserId, startDate: input.startDate, endDate: input.endDate, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  };

  getMyLeads = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead/my-leads',
      params: { filter: input.filter, source: input.source, status: input.status, assignedUserId: input.assignedUserId, startDate: input.startDate, endDate: input.endDate, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });

  update = (id: string, input: CreateUpdateSalesLeadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  updateStatus = (id: string, status: LeadStatus, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}/status`,
      params: { status },
    },
    { apiName: this.apiName,...config });
}
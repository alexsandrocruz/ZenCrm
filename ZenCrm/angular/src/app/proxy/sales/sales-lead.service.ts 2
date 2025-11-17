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
  

  getLeadsNeedingFollowUp = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead/leads-needing-follow-up',
      params: { filter: input.filter, status: input.status, source: input.source, priority: input.priority, assignedUserId: input.assignedUserId, clientId: input.clientId, converted: input.converted, startDate: input.startDate, endDate: input.endDate, minEstimatedValue: input.minEstimatedValue, maxEstimatedValue: input.maxEstimatedValue, doNotContact: input.doNotContact, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead',
      params: { filter: input.filter, status: input.status, source: input.source, priority: input.priority, assignedUserId: input.assignedUserId, clientId: input.clientId, converted: input.converted, startDate: input.startDate, endDate: input.endDate, minEstimatedValue: input.minEstimatedValue, maxEstimatedValue: input.maxEstimatedValue, doNotContact: input.doNotContact, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyLeads = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead/my-leads',
      params: { filter: input.filter, status: input.status, source: input.source, priority: input.priority, assignedUserId: input.assignedUserId, clientId: input.clientId, converted: input.converted, startDate: input.startDate, endDate: input.endDate, minEstimatedValue: input.minEstimatedValue, maxEstimatedValue: input.maxEstimatedValue, doNotContact: input.doNotContact, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getSourceOptions = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<enum>>({
      method: 'GET',
      url: '/api/app/sales-lead/source-options',
    },
    { apiName: this.apiName,...config });
  

  getStatusOptions = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<enum>>({
      method: 'GET',
      url: '/api/app/sales-lead/status-options',
    },
    { apiName: this.apiName,...config });
  

  getUnassignedLeads = (input: GetSalesLeadsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesLeadDto>>({
      method: 'GET',
      url: '/api/app/sales-lead/unassigned-leads',
      params: { filter: input.filter, status: input.status, source: input.source, priority: input.priority, assignedUserId: input.assignedUserId, clientId: input.clientId, converted: input.converted, startDate: input.startDate, endDate: input.endDate, minEstimatedValue: input.minEstimatedValue, maxEstimatedValue: input.maxEstimatedValue, doNotContact: input.doNotContact, includeInactive: input.includeInactive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  markAsDoNotContact = (id: string, doNotContact: boolean = true, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: `/api/app/sales-lead/${id}/mark-as-do-not-contact`,
      params: { doNotContact },
    },
    { apiName: this.apiName,...config });
  

  setFollowUpDate = (id: string, followUpDate: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'POST',
      url: `/api/app/sales-lead/${id}/set-follow-up-date`,
      params: { followUpDate },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateSalesLeadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateLastContact = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}/last-contact`,
    },
    { apiName: this.apiName,...config });
  

  updateStatus = (id: string, status: LeadStatus, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesLeadDto>({
      method: 'PUT',
      url: `/api/app/sales-lead/${id}/status`,
      params: { status },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

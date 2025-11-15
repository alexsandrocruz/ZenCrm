import type { SalesOpportunityDto, CreateUpdateSalesOpportunityDto, GetSalesOpportunitiesInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SalesOpportunityService {
  apiName = 'Default';

  create = (input: CreateUpdateSalesOpportunityDto) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'POST',
      url: '/api/app/sales-opportunity',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/sales-opportunity/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'GET',
      url: `/api/app/sales-opportunity/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetSalesOpportunitiesInput) =>
    this.restService.request<any, PagedResultDto<SalesOpportunityDto>>({
      method: 'GET',
      url: '/api/app/sales-opportunity',
      params: {
        sorting: input.sorting,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        filter: input.filter,
        pipelineStage: input.pipelineStage,
        assignedUserId: input.assignedUserId,
        clientId: input.clientId,
        customerId: input.customerId
      },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateSalesOpportunityDto) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'PUT',
      url: `/api/app/sales-opportunity/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  assignToUser = (id: string, userId: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-opportunity/${id}/assign-to-user`,
      params: { userId },
    },
    { apiName: this.apiName });

  advanceStage = (id: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-opportunity/${id}/advance-stage`,
    },
    { apiName: this.apiName });

  close = (id: string, isWon: boolean, closeReason?: string, actualValue?: number) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-opportunity/${id}/close`,
      params: { isWon, closeReason, actualValue },
    },
    { apiName: this.apiName });

  setPriority = (id: string, priority: number) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/sales-opportunity/${id}/set-priority`,
      params: { priority },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
import type { CreateUpdateSalesOpportunityDto, GetSalesOpportunitiesInput, SalesOpportunityDto } from './models';
import type { PipelineStage } from './pipeline-stage.enum';
import type { Priority } from './priority.enum';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SalesOpportunityService {
  apiName = 'Default';

  constructor(private restService: RestService) {
    console.log('SalesOpportunityService initialized with API URL:', 'https://localhost:44340');
  }

  create = (input: CreateUpdateSalesOpportunityDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'POST',
      url: '/api/app/sales-opportunity',
      body: input,
    },
    { apiName: this.apiName,...config });

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/sales-opportunity/${id}`,
    },
    { apiName: this.apiName,...config });

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'GET',
      url: `/api/app/sales-opportunity/${id}`,
    },
    { apiName: this.apiName,...config });

  getList = (input: GetSalesOpportunitiesInput, config?: Partial<Rest.Config>) => {
    console.log('SalesOpportunityService.getList() called with input:', input);
    return this.restService.request<any, PagedResultDto<SalesOpportunityDto>>({
      method: 'GET',
      url: '/api/app/sales-opportunity',
      params: { filter: input.filter, clientId: input.clientId, ownerUserId: input.ownerUserId, stage: input.stage, priority: input.priority, salesLeadId: input.salesLeadId, startDate: input.startDate, endDate: input.endDate, minEstimatedValue: input.minEstimatedValue, maxEstimatedValue: input.maxEstimatedValue, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  };

  update = (id: string, input: CreateUpdateSalesOpportunityDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesOpportunityDto>({
      method: 'PUT',
      url: `/api/app/sales-opportunity/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}
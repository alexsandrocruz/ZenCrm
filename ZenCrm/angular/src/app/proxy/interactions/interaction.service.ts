import type { InteractionDto, CreateUpdateInteractionDto, GetInteractionsInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class InteractionService {
  apiName = 'Default';

  create = (input: CreateUpdateInteractionDto) =>
    this.restService.request<any, InteractionDto>({
      method: 'POST',
      url: '/api/app/interaction',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/interaction/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, InteractionDto>({
      method: 'GET',
      url: `/api/app/interaction/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetInteractionsInput) =>
    this.restService.request<any, PagedResultDto<InteractionDto>>({
      method: 'GET',
      url: '/api/app/interaction',
      params: {
        sorting: input.sorting,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        filter: input.filter,
        interactionType: input.interactionType,
        interactionStatus: input.interactionStatus,
        assignedUserId: input.assignedUserId,
        clientId: input.clientId,
        customerId: input.customerId,
        salesLeadId: input.salesLeadId,
        salesOpportunityId: input.salesOpportunityId
      },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateInteractionDto) =>
    this.restService.request<any, InteractionDto>({
      method: 'PUT',
      url: `/api/app/interaction/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  complete = (id: string, result?: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/interaction/${id}/complete`,
      params: { result },
    },
    { apiName: this.apiName });

  cancel = (id: string, reason?: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/interaction/${id}/cancel`,
      params: { reason },
    },
    { apiName: this.apiName });

  postpone = (id: string, newDate: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/sales/interaction/${id}/postpone`,
      params: { newDate },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
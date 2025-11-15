import type { ClientDto, CreateUpdateClientDto, GetClientsInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  apiName = 'Default';

  create = (input: CreateUpdateClientDto) =>
    this.restService.request<any, ClientDto>({
      method: 'POST',
      url: '/api/app/client',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/client/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, ClientDto>({
      method: 'GET',
      url: `/api/app/client/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetClientsInput) =>
    this.restService.request<any, PagedResultDto<ClientDto>>({
      method: 'GET',
      url: '/api/app/client',
      params: {
        sorting: input.sorting,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        filter: input.filter,
        clientType: input.clientType,
        industry: input.industry,
        assignedUserId: input.assignedUserId
      },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateUpdateClientDto) =>
    this.restService.request<any, ClientDto>({
      method: 'PUT',
      url: `/api/app/client/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  assignToUser = (id: string, userId: string) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/client/${id}/assign-to-user`,
      params: { userId },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
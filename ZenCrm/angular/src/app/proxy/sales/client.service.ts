import type { ClientIndustry } from './client-industry.enum';
import type { ClientType } from './client-type.enum';
import type { ClientDto, CreateUpdateClientDto, GetClientsInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  apiName = 'Default';

  constructor(private restService: RestService) {
    // Garantir que o ambiente esteja configurado corretamente
    if (environment.apis?.default?.url) {
      console.log('ClientService initialized with API URL:', environment.apis.default.url);
    }
  }

  assignToUser = (id: string, userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClientDto>({
      method: 'POST',
      url: `/api/app/client/${id}/assign-to-user/${userId}`,
    },
    { apiName: this.apiName,...config });


  create = (input: CreateUpdateClientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClientDto>({
      method: 'POST',
      url: '/api/app/client',
      body: input,
    },
    { apiName: this.apiName,...config });


  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/client/${id}`,
    },
    { apiName: this.apiName,...config });


  get = (id: string, config?: Partial<Rest.Config>) => {
    console.log('ClientService.get() called with id:', id);
    return this.restService.request<any, ClientDto>({
      method: 'GET',
      url: `/api/app/client/${id}`,
    },
    { apiName: this.apiName,...config });
  };


  getList = (input: GetClientsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ClientDto>>({
      method: 'GET',
      url: '/api/app/client',
      params: { filter: input.filter, clientType: input.clientType, industry: input.industry, assignedUserId: input.assignedUserId, isActive: input.isActive, startDate: input.startDate, endDate: input.endDate, minAnnualRevenue: input.minAnnualRevenue, maxAnnualRevenue: input.maxAnnualRevenue, minEmployees: input.minEmployees, maxEmployees: input.maxEmployees, city: input.city, state: input.state, country: input.country, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });


  update = (id: string, input: CreateUpdateClientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ClientDto>({
      method: 'PUT',
      url: `/api/app/client/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}
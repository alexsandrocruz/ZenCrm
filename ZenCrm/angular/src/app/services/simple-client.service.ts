import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClientDto, CreateUpdateClientDto, GetClientsInput } from '../proxy/sales';
import { PagedResultDto } from '@abp/ng.core';

@Injectable({
  providedIn: 'root'
})
export class SimpleClientService {
  private baseUrl = '/api/app/client';

  constructor(private http: HttpClient) {}

  getList(input: GetClientsInput): Observable<PagedResultDto<ClientDto>> {
    return this.http.get<PagedResultDto<ClientDto>>(this.baseUrl, { params: input as any });
  }

  get(id: string): Observable<ClientDto> {
    return this.http.get<ClientDto>(`${this.baseUrl}/${id}`);
  }

  create(input: CreateUpdateClientDto): Observable<ClientDto> {
    return this.http.post<ClientDto>(this.baseUrl, input);
  }

  update(id: string, input: CreateUpdateClientDto): Observable<ClientDto> {
    return this.http.put<ClientDto>(`${this.baseUrl}/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  assignToUser(id: string, userId: string): Observable<ClientDto> {
    return this.http.post<ClientDto>(`${this.baseUrl}/${id}/assign-to-user/${userId}`, {});
  }
}
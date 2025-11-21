import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  InteractionDto,
  CreateUpdateInteractionDto,
  GetInteractionsInput
} from '../proxy/sales/models';
import { PagedResultDto } from '@abp/ng.core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SimpleInteractionService {
  private baseUrl = `${environment.apis.default.url}/api/app/interaction`;

  constructor(private http: HttpClient) {}

  // Implementações básicas via HTTP
  getList(input: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const params = this.buildParams(input);
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}`, { params });
  }

  get(id: string): Observable<InteractionDto> {
    return this.http.get<InteractionDto>(`${this.baseUrl}/${id}`);
  }

  create(input: CreateUpdateInteractionDto): Observable<InteractionDto> {
    return this.http.post<InteractionDto>(this.baseUrl, input);
  }

  update(id: string, input: CreateUpdateInteractionDto): Observable<InteractionDto> {
    return this.http.put<InteractionDto>(`${this.baseUrl}/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  start(id: string): Observable<InteractionDto> {
    return this.http.post<InteractionDto>(`${this.baseUrl}/${id}/start`, {});
  }

  complete(id: string, outcome?: string): Observable<InteractionDto> {
    const params = outcome ? { outcome } : {};
    return this.http.post<InteractionDto>(`${this.baseUrl}/${id}/complete`, {}, { params });
  }

  cancel(id: string): Observable<InteractionDto> {
    return this.http.post<InteractionDto>(`${this.baseUrl}/${id}/cancel`, {});
  }

  postpone(id: string, newScheduledDate: Date, reason?: string): Observable<InteractionDto> {
    const params: any = { newScheduledDate: newScheduledDate.toISOString() };
    if (reason) {
      params.reason = reason;
    }
    return this.http.post<InteractionDto>(`${this.baseUrl}/${id}/postpone`, {}, { params });
  }

  setReminder(id: string, requiresReminder: boolean, reminderDate?: Date): Observable<InteractionDto> {
    const params = { requiresReminder: requiresReminder.toString() };
    if (reminderDate) {
      (params as any).reminderDate = reminderDate.toISOString();
    }
    return this.http.post<InteractionDto>(`${this.baseUrl}/${id}/set-reminder`, {}, { params });
  }

  getMyInteractions(input: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const params = this.buildParams(input);
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}/my-interactions`, { params });
  }

  getByClient(clientId: string, input: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const params = this.buildParams({ ...input, clientId });
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}/by-client/${clientId}`, { params });
  }

  getByCustomer(customerId: string, input: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const params = this.buildParams({ ...input, customerId });
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}/by-customer/${customerId}`, { params });
  }

  getUpcomingInteractions(input?: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const defaultInput = {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'scheduledDate ASC',
      includeCompleted: false,
      includeCancelled: false
    } as GetInteractionsInput;

    const params = this.buildParams({ ...defaultInput, ...input });
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}/upcoming-interactions`, { params });
  }

  getOverdueInteractions(input?: GetInteractionsInput): Observable<PagedResultDto<InteractionDto>> {
    const defaultInput = {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'scheduledDate ASC',
      includeCompleted: false,
      includeCancelled: false
    } as GetInteractionsInput;

    const params = this.buildParams({ ...defaultInput, ...input });
    return this.http.get<PagedResultDto<InteractionDto>>(`${this.baseUrl}/overdue-interactions`, { params });
  }

  private buildParams(input: any): any {
    const params: any = {};
    Object.keys(input).forEach(key => {
      if (input[key] !== undefined && input[key] !== null) {
        params[key] = input[key];
      }
    });
    return params;
  }
}
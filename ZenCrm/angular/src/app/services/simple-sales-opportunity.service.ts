import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SalesOpportunityDto, CreateUpdateSalesOpportunityDto, GetSalesOpportunitiesInput } from '../proxy/sales';
import { PagedResultDto } from '@abp/ng.core';
import { PipelineStage, Priority } from '../proxy/sales';

@Injectable({
  providedIn: 'root'
})
export class SimpleSalesOpportunityService {
  private baseUrl = '/api/app/sales-opportunity';

  constructor(private http: HttpClient) {}

  getList(input: GetSalesOpportunitiesInput): Observable<PagedResultDto<SalesOpportunityDto>> {
    return this.http.get<PagedResultDto<SalesOpportunityDto>>(this.baseUrl, { params: input as any });
  }

  get(id: string): Observable<SalesOpportunityDto> {
    return this.http.get<SalesOpportunityDto>(`${this.baseUrl}/${id}`);
  }

  create(input: CreateUpdateSalesOpportunityDto): Observable<SalesOpportunityDto> {
    return this.http.post<SalesOpportunityDto>(this.baseUrl, input);
  }

  update(id: string, input: CreateUpdateSalesOpportunityDto): Observable<SalesOpportunityDto> {
    return this.http.put<SalesOpportunityDto>(`${this.baseUrl}/${id}`, input);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  moveOpportunityToStage(id: string, newStage: PipelineStage): Observable<SalesOpportunityDto> {
    return this.http.post<SalesOpportunityDto>(`${this.baseUrl}/${id}/move-to-stage`, null, {
      params: { newStage: newStage.toString() }
    });
  }

  setPriority(id: string, priority: Priority): Observable<SalesOpportunityDto> {
    return this.http.post<SalesOpportunityDto>(`${this.baseUrl}/${id}/set-priority`, null, {
      params: { priority: priority.toString() }
    });
  }

  updateExpectedCloseDate(id: string, date: string): Observable<SalesOpportunityDto> {
    return this.http.put<SalesOpportunityDto>(`${this.baseUrl}/${id}/expected-close-date`, null, {
      params: { date }
    });
  }

  updateExpectedValue(id: string, value: number): Observable<SalesOpportunityDto> {
    return this.http.put<SalesOpportunityDto>(`${this.baseUrl}/${id}/expected-value`, null, {
      params: { value: value.toString() }
    });
  }

  closeWon(id: string, actualValue?: number): Observable<SalesOpportunityDto> {
    return this.http.post<SalesOpportunityDto>(`${this.baseUrl}/${id}/close-won`, null, {
      params: { actualValue: actualValue?.toString() }
    });
  }

  closeLost(id: string, lostReason: string, competitor?: string): Observable<SalesOpportunityDto> {
    return this.http.post<SalesOpportunityDto>(`${this.baseUrl}/${id}/close-lost`, null, {
      params: { lostReason, competitor }
    });
  }
}
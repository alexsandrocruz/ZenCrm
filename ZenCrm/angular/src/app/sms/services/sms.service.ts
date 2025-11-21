import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  SendSmsRequestDto,
  SmsSendResultDto,
  PhoneValidationResultDto,
  SmsCostResultDto,
  SmsTemplateDto,
  SmsDeliveryStatusDto,
  SmsDeliveryStatisticsDto,
  SmsLookupParameters
} from '../models/sms.models';

@Injectable({
  providedIn: 'root'
})
export class SmsService {
  private baseUrl = `${environment.apis.default.url}/api/app/sms`;

  constructor(private http: HttpClient) {}

  /**
   * Send a single SMS message
   */
  sendSms(request: SendSmsRequestDto): Observable<SmsSendResultDto> {
    return this.http.post<SmsSendResultDto>(`${this.baseUrl}/send`, request);
  }

  /**
   * Send SMS to multiple phone numbers
   */
  sendBulkSms(request: SendSmsRequestDto & { phoneNumbers: string[] }): Observable<SmsSendResultDto[]> {
    return this.http.post<SmsSendResultDto[]>(`${this.baseUrl}/send-bulk`, request);
  }

  /**
   * Validate phone numbers
   */
  validatePhoneNumber(phoneNumber: string): Observable<PhoneValidationResultDto> {
    const params = new HttpParams().set('phoneNumber', phoneNumber);
    return this.http.post<PhoneValidationResultDto>(`${this.baseUrl}/validate-phone-numbers`, { phoneNumber });
  }

  /**
   * Calculate SMS cost
   */
  calculateCost(phoneNumber: string, messageLength: number): Observable<SmsCostResultDto> {
    const params = new HttpParams()
      .set('phoneNumber', phoneNumber)
      .set('messageLength', messageLength.toString());
    return this.http.get<SmsCostResultDto>(`${this.baseUrl}/calculate-cost`, { params });
  }

  /**
   * Get SMS delivery statistics
   */
  getDeliveryStatistics(startDate?: Date, endDate?: Date): Observable<SmsDeliveryStatisticsDto> {
    let params = new HttpParams();
    if (startDate) {
      params = params.set('startDate', startDate.toISOString());
    }
    if (endDate) {
      params = params.set('endDate', endDate.toISOString());
    }
    return this.http.get<SmsDeliveryStatisticsDto>(`${this.baseUrl}/delivery-statistics`, { params });
  }

  /**
   * Get delivery status for a specific message
   */
  getDeliveryStatus(messageId: string): Observable<SmsDeliveryStatusDto> {
    return this.http.get<SmsDeliveryStatusDto>(`${this.baseUrl}/${messageId}/delivery-status`);
  }

  /**
   * Get list of SMS messages
   */
  getSmsList(parameters: SmsLookupParameters): Observable<any> {
    let params = new HttpParams();

    if (parameters.filter) {
      params = params.set('Filter', parameters.filter);
    }
    if (parameters.status) {
      params = params.set('Status', parameters.status);
    }
    if (parameters.messageType) {
      params = params.set('MessageType', parameters.messageType);
    }
    if (parameters.startDate) {
      params = params.set('StartDate', parameters.startDate.toISOString());
    }
    if (parameters.endDate) {
      params = params.set('EndDate', parameters.endDate.toISOString());
    }
    if (parameters.toPhoneNumber) {
      params = params.set('ToPhoneNumber', parameters.toPhoneNumber);
    }
    if (parameters.campaignId) {
      params = params.set('CampaignId', parameters.campaignId);
    }
    if (parameters.sorting) {
      params = params.set('Sorting', parameters.sorting);
    }

    params = params.set('MaxResultCount', parameters.maxResultCount.toString());
    params = params.set('SkipCount', parameters.skipCount.toString());

    return this.http.get<any>(`${this.baseUrl}`, { params });
  }

  /**
   * Get SMS templates
   */
  getTemplates(): Observable<SmsTemplateDto[]> {
    return this.http.get<SmsTemplateDto[]>(`${this.baseUrl}/templates`);
  }

  /**
   * Get specific SMS template
   */
  getTemplate(templateId: string): Observable<SmsTemplateDto> {
    return this.http.get<SmsTemplateDto>(`${this.baseUrl}/templates/${templateId}`);
  }

  /**
   * Create new SMS template
   */
  createTemplate(template: Partial<SmsTemplateDto>): Observable<SmsTemplateDto> {
    return this.http.post<SmsTemplateDto>(`${this.baseUrl}/templates`, template);
  }

  /**
   * Update SMS template
   */
  updateTemplate(templateId: string, template: Partial<SmsTemplateDto>): Observable<SmsTemplateDto> {
    return this.http.put<SmsTemplateDto>(`${this.baseUrl}/templates/${templateId}`, template);
  }

  /**
   * Delete SMS template
   */
  deleteTemplate(templateId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/templates/${templateId}`);
  }

  /**
   * Generate SMS content from template
   */
  generateContent(templateId: string, variables: { [key: string]: any }): Observable<{ content: string; segments: number; isValid: boolean }> {
    return this.http.post<{ content: string; segments: number; isValid: boolean }>(`${this.baseUrl}/templates/${templateId}/generate-content`, { variables });
  }

  /**
   * Get SMS template statistics
   */
  getTemplateStatistics(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/templates/statistics`);
  }
}
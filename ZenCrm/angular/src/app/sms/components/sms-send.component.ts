import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SmsService } from '../services/sms.service';
import { SendSmsRequestDto, SmsSendResultDto } from '../models/sms.models';
import { TextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';

@Component({
  selector: 'app-sms-send',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TextBoxModule,
    DropDownListModule,
    CheckBoxModule,
    DateTimePickerModule
  ],
  templateUrl: './sms-send.component.html',
  styles: [`
    :host {
      display: block;
      width: 100%;
    }

    .page-content {
      padding: 20px;
      background-color: #f5f5f5;
      min-height: 100vh;
    }

    .breadcrumbs {
      margin-bottom: 20px;
    }

    .breadcrumb-text {
      font-size: 16px;
      font-weight: 500;
      color: #333;
      opacity: 0.87;
    }

    .content-wrapper {
      max-width: 1200px;
      margin: 0 auto;
    }

    .send-sms-card {
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0,0,0,0.08);
      margin-bottom: 20px;
    }

    .card-header {
      padding: 24px;
      border-bottom: 1px solid #e0e0e0;
    }

    .card-title {
      font-size: 20px;
      font-weight: 500;
      color: #333;
      margin: 0 0 8px 0;
    }

    .card-subtitle {
      font-size: 14px;
      color: #666;
      margin: 0;
      opacity: 0.87;
    }

    .card-body {
      padding: 24px;
    }

    .card-footer {
      padding: 20px 24px;
      border-top: 1px solid #e0e0e0;
      background-color: #fafafa;
    }

    .form-group {
      margin-bottom: 20px;
    }

    .error-message {
      color: #f44336;
      font-size: 12px;
      margin-top: 4px;
    }

    .character-count {
      font-size: 12px;
      color: #666;
      margin-top: 4px;
      text-align: right;
    }

    .template-variables {
      margin-top: 24px;
      padding: 20px;
      background-color: #f8f9fa;
      border-radius: 6px;
      border-left: 4px solid #2196f3;
    }

    .advanced-settings {
      margin-top: 24px;
    }

    .section-title {
      font-size: 16px;
      font-weight: 500;
      color: #333;
      margin: 0 0 8px 0;
    }

    .section-subtitle {
      font-size: 14px;
      color: #666;
      margin: 0 0 20px 0;
      opacity: 0.87;
    }

    .action-buttons {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
    }

    .results-section {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .result-card {
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
      border-left: 4px solid #2196f3;
    }

    .result-card.validation-card {
      border-left-color: #4caf50;
    }

    .result-card.cost-card {
      border-left-color: #ff9800;
    }

    .result-card.send-result-card.success {
      border-left-color: #4caf50;
    }

    .result-card.send-result-card.error {
      border-left-color: #f44336;
    }

    .result-header {
      padding: 16px 20px;
      border-bottom: 1px solid #e0e0e0;
      background-color: #fafafa;
    }

    .result-title {
      font-size: 16px;
      font-weight: 500;
      margin: 0;
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .result-title.success {
      color: #4caf50;
    }

    .result-title.error {
      color: #f44336;
    }

    .result-content {
      padding: 20px;
    }

    .numbers-list {
      list-style: none;
      padding: 0;
      margin: 0;
    }

    .valid-number {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 8px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .valid-number:last-child {
      border-bottom: none;
    }

    .invalid-number {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 8px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .invalid-number:last-child {
      border-bottom: none;
    }

    .original {
      font-weight: 500;
      color: #333;
    }

    .formatted {
      color: #4caf50;
      font-weight: 500;
    }

    .arrow {
      color: #666;
    }

    .error {
      color: #f44336;
      font-size: 12px;
    }

    .cost-details {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
    }

    .cost-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px;
      background-color: #f8f9fa;
      border-radius: 6px;
    }

    .cost-label {
      font-weight: 500;
      color: #333;
    }

    .cost-value {
      font-weight: 600;
      color: #2196f3;
    }

    .cost-type.international {
      color: #ff9800;
    }

    .cost-type.domestic {
      color: #4caf50;
    }

    .success-details,
    .error-details {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .detail-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 8px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .detail-item:last-child {
      border-bottom: none;
    }

    .detail-label {
      font-weight: 500;
      color: #333;
    }

    .detail-value {
      font-weight: 400;
      color: #666;
    }

    .error-text {
      color: #f44336;
    }

    /* Responsive adjustments */
    @media (max-width: 768px) {
      .page-content {
        padding: 12px;
      }

      .card-header,
      .card-body,
      .card-footer {
        padding: 16px;
      }

      .action-buttons {
        flex-direction: column;
      }

      .action-buttons .e-btn {
        width: 100%;
        justify-content: center;
      }
    }
  `]
})
export class SmsSendComponent implements OnInit {
  smsForm: FormGroup;
  templates: any[] = [];
  currentTemplate: any;
  validationResult: any;
  costResult: any;
  sendResult: SmsSendResultDto | null = null;
  isSubmitting = false;
  templateVariables: { [key: string]: { name: string; description: string } } = {};

  // Options for dropdowns
  categoryOptions = [
    { text: 'Transacional', value: 0 },
    { text: 'Marketing', value: 1 },
    { text: 'Notificação', value: 2 },
    { text: 'Autenticação', value: 3 },
    { text: 'Suporte', value: 4 }
  ];

  priorityOptions = [
    { text: 'Baixa', value: 0 },
    { text: 'Normal', value: 1 },
    { text: 'Alta', value: 2 }
  ];

  constructor(
    private fb: FormBuilder,
    private smsService: SmsService
  ) {
    this.smsForm = this.fb.group({
      phoneNumber: ['', [Validators.required]],
      smsTemplateId: [''],
      content: [''],
      category: [0],
      priority: [1],
      scheduleSend: [false],
      scheduledSendDate: [''],
      campaignId: ['']
    });
  }

  ngOnInit(): void {
    this.loadTemplates();
    this.setupFormListeners();
  }

  private setupFormListeners(): void {
    this.smsForm.get('smsTemplateId')?.valueChanges.subscribe(templateId => {
      if (templateId) {
        this.loadTemplate(templateId);
        this.smsForm.get('content')?.clearValidators();
        this.smsForm.get('content')?.updateValueAndValidity();
      } else {
        this.currentTemplate = null;
        this.templateVariables = {};
        this.smsForm.get('content')?.setValidators([Validators.required]);
        this.smsForm.get('content')?.updateValueAndValidity();
      }
    });
  }

  private async loadTemplates(): Promise<void> {
    try {
      this.templates = await this.smsService.getTemplates().toPromise();
      console.log('Templates loaded:', this.templates);
    } catch (error) {
      console.error('Error loading templates:', error);
      // Não mostra alert, apenas loga o erro
    }
  }

  private async loadTemplate(templateId: string): Promise<void> {
    try {
      this.currentTemplate = await this.smsService.getTemplate(templateId).toPromise();
      if (this.currentTemplate?.variableDefinitions) {
        this.parseTemplateVariables(this.currentTemplate.variableDefinitions);
      }
    } catch (error) {
      console.error('Error loading template:', error);
      alert('Failed to load template details');
    }
  }

  private parseTemplateVariables(variableDefinitions: string): void {
    try {
      const variables = JSON.parse(variableDefinitions);
      this.templateVariables = {};

      Object.keys(variables).forEach(key => {
        this.templateVariables[key] = {
          name: key,
          description: variables[key]?.description || key
        };
        // Add form control for this variable
        if (!this.smsForm.contains(key)) {
          this.smsForm.addControl(key, this.fb.control(''));
        }
      });
    } catch (error) {
      console.error('Error parsing template variables:', error);
    }
  }

  async validatePhone(): Promise<void> {
    const phoneNumber = this.smsForm.get('phoneNumber')?.value;
    if (!phoneNumber) {
      alert('Please enter a phone number first');
      return;
    }

    try {
      this.validationResult = await this.smsService.validatePhoneNumber(phoneNumber).toPromise();
    } catch (error) {
      console.error('Error validating phone:', error);
      alert('Failed to validate phone number');
    }
  }

  async calculateCost(): Promise<void> {
    const phoneNumber = this.smsForm.get('phoneNumber')?.value;
    const content = this.getMessageContent();

    if (!phoneNumber || !content) {
      alert('Please enter phone number and message content first');
      return;
    }

    try {
      this.costResult = await this.smsService.calculateCost(phoneNumber, content.length).toPromise();
    } catch (error) {
      console.error('Error calculating cost:', error);
      alert('Failed to calculate cost');
    }
  }

  async onSubmit(): Promise<void> {
    if (!this.smsForm.valid) {
      alert('Please fill all required fields');
      return;
    }

    this.isSubmitting = true;

    try {
      const request: SendSmsRequestDto = {
        phoneNumber: this.smsForm.get('phoneNumber')?.value,
        smsTemplateId: this.smsForm.get('smsTemplateId')?.value || null,
        content: this.getMessageContent(),
        variables: this.getTemplateVariables(),
        category: this.smsForm.get('category')?.value,
        priority: this.smsForm.get('priority')?.value,
        scheduledSendDate: this.smsForm.get('scheduleSend')?.value
          ? new Date(this.smsForm.get('scheduledSendDate')?.value)
          : null,
        campaignId: this.smsForm.get('campaignId')?.value || null
      };

      this.sendResult = await this.smsService.sendSms(request).toPromise();

      if (this.sendResult.success) {
        alert('SMS sent successfully!');
        this.resetForm();
      } else {
        alert('Failed to send SMS');
      }
    } catch (error) {
      console.error('Error sending SMS:', error);
      alert('Error sending SMS');
    } finally {
      this.isSubmitting = false;
    }
  }

  private getMessageContent(): string {
    if (this.smsForm.get('smsTemplateId')?.value) {
      return ''; // Template content will be generated on backend
    }
    return this.smsForm.get('content')?.value || '';
  }

  private getTemplateVariables(): { [key: string]: any } {
    const variables: { [key: string]: any } = {};
    Object.keys(this.templateVariables).forEach(key => {
      if (this.smsForm.contains(key)) {
        variables[key] = this.smsForm.get(key)?.value;
      }
    });
    return variables;
  }

  private resetForm(): void {
    this.smsForm.reset({
      phoneNumber: '',
      smsTemplateId: '',
      content: '',
      category: 0,
      priority: 1,
      scheduleSend: false,
      scheduledSendDate: '',
      campaignId: ''
    });

    // Remove template variable controls
    Object.keys(this.templateVariables).forEach(key => {
      if (this.smsForm.contains(key)) {
        this.smsForm.removeControl(key);
      }
    });

    this.templateVariables = {};
    this.currentTemplate = null;
    this.validationResult = null;
    this.costResult = null;
    this.sendResult = null;
  }

  onTemplateChange(): void {
    const templateId = this.smsForm.get('smsTemplateId')?.value;
    if (!templateId) {
      this.currentTemplate = null;
      this.templateVariables = {};
    }
  }
}
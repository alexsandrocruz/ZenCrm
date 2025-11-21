export interface SendSmsRequestDto {
  phoneNumber: string;
  smsTemplateId?: string;
  content?: string;
  variables?: { [key: string]: any };
  priority: number;
  scheduledSendDate?: Date;
  relatedEntityId?: string;
  relatedEntityType?: string;
  interactionId?: string;
  campaignId?: string;
  category: number;
}

export interface SmsSendResultDto {
  messageId: string;
  success: boolean;
  errorMessage?: string;
  externalMessageId?: string;
  cost: number;
  currency: string;
  segments: number;
  sentAt?: Date;
}

export interface PhoneValidationResultDto {
  validNumbers: ValidPhoneNumberDto[];
  invalidNumbers: InvalidPhoneNumberDto[];
  totalNumbers: number;
  allValid: boolean;
}

export interface ValidPhoneNumberDto {
  original: string;
  formatted: string;
  countryCode: string;
  type: string;
}

export interface InvalidPhoneNumberDto {
  original: string;
  errorReason: string;
}

export interface SmsCostResultDto {
  price: number;
  currency: string;
  segments: number;
  countryCode: string;
  isInternational: boolean;
  pricePerSegment: string;
}

export interface SmsTemplateDto {
  id: string;
  name: string;
  description: string;
  category: number;
  contentTemplate: string;
  variableDefinitions: string;
  culture: string;
  isActive: boolean;
  tags: string;
  maxCharactersPerSegment: number;
  useUnicode: boolean;
  autoSplitLongMessages: boolean;
  senderName: string;
  isMarketingTemplate: boolean;
  requiredConsentType: number;
  defaultPriority: number;
  usageCount: number;
  creationTime: Date;
  lastModificationTime?: Date;
}

export interface SmsDeliveryStatusDto {
  messageId: string;
  status: string;
  sentAt?: Date;
  deliveredAt?: Date;
  readAt?: Date;
  failedAt?: Date;
  errorCode?: string;
  errorMessage?: string;
  retryCount: number;
}

export interface SmsDeliveryStatisticsDto {
  totalMessages: number;
  queuedMessages: number;
  sentMessages: number;
  deliveredMessages: number;
  readMessages: number;
  failedMessages: number;
  undeliveredMessages: number;
  rejectedMessages: number;
  canceledMessages: number;
  totalCost: number;
  averageCost: number;
  deliveryRate: number;
  readRate: number;
  failureRate: number;
  averageDeliveryTime?: TimeSpan;
  totalSegments: number;
  totalRetries: number;
  lastActivity?: Date;
}

export interface TimeSpan {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  milliseconds: number;
  ticks: number;
  totalDays: number;
  totalHours: number;
  totalMinutes: number;
  totalSeconds: number;
  totalMilliseconds: number;
}

export interface SmsLookupParameters {
  filter?: string;
  status?: string;
  messageType?: string;
  startDate?: Date;
  endDate?: Date;
  toPhoneNumber?: string;
  campaignId?: string;
  maxResultCount: number;
  skipCount: number;
  sorting?: string;
}
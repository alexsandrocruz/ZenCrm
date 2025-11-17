import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ClientType } from './client-type.enum';
import type { ClientIndustry } from './client-industry.enum';
import type { InteractionType } from './interaction-type.enum';
import type { InteractionStatus } from './interaction-status.enum';
import type { Priority } from './priority.enum';
import type { LeadStatus } from './lead-status.enum';
import type { LeadSource } from './lead-source.enum';
import type { PipelineStage } from './pipeline-stage.enum';

export interface ClientDto extends AuditedEntityDto<string> {
  name?: string;
  documentNumber?: string;
  clientType?: ClientType;
  industry?: ClientIndustry;
  description?: string;
  website?: string;
  email?: string;
  phone?: string;
  address?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  annualRevenue?: number;
  numberOfEmployees?: number;
  isActive: boolean;
  assignedUserId?: string;
  assignedUserName?: string;
  lastInteractionDate?: string;
  customerCount: number;
  opportunityCount: number;
  totalOpportunityValue: number;
  getFullAddress?: string;
  getTypeDisplay?: string;
  getIndustryDisplay?: string;
}

export interface CreateUpdateClientDto {
  name: string;
  documentNumber?: string;
  clientType?: ClientType;
  industry?: ClientIndustry;
  description?: string;
  website?: string;
  email?: string;
  phone?: string;
  address?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  country?: string;
  annualRevenue?: number;
  numberOfEmployees?: number;
  isActive: boolean;
  assignedUserId?: string;
}

export interface CreateUpdateCustomerDto {
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  jobTitle?: string;
  department?: string;
  notes?: string;
  isActive: boolean;
  isPrimaryContact: boolean;
  isKeyDecisionMaker: boolean;
  clientId?: string;
  assignedUserId?: string;
}

export interface CreateUpdateInteractionDto {
  subject: string;
  description?: string;
  type?: InteractionType;
  status?: InteractionStatus;
  priority?: Priority;
  scheduledDate?: string;
  startDate?: string;
  endDate?: string;
  durationMinutes: number;
  location?: string;
  outcome?: string;
  salesLeadId?: string;
  clientId?: string;
  customerId?: string;
  ownerUserId?: string;
  isAllDay: boolean;
  requiresReminder: boolean;
  reminderDate?: string;
  additionalData?: string;
}

export interface CreateUpdateSalesLeadDto {
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  company?: string;
  jobTitle?: string;
  description?: string;
  status?: LeadStatus;
  source?: LeadSource;
  priority?: Priority;
  assignedUserId?: string;
  clientId?: string;
  estimatedValue: number;
  expectedCloseDate?: string;
  nextFollowUpDate?: string;
  doNotContact: boolean;
}

export interface CreateUpdateSalesOpportunityDto {
  name: string;
  description?: string;
  estimatedValue: number;
  priority?: Priority;
  expectedCloseDate?: string;
  salesLeadId?: string;
  clientId?: string;
  ownerUserId?: string;
  competitor?: string;
  parentOpportunityId?: string;
  isActive: boolean;
}

export interface CustomerDto extends AuditedEntityDto<string> {
  firstName?: string;
  lastName?: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  jobTitle?: string;
  department?: string;
  notes?: string;
  isActive: boolean;
  isPrimaryContact: boolean;
  isKeyDecisionMaker: boolean;
  clientId?: string;
  clientName?: string;
  lastContactDate?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  interactionCount: number;
  lastInteractionDate?: string;
  getFullName?: string;
  getDisplayName?: string;
}

export interface GetClientsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  clientType?: ClientType;
  industry?: ClientIndustry;
  assignedUserId?: string;
  isActive?: boolean;
  startDate?: string;
  endDate?: string;
  minAnnualRevenue?: number;
  maxAnnualRevenue?: number;
  minEmployees?: number;
  maxEmployees?: number;
  city?: string;
  state?: string;
  country?: string;
}

export interface GetCustomersInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  clientId?: string;
  assignedUserId?: string;
  isActive?: boolean;
  isPrimaryContact?: boolean;
  isKeyDecisionMaker?: boolean;
  jobTitle?: string;
  department?: string;
  startDate?: string;
  endDate?: string;
  includeInactive: boolean;
}

export interface GetInteractionsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  type?: InteractionType;
  status?: InteractionStatus;
  priority?: Priority;
  ownerUserId?: string;
  salesLeadId?: string;
  clientId?: string;
  customerId?: string;
  startDate?: string;
  endDate?: string;
  requiresReminder?: boolean;
  isAllDay?: boolean;
  includeCompleted: boolean;
  includeCancelled: boolean;
}

export interface GetSalesLeadsInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  status?: LeadStatus;
  source?: LeadSource;
  priority?: Priority;
  assignedUserId?: string;
  clientId?: string;
  converted?: boolean;
  startDate?: string;
  endDate?: string;
  minEstimatedValue?: number;
  maxEstimatedValue?: number;
  doNotContact?: boolean;
  includeInactive: boolean;
}

export interface GetSalesOpportunitiesInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  stage?: PipelineStage;
  priority?: Priority;
  ownerUserId?: string;
  salesLeadId?: string;
  clientId?: string;
  parentOpportunityId?: string;
  startDate?: string;
  endDate?: string;
  expectedCloseDateStart?: string;
  expectedCloseDateEnd?: string;
  minEstimatedValue?: number;
  maxEstimatedValue?: number;
  minProbability?: number;
  maxProbability?: number;
  isActive?: boolean;
  isClosed?: boolean;
  isOverdue?: boolean;
  competitor?: string;
}

export interface InteractionDto extends AuditedEntityDto<string> {
  subject?: string;
  description?: string;
  type?: InteractionType;
  status?: InteractionStatus;
  priority?: Priority;
  scheduledDate?: string;
  startDate?: string;
  endDate?: string;
  durationMinutes: number;
  location?: string;
  outcome?: string;
  salesLeadId?: string;
  salesLeadName?: string;
  clientId?: string;
  clientName?: string;
  customerId?: string;
  customerName?: string;
  ownerUserId?: string;
  ownerUserName?: string;
  isAllDay: boolean;
  requiresReminder: boolean;
  reminderDate?: string;
  additionalData?: string;
  getTargetEntity?: string;
  getTypeDisplay?: string;
  getStatusDisplay?: string;
  isOverdue: boolean;
  getDurationDisplay?: string;
}

export interface SalesLeadDto extends AuditedEntityDto<string> {
  firstName?: string;
  lastName?: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  company?: string;
  jobTitle?: string;
  description?: string;
  status?: LeadStatus;
  source?: LeadSource;
  priority?: Priority;
  assignedUserId?: string;
  assignedUserName?: string;
  clientId?: string;
  clientName?: string;
  estimatedValue: number;
  expectedCloseDate?: string;
  lastContactDate?: string;
  nextFollowUpDate?: string;
  doNotContact: boolean;
  converted: boolean;
  convertedDate?: string;
  opportunityId?: string;
  getFullName?: string;
  getStatusDisplay?: string;
  getPriorityDisplay?: string;
}

export interface SalesOpportunityDto extends AuditedEntityDto<string> {
  name?: string;
  description?: string;
  stage?: PipelineStage;
  estimatedValue: number;
  probability: number;
  priority?: Priority;
  expectedCloseDate?: string;
  salesLeadId?: string;
  salesLeadName?: string;
  clientId?: string;
  clientName?: string;
  ownerUserId?: string;
  ownerUserName?: string;
  stageChangeDate?: string;
  previousStage?: PipelineStage;
  actualCloseDate?: string;
  actualValue?: number;
  lostReason?: string;
  competitor?: string;
  parentOpportunityId?: string;
  parentOpportunityName?: string;
  isActive: boolean;
  daysInCurrentStage: number;
  getWeightedValue: number;
  isClosed: boolean;
  isOverdue: boolean;
  getStageDisplay?: string;
  getPriorityDisplay?: string;
}

export interface InteractionDto {
  id: string;
  title: string;
  description?: string;
  interactionType: number;
  interactionStatus: number;
  scheduledDate?: string;
  actualStartDate?: string;
  actualEndDate?: string;
  duration?: number;
  location?: string;
  isVirtual: boolean;
  virtualMeetingUrl?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  clientId?: string;
  clientName?: string;
  customerId?: string;
  customerName?: string;
  salesLeadId?: string;
  salesLeadTitle?: string;
  salesOpportunityId?: string;
  salesOpportunityName?: string;
  priority: number;
  result?: string;
  notes?: string;
  reminderMinutes: number;
  isActive: boolean;
  creationTime: string;
  creatorId?: string;
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreateUpdateInteractionDto {
  title: string;
  description?: string;
  interactionType: number;
  interactionStatus: number;
  scheduledDate?: string;
  duration?: number;
  location?: string;
  isVirtual: boolean;
  virtualMeetingUrl?: string;
  assignedUserId?: string;
  clientId?: string;
  customerId?: string;
  salesLeadId?: string;
  salesOpportunityId?: string;
  priority: number;
  notes?: string;
  reminderMinutes: number;
  isActive: boolean;
}

export interface GetInteractionsInput {
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
  filter?: string;
  interactionType?: number;
  interactionStatus?: number;
  assignedUserId?: string;
  clientId?: string;
  customerId?: string;
  salesLeadId?: string;
  salesOpportunityId?: string;
}
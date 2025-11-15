export interface SalesOpportunityDto {
  id: string;
  name: string;
  description?: string;
  clientId?: string;
  clientName?: string;
  customerId?: string;
  customerName?: string;
  salesLeadId?: string;
  salesLeadTitle?: string;
  pipelineStage: number;
  estimatedValue: number;
  actualValue?: number;
  probability: number;
  expectedCloseDate?: string;
  actualCloseDate?: string;
  priority: number;
  assignedUserId?: string;
  assignedUserName?: string;
  isWon: boolean;
  isClosed: boolean;
  closeReason?: string;
  tags?: string;
  notes?: string;
  isActive: boolean;
  creationTime: string;
  creatorId?: string;
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreateUpdateSalesOpportunityDto {
  name: string;
  description?: string;
  clientId?: string;
  customerId?: string;
  salesLeadId?: string;
  pipelineStage: number;
  estimatedValue: number;
  probability: number;
  expectedCloseDate?: string;
  priority: number;
  assignedUserId?: string;
  tags?: string;
  notes?: string;
  isActive: boolean;
}

export interface GetSalesOpportunitiesInput {
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
  filter?: string;
  pipelineStage?: number;
  assignedUserId?: string;
  clientId?: string;
  customerId?: string;
}
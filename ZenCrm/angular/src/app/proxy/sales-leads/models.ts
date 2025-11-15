export interface SalesLeadDto {
  id: string;
  title: string;
  description?: string;
  leadStatus: number;
  leadSource: number;
  estimatedValue?: number;
  estimatedCloseDate?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  clientId?: string;
  clientName?: string;
  customerId?: string;
  customerName?: string;
  probability: number;
  priority: number;
  notes?: string;
  tags?: string;
  isActive: boolean;
  creationTime: string;
  creatorId?: string;
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreateUpdateSalesLeadDto {
  title: string;
  description?: string;
  leadStatus: number;
  leadSource: number;
  estimatedValue?: number;
  estimatedCloseDate?: string;
  assignedUserId?: string;
  clientId?: string;
  customerId?: string;
  probability: number;
  priority: number;
  notes?: string;
  tags?: string;
  isActive: boolean;
}

export interface GetSalesLeadsInput {
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
  filter?: string;
  leadStatus?: number;
  leadSource?: number;
  assignedUserId?: string;
  clientId?: string;
  customerId?: string;
}
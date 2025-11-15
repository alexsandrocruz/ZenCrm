export interface CustomerDto {
  id: string;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  title?: string;
  department?: string;
  clientId?: string;
  clientName?: string;
  isPrimaryContact: boolean;
  isDecisionMaker: boolean;
  notes?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  isActive: boolean;
  creationTime: string;
  creatorId?: string;
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreateUpdateCustomerDto {
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  mobilePhone?: string;
  title?: string;
  department?: string;
  clientId?: string;
  isPrimaryContact: boolean;
  isDecisionMaker: boolean;
  notes?: string;
  assignedUserId?: string;
  isActive: boolean;
}

export interface GetCustomersInput {
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
  filter?: string;
  clientId?: string;
  assignedUserId?: string;
  isDecisionMaker?: boolean;
  isPrimaryContact?: boolean;
}
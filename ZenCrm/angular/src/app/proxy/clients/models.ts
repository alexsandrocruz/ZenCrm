export interface ClientDto {
  id: string;
  name: string;
  clientType: number;
  industry?: number;
  website?: string;
  phone?: string;
  email?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  description?: string;
  annualRevenue?: number;
  numberOfEmployees?: number;
  assignedUserId?: string;
  assignedUserName?: string;
  isActive: boolean;
  creationTime: string;
  creatorId?: string;
  lastModificationTime?: string;
  lastModifierId?: string;
}

export interface CreateUpdateClientDto {
  name: string;
  clientType: number;
  industry?: number;
  website?: string;
  phone?: string;
  email?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  description?: string;
  annualRevenue?: number;
  numberOfEmployees?: number;
  assignedUserId?: string;
  isActive: boolean;
}

export interface GetClientsInput {
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
  filter?: string;
  clientType?: number;
  industry?: number;
  assignedUserId?: string;
}
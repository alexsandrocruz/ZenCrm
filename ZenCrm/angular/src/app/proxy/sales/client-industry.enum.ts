import { mapEnumToOptions } from '@abp/ng.core';

export enum ClientIndustry {
  None = 0,
  Technology = 1,
  Healthcare = 2,
  Finance = 3,
  Manufacturing = 4,
  Retail = 5,
  Education = 6,
  Government = 7,
  RealEstate = 8,
  Transportation = 9,
  Media = 10,
  ProfessionalServices = 11,
  Hospitality = 12,
  Agriculture = 13,
  Energy = 14,
  Consulting = 15,
  Telecommunications = 16,
  Other = 99,
}

export const clientIndustryOptions = mapEnumToOptions(ClientIndustry);

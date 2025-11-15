export enum ClientIndustry {
  Technology = 0,
  Healthcare = 1,
  Finance = 2,
  Manufacturing = 3,
  Retail = 4,
  Education = 5,
  Government = 6,
  Consulting = 7,
  RealEstate = 8,
  Construction = 9,
  Media = 10,
  Hospitality = 11,
  Transportation = 12,
  Energy = 13,
  Agriculture = 14,
  Other = 99
}

export const clientIndustryOptions = [
  { value: ClientIndustry.Technology, label: 'Technology' },
  { value: ClientIndustry.Healthcare, label: 'Healthcare' },
  { value: ClientIndustry.Finance, label: 'Finance' },
  { value: ClientIndustry.Manufacturing, label: 'Manufacturing' },
  { value: ClientIndustry.Retail, label: 'Retail' },
  { value: ClientIndustry.Education, label: 'Education' },
  { value: ClientIndustry.Government, label: 'Government' },
  { value: ClientIndustry.Consulting, label: 'Consulting' },
  { value: ClientIndustry.RealEstate, label: 'Real Estate' },
  { value: ClientIndustry.Construction, label: 'Construction' },
  { value: ClientIndustry.Media, label: 'Media' },
  { value: ClientIndustry.Hospitality, label: 'Hospitality' },
  { value: ClientIndustry.Transportation, label: 'Transportation' },
  { value: ClientIndustry.Energy, label: 'Energy' },
  { value: ClientIndustry.Agriculture, label: 'Agriculture' },
  { value: ClientIndustry.Other, label: 'Other' }
];
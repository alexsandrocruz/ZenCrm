export enum LeadStatus {
  New = 0,
  Qualifying = 1,
  Qualified = 2,
  Converted = 3,
  Lost = 4,
  Recycled = 5
}

export const leadStatusOptions = [
  { value: LeadStatus.New, label: 'New' },
  { value: LeadStatus.Qualifying, label: 'Qualifying' },
  { value: LeadStatus.Qualified, label: 'Qualified' },
  { value: LeadStatus.Converted, label: 'Converted' },
  { value: LeadStatus.Lost, label: 'Lost' },
  { value: LeadStatus.Recycled, label: 'Recycled' }
];
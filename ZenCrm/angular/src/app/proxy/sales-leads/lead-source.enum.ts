export enum LeadSource {
  Website = 0,
  Referral = 1,
  ColdCall = 2,
  Email = 3,
  SocialMedia = 4,
  TradeShow = 5,
  Partner = 6,
  Advertisement = 7,
  ExistingClient = 8,
  Other = 99
}

export const leadSourceOptions = [
  { value: LeadSource.Website, label: 'Website' },
  { value: LeadSource.Referral, label: 'Referral' },
  { value: LeadSource.ColdCall, label: 'Cold Call' },
  { value: LeadSource.Email, label: 'Email' },
  { value: LeadSource.SocialMedia, label: 'Social Media' },
  { value: LeadSource.TradeShow, label: 'Trade Show' },
  { value: LeadSource.Partner, label: 'Partner' },
  { value: LeadSource.Advertisement, label: 'Advertisement' },
  { value: LeadSource.ExistingClient, label: 'Existing Client' },
  { value: LeadSource.Other, label: 'Other' }
];
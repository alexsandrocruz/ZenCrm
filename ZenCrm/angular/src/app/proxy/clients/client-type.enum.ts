export enum ClientType {
  Individual = 0,
  Business = 1,
  Government = 2,
  NonProfit = 3,
  Educational = 4
}

export const clientTypeOptions = [
  { value: ClientType.Individual, label: 'Individual' },
  { value: ClientType.Business, label: 'Business' },
  { value: ClientType.Government, label: 'Government' },
  { value: ClientType.NonProfit, label: 'NonProfit' },
  { value: ClientType.Educational, label: 'Educational' }
];
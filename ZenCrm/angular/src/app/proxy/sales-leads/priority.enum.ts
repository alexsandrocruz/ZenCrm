export enum Priority {
  Low = 0,
  Normal = 1,
  High = 2,
  Critical = 3
}

export const priorityOptions = [
  { value: Priority.Low, label: 'Low' },
  { value: Priority.Normal, label: 'Normal' },
  { value: Priority.High, label: 'High' },
  { value: Priority.Critical, label: 'Critical' }
];
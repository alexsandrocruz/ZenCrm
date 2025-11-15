export enum InteractionStatus {
  Scheduled = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3,
  Postponed = 4,
  NoShow = 5
}

export const interactionStatusOptions = [
  { value: InteractionStatus.Scheduled, label: 'Scheduled' },
  { value: InteractionStatus.InProgress, label: 'In Progress' },
  { value: InteractionStatus.Completed, label: 'Completed' },
  { value: InteractionStatus.Cancelled, label: 'Cancelled' },
  { value: InteractionStatus.Postponed, label: 'Postponed' },
  { value: InteractionStatus.NoShow, label: 'No Show' }
];
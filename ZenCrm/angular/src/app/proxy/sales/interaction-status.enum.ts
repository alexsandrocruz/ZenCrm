import { mapEnumToOptions } from '@abp/ng.core';

export enum InteractionStatus {
  Scheduled = 1,
  InProgress = 2,
  Completed = 3,
  Cancelled = 4,
  Postponed = 5,
  Failed = 6,
  Pending = 7,
}

export const interactionStatusOptions = mapEnumToOptions(InteractionStatus);

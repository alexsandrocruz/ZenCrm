import { mapEnumToOptions } from '@abp/ng.core';

export enum Priority {
  Low = 1,
  Normal = 2,
  High = 3,
  Critical = 4,
}

export const priorityOptions = mapEnumToOptions(Priority);

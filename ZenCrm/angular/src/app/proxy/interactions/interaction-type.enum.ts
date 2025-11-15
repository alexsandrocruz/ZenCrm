export enum InteractionType {
  PhoneCall = 0,
  Email = 1,
  Meeting = 2,
  VideoConference = 3,
  Presentation = 4,
  SiteVisit = 5,
  LunchMeeting = 6,
  Conference = 7,
  Webinar = 8,
  Task = 9,
  FollowUp = 10,
  Other = 99
}

export const interactionTypeOptions = [
  { value: InteractionType.PhoneCall, label: 'Phone Call' },
  { value: InteractionType.Email, label: 'Email' },
  { value: InteractionType.Meeting, label: 'Meeting' },
  { value: InteractionType.VideoConference, label: 'Video Conference' },
  { value: InteractionType.Presentation, label: 'Presentation' },
  { value: InteractionType.SiteVisit, label: 'Site Visit' },
  { value: InteractionType.LunchMeeting, label: 'Lunch Meeting' },
  { value: InteractionType.Conference, label: 'Conference' },
  { value: InteractionType.Webinar, label: 'Webinar' },
  { value: InteractionType.Task, label: 'Task' },
  { value: InteractionType.FollowUp, label: 'Follow Up' },
  { value: InteractionType.Other, label: 'Other' }
];
export enum PipelineStage {
  Lead = 0,
  Qualifying = 1,
  Qualified = 2,
  Analysis = 3,
  Proposal = 4,
  ProposalSent = 5,
  Negotiation = 6,
  VerbalCommitment = 7,
  Closing = 8,
  Won = 9,
  Lost = 10,
  OnHold = 11
}

export const pipelineStageOptions = [
  { value: PipelineStage.Lead, label: 'Lead' },
  { value: PipelineStage.Qualifying, label: 'Qualifying' },
  { value: PipelineStage.Qualified, label: 'Qualified' },
  { value: PipelineStage.Analysis, label: 'Analysis' },
  { value: PipelineStage.Proposal, label: 'Proposal' },
  { value: PipelineStage.ProposalSent, label: 'Proposal Sent' },
  { value: PipelineStage.Negotiation, label: 'Negotiation' },
  { value: PipelineStage.VerbalCommitment, label: 'Verbal Commitment' },
  { value: PipelineStage.Closing, label: 'Closing' },
  { value: PipelineStage.Won, label: 'Won' },
  { value: PipelineStage.Lost, label: 'Lost' },
  { value: PipelineStage.OnHold, label: 'On Hold' }
];
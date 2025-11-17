using System;
using Volo.Abp.Application.Dtos;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class SalesOpportunityDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public PipelineStage Stage { get; set; }

    public decimal EstimatedValue { get; set; }

    public decimal Probability { get; set; }

    public Priority Priority { get; set; }

    public DateTime ExpectedCloseDate { get; set; }

    public Guid SalesLeadId { get; set; }

    public string? SalesLeadName { get; set; }

    public Guid? ClientId { get; set; }

    public string? ClientName { get; set; }

    public Guid OwnerUserId { get; set; }

    public string? OwnerUserName { get; set; }

    public DateTime StageChangeDate { get; set; }

    public PipelineStage? PreviousStage { get; set; }

    public DateTime? ActualCloseDate { get; set; }

    public decimal? ActualValue { get; set; }

    public string? LostReason { get; set; }

    public string? Competitor { get; set; }

    public Guid? ParentOpportunityId { get; set; }

    public string? ParentOpportunityName { get; set; }

    public bool IsActive { get; set; }

    public int DaysInCurrentStage { get; set; }

    public decimal GetWeightedValue => EstimatedValue * (Probability / 100);

    public bool IsClosed => Stage == PipelineStage.Won || Stage == PipelineStage.Lost;

    public bool IsOverdue => !IsClosed && ExpectedCloseDate < DateTime.UtcNow.Date;

    public string GetStageDisplay => Stage.ToString();

    public string GetPriorityDisplay => Priority.ToString();
}
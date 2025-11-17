using System;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Sales;

public class GetSalesOpportunitiesInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public PipelineStage? Stage { get; set; }

    public Priority? Priority { get; set; }

    public Guid? OwnerUserId { get; set; }

    public Guid? SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? ParentOpportunityId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpectedCloseDateStart { get; set; }

    public DateTime? ExpectedCloseDateEnd { get; set; }

    public decimal? MinEstimatedValue { get; set; }

    public decimal? MaxEstimatedValue { get; set; }

    public int? MinProbability { get; set; }

    public int? MaxProbability { get; set; }

    public bool? IsActive { get; set; } = true;

    public bool? IsClosed { get; set; }

    public bool? IsOverdue { get; set; }

    public string? Competitor { get; set; }
}
using System;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Sales;

public class GetSalesLeadsInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public LeadStatus? Status { get; set; }

    public LeadSource? Source { get; set; }

    public Priority? Priority { get; set; }

    public Guid? AssignedUserId { get; set; }

    public Guid? ClientId { get; set; }

    public bool? Converted { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? MinEstimatedValue { get; set; }

    public decimal? MaxEstimatedValue { get; set; }

    public bool? DoNotContact { get; set; }

    public bool IncludeInactive { get; set; } = false;
}
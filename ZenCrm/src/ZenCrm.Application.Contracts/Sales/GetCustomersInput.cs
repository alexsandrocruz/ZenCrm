using System;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Sales;

public class GetCustomersInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? AssignedUserId { get; set; }

    public bool? IsActive { get; set; } = true;

    public bool? IsPrimaryContact { get; set; }

    public bool? IsKeyDecisionMaker { get; set; }

    public string? JobTitle { get; set; }

    public string? Department { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IncludeInactive { get; set; } = false;
}
using System;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Sales;

public class GetClientsInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public ClientType? Type { get; set; }

    public ClientIndustry? Industry { get; set; }

    public Guid? AssignedUserId { get; set; }

    public bool? IsActive { get; set; } = true;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? MinAnnualRevenue { get; set; }

    public decimal? MaxAnnualRevenue { get; set; }

    public int? MinEmployees { get; set; }

    public int? MaxEmployees { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }
}
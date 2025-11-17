using System;
using Volo.Abp.Application.Dtos;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class SalesLeadDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? MobilePhone { get; set; }

    public string? Company { get; set; }

    public string? JobTitle { get; set; }

    public string? Description { get; set; }

    public LeadStatus Status { get; set; }

    public LeadSource Source { get; set; }

    public Priority Priority { get; set; }

    public Guid? AssignedUserId { get; set; }

    public string? AssignedUserName { get; set; }

    public Guid? ClientId { get; set; }

    public string? ClientName { get; set; }

    public decimal EstimatedValue { get; set; }

    public DateTime? ExpectedCloseDate { get; set; }

    public DateTime? LastContactDate { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public bool DoNotContact { get; set; }

    public bool Converted { get; set; }

    public DateTime? ConvertedDate { get; set; }

    public Guid? OpportunityId { get; set; }

    public string GetFullName => $"{FirstName} {LastName}".Trim();

    public string GetStatusDisplay => Status.ToString();

    public string GetPriorityDisplay => Priority.ToString();
}
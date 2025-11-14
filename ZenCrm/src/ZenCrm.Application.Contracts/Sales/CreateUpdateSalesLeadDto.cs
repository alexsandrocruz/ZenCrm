using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CreateUpdateSalesLeadDto
{
    [Required]
    [StringLength(128)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(32)]
    [Phone]
    public string? Phone { get; set; }

    [StringLength(32)]
    [Phone]
    public string? MobilePhone { get; set; }

    [StringLength(512)]
    public string? Company { get; set; }

    [StringLength(128)]
    public string? JobTitle { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.New;

    public LeadSource Source { get; set; } = LeadSource.Website;

    public Priority Priority { get; set; } = Priority.Normal;

    public Guid? AssignedUserId { get; set; }

    public Guid? ClientId { get; set; }

    public decimal EstimatedValue { get; set; } = 0;

    public DateTime? ExpectedCloseDate { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public bool DoNotContact { get; set; } = false;
}
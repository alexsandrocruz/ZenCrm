using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CreateUpdateSalesOpportunityDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public decimal EstimatedValue { get; set; } = 0;

    public Priority Priority { get; set; } = Priority.Normal;

    public DateTime ExpectedCloseDate { get; set; }

    public Guid SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid OwnerUserId { get; set; }

    [StringLength(512)]
    public string? Competitor { get; set; }

    public Guid? ParentOpportunityId { get; set; }

    public bool IsActive { get; set; } = true;
}
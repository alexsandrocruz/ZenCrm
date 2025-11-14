using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CreateUpdateCustomerDto
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

    [StringLength(128)]
    public string? JobTitle { get; set; }

    [StringLength(256)]
    public string? Department { get; set; }

    [StringLength(512)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsPrimaryContact { get; set; } = false;

    public bool IsKeyDecisionMaker { get; set; } = false;

    public Guid? ClientId { get; set; }

    public Guid? AssignedUserId { get; set; }
}
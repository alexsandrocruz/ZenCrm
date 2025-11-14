using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CreateUpdateClientDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [StringLength(32)]
    public string? DocumentNumber { get; set; }

    public ClientType Type { get; set; } = ClientType.Business;

    public ClientIndustry Industry { get; set; } = ClientIndustry.None;

    [StringLength(512)]
    public string? Description { get; set; }

    [StringLength(256)]
    public string? Website { get; set; }

    [StringLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(32)]
    [Phone]
    public string? Phone { get; set; }

    [StringLength(1024)]
    public string? Address { get; set; }

    [StringLength(256)]
    public string? City { get; set; }

    [StringLength(128)]
    public string? State { get; set; }

    [StringLength(64)]
    public string? PostalCode { get; set; }

    [StringLength(128)]
    public string? Country { get; set; }

    public decimal AnnualRevenue { get; set; } = 0;

    public int NumberOfEmployees { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public Guid? AssignedUserId { get; set; }
}
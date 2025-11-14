using System;
using Volo.Abp.Application.Dtos;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CustomerDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? MobilePhone { get; set; }

    public string? JobTitle { get; set; }

    public string? Department { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public bool IsPrimaryContact { get; set; }

    public bool IsKeyDecisionMaker { get; set; }

    public Guid? ClientId { get; set; }

    public string? ClientName { get; set; }

    public DateTime? LastContactDate { get; set; }

    public Guid? AssignedUserId { get; set; }

    public string? AssignedUserName { get; set; }

    public int InteractionCount { get; set; }

    public DateTime? LastInteractionDate { get; set; }

    public string GetFullName => $"{FirstName} {LastName}".Trim();

    public string GetDisplayName
    {
        get
        {
            var name = GetFullName;
            if (!string.IsNullOrWhiteSpace(JobTitle))
            {
                return $"{name} - {JobTitle}";
            }
            return name;
        }
    }
}
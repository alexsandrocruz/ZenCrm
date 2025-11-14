using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class SalesLead : AuditedAggregateRoot<Guid>
{
    [Required]
    [StringLength(128)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string LastName { get; set; } = string.Empty;

    [CanBeNull]
    [StringLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [CanBeNull]
    [StringLength(32)]
    [Phone]
    public string? Phone { get; set; }

    [CanBeNull]
    [StringLength(32)]
    public string? MobilePhone { get; set; }

    [CanBeNull]
    [StringLength(512)]
    public string? Company { get; set; }

    [CanBeNull]
    [StringLength(128)]
    public string? JobTitle { get; set; }

    [CanBeNull]
    [StringLength(2000)]
    public string? Description { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.New;

    public LeadSource Source { get; set; } = LeadSource.Website;

    public Priority Priority { get; set; } = Priority.Normal;

    public Guid? AssignedUserId { get; set; }

    public Guid? ClientId { get; set; }

    public decimal EstimatedValue { get; set; } = 0;

    public DateTime? ExpectedCloseDate { get; set; }

    public DateTime? LastContactDate { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public bool DoNotContact { get; set; } = false;

    public bool Converted { get; set; } = false;

    public DateTime? ConvertedDate { get; set; }

    public Guid? OpportunityId { get; set; }

    protected SalesLead()
    {
    }

    public SalesLead(
        Guid id,
        string firstName,
        string lastName,
        string? email = null,
        string? phone = null,
        string? company = null
    ) : base(id)
    {
        SetName(firstName, lastName);
        Email = email;
        Phone = phone;
        Company = company;
    }

    public SalesLead SetName(string firstName, string lastName)
    {
        FirstName = Check.NotNullOrWhiteSpace(firstName, nameof(firstName), maxLength: 128);
        LastName = Check.NotNullOrWhiteSpace(lastName, nameof(lastName), maxLength: 128);
        return this;
    }

    public SalesLead SetEmail(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!email.Contains('@'))
            {
                throw new BusinessException("Email format is invalid");
            }
        }
        Email = email;
        return this;
    }

    public SalesLead SetPhone(string? phone)
    {
        Phone = phone;
        return this;
    }

    public SalesLead SetStatus(LeadStatus status)
    {
        if (Status == LeadStatus.Converted && status != LeadStatus.Converted)
        {
            throw new BusinessException("Cannot change status of converted lead");
        }

        Status = status;
        return this;
    }

    public SalesLead AssignToUser(Guid userId)
    {
        AssignedUserId = userId;
        return this;
    }

    public SalesLead SetEstimatedValue(decimal value)
    {
        if (value < 0)
        {
            throw new BusinessException("Estimated value cannot be negative");
        }
        EstimatedValue = value;
        return this;
    }

    public SalesLead SetFollowUpDate(DateTime? followUpDate)
    {
        if (followUpDate.HasValue && followUpDate.Value < DateTime.UtcNow.Date)
        {
            throw new BusinessException("Follow-up date cannot be in the past");
        }
        NextFollowUpDate = followUpDate;
        return this;
    }

    public SalesLead UpdateLastContact()
    {
        LastContactDate = DateTime.UtcNow;
        return this;
    }

    public SalesLead MarkAsDoNotContact()
    {
        DoNotContact = true;
        return this;
    }

    public SalesLead ConvertToOpportunity(Guid opportunityId)
    {
        if (Converted)
        {
            throw new BusinessException("Lead is already converted");
        }

        Converted = true;
        ConvertedDate = DateTime.UtcNow;
        OpportunityId = opportunityId;
        Status = LeadStatus.Converted;

        return this;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }
}
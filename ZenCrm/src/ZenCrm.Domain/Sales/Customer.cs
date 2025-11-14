using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class Customer : AuditedAggregateRoot<Guid>
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
    [Phone]
    public string? MobilePhone { get; set; }

    [CanBeNull]
    [StringLength(128)]
    public string? JobTitle { get; set; }

    [CanBeNull]
    [StringLength(256)]
    public string? Department { get; set; }

    [CanBeNull]
    [StringLength(512)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsPrimaryContact { get; set; } = false;

    public bool IsKeyDecisionMaker { get; set; } = false;

    public Guid? ClientId { get; set; }

    public DateTime? LastContactDate { get; set; }

    public Guid? AssignedUserId { get; set; }

    protected Customer()
    {
    }

    public Customer(
        Guid id,
        string firstName,
        string lastName,
        Guid? clientId = null
    ) : base(id)
    {
        SetName(firstName, lastName);
        ClientId = clientId;
    }

    public Customer SetName(string firstName, string lastName)
    {
        FirstName = Check.NotNullOrWhiteSpace(firstName, nameof(firstName), maxLength: 128);
        LastName = Check.NotNullOrWhiteSpace(lastName, nameof(lastName), maxLength: 128);
        return this;
    }

    public Customer SetEmail(string? email)
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

    public Customer SetPhone(string? phone, string? mobilePhone = null)
    {
        Phone = phone;
        MobilePhone = mobilePhone;
        return this;
    }

    public Customer SetJobInfo(string? jobTitle, string? department)
    {
        JobTitle = jobTitle?.Trim();
        Department = department?.Trim();
        return this;
    }

    public Customer SetClient(Guid clientId)
    {
        ClientId = clientId;
        return this;
    }

    public Customer AssignToUser(Guid? userId)
    {
        AssignedUserId = userId;
        return this;
    }

    public Customer SetAsPrimaryContact(bool isPrimary)
    {
        IsPrimaryContact = isPrimary;
        return this;
    }

    public Customer SetAsKeyDecisionMaker(bool isKeyDecisionMaker)
    {
        IsKeyDecisionMaker = isKeyDecisionMaker;
        return this;
    }

    public Customer SetStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public Customer UpdateLastContact()
    {
        LastContactDate = DateTime.UtcNow;
        return this;
    }

    public Customer AddNotes(string notes)
    {
        if (!string.IsNullOrWhiteSpace(notes))
        {
            if (string.IsNullOrWhiteSpace(Notes))
            {
                Notes = notes.Trim();
            }
            else
            {
                Notes += Environment.NewLine + Environment.NewLine + $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] " + notes.Trim();
            }
        }
        return this;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    public string GetDisplayName()
    {
        var name = GetFullName();
        if (!string.IsNullOrWhiteSpace(JobTitle))
        {
            return $"{name} - {JobTitle}";
        }
        return name;
    }
}
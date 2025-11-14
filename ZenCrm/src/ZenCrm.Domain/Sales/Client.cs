using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class Client : AuditedAggregateRoot<Guid>
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [StringLength(32)]
    public string? DocumentNumber { get; set; }

    public ClientType Type { get; set; } = ClientType.Business;

    public ClientIndustry Industry { get; set; } = ClientIndustry.None;

    [CanBeNull]
    [StringLength(512)]
    public string? Description { get; set; }

    [CanBeNull]
    [StringLength(256)]
    public string? Website { get; set; }

    [CanBeNull]
    [StringLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    [CanBeNull]
    [StringLength(32)]
    [Phone]
    public string? Phone { get; set; }

    [CanBeNull]
    [StringLength(1024)]
    public string? Address { get; set; }

    [CanBeNull]
    [StringLength(256)]
    public string? City { get; set; }

    [CanBeNull]
    [StringLength(128)]
    public string? State { get; set; }

    [CanBeNull]
    [StringLength(64)]
    public string? PostalCode { get; set; }

    [CanBeNull]
    [StringLength(128)]
    public string? Country { get; set; }

    public decimal AnnualRevenue { get; set; } = 0;

    public int NumberOfEmployees { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public Guid? AssignedUserId { get; set; }

    public DateTime? LastInteractionDate { get; set; }

    protected Client()
    {
    }

    public Client(
        Guid id,
        string name,
        ClientType type = ClientType.Business
    ) : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 256);
        Type = type;
    }

    public Client SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 256);
        return this;
    }

    public Client SetDocumentNumber(string? documentNumber)
    {
        if (!string.IsNullOrWhiteSpace(documentNumber))
        {
            documentNumber = documentNumber.Trim();
            // Basic validation - you might want to add more sophisticated validation
            if (documentNumber.Length > 32)
            {
                throw new BusinessException("Document number cannot be longer than 32 characters");
            }
        }
        DocumentNumber = documentNumber;
        return this;
    }

    public Client SetIndustry(ClientIndustry industry)
    {
        Industry = industry;
        return this;
    }

    public Client SetContactInfo(string? email, string? phone, string? website)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!email.Contains('@'))
            {
                throw new BusinessException("Email format is invalid");
            }
        }
        Email = email;
        Phone = phone;
        Website = website;
        return this;
    }

    public Client SetAddress(string? address, string? city, string? state, string? postalCode, string? country)
    {
        Address = address?.Trim();
        City = city?.Trim();
        State = state?.Trim();
        PostalCode = postalCode?.Trim();
        Country = country?.Trim();
        return this;
    }

    public Client SetFinancialInfo(decimal annualRevenue, int numberOfEmployees)
    {
        if (annualRevenue < 0)
        {
            throw new BusinessException("Annual revenue cannot be negative");
        }
        if (numberOfEmployees < 0)
        {
            throw new BusinessException("Number of employees cannot be negative");
        }
        AnnualRevenue = annualRevenue;
        NumberOfEmployees = numberOfEmployees;
        return this;
    }

    public Client AssignToUser(Guid? userId)
    {
        AssignedUserId = userId;
        return this;
    }

    public Client SetStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public Client UpdateLastInteraction()
    {
        LastInteractionDate = DateTime.UtcNow;
        return this;
    }

    public string GetFullAddress()
    {
        var addressParts = new List<string>();

        if (!string.IsNullOrWhiteSpace(Address))
            addressParts.Add(Address);

        var cityStateZip = new List<string>();
        if (!string.IsNullOrWhiteSpace(City))
            cityStateZip.Add(City);
        if (!string.IsNullOrWhiteSpace(State))
            cityStateZip.Add(State);
        if (!string.IsNullOrWhiteSpace(PostalCode))
            cityStateZip.Add(PostalCode);

        if (cityStateZip.Any())
            addressParts.Add(string.Join(", ", cityStateZip));

        if (!string.IsNullOrWhiteSpace(Country))
            addressParts.Add(Country);

        return string.Join(", ", addressParts);
    }
}
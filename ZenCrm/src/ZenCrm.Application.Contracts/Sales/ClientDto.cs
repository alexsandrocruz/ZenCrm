using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class ClientDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? DocumentNumber { get; set; }

    public ClientType Type { get; set; }

    public ClientIndustry Industry { get; set; }

    public string? Description { get; set; }

    public string? Website { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public decimal AnnualRevenue { get; set; }

    public int NumberOfEmployees { get; set; }

    public bool IsActive { get; set; }

    public Guid? AssignedUserId { get; set; }

    public string? AssignedUserName { get; set; }

    public DateTime? LastInteractionDate { get; set; }

    public int CustomerCount { get; set; }

    public int OpportunityCount { get; set; }

    public decimal TotalOpportunityValue { get; set; }

    public string GetFullAddress
    {
        get
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

    public string GetTypeDisplay => Type.ToString();

    public string GetIndustryDisplay => Industry.ToString();
}
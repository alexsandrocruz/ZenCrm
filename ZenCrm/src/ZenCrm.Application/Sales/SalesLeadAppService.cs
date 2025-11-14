using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using ZenCrm.Permissions;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

[Authorize(ZenCrmPermissions.SalesLeads.Default)]
public class SalesLeadAppService : ApplicationService, ISalesLeadAppService
{
    private readonly IRepository<SalesLead, Guid> _salesLeadRepository;

    public SalesLeadAppService(IRepository<SalesLead, Guid> salesLeadRepository)
    {
        _salesLeadRepository = salesLeadRepository;
    }

    public async Task<PagedResultDto<SalesLeadDto>> GetListAsync(GetSalesLeadsInput input)
    {
        IQueryable<SalesLead> queryable = await _salesLeadRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x =>
                x.FirstName.Contains(input.Filter) ||
                x.LastName.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.Company.Contains(input.Filter))
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value)
            .WhereIf(input.Source.HasValue, x => x.Source == input.Source.Value)
            .WhereIf(input.Priority.HasValue, x => x.Priority == input.Priority.Value)
            .WhereIf(input.AssignedUserId.HasValue, x => x.AssignedUserId == input.AssignedUserId.Value)
            .WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId.Value)
            .WhereIf(input.Converted.HasValue, x => x.Converted == input.Converted.Value)
            .WhereIf(input.StartDate.HasValue, x => x.CreationTime >= input.StartDate.Value)
            .WhereIf(input.EndDate.HasValue, x => x.CreationTime <= input.EndDate.Value.AddDays(1).AddTicks(-1))
            .WhereIf(input.MinEstimatedValue.HasValue, x => x.EstimatedValue >= input.MinEstimatedValue.Value)
            .WhereIf(input.MaxEstimatedValue.HasValue, x => x.EstimatedValue <= input.MaxEstimatedValue.Value)
            .WhereIf(input.DoNotContact.HasValue, x => x.DoNotContact == input.DoNotContact.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderByDescending<SalesLead, DateTime>(x => x.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var leads = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<SalesLeadDto>(
            totalCount,
            ObjectMapper.Map<List<SalesLead>, List<SalesLeadDto>>(leads)
        );
    }

    public async Task<SalesLeadDto> GetAsync(Guid id)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Create)]
    public async Task<SalesLeadDto> CreateAsync(CreateUpdateSalesLeadDto input)
    {
        var lead = ObjectMapper.Map<CreateUpdateSalesLeadDto, SalesLead>(input);

        await _salesLeadRepository.InsertAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Edit)]
    public async Task<SalesLeadDto> UpdateAsync(Guid id, CreateUpdateSalesLeadDto input)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        ObjectMapper.Map(input, lead);

        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _salesLeadRepository.DeleteAsync(id);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Assign)]
    public async Task<SalesLeadDto> AssignToUserAsync(Guid id, Guid userId)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.AssignToUser(userId);
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Edit)]
    public async Task<SalesLeadDto> UpdateStatusAsync(Guid id, LeadStatus status)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.SetStatus(status);
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Convert)]
    public async Task<SalesLeadDto> ConvertToOpportunityAsync(Guid id)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.ConvertToOpportunity(Guid.NewGuid());
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Edit)]
    public async Task<SalesLeadDto> SetFollowUpDateAsync(Guid id, DateTime followUpDate)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.SetFollowUpDate(followUpDate);
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    [Authorize(ZenCrmPermissions.SalesLeads.Edit)]
    public async Task<SalesLeadDto> MarkAsDoNotContactAsync(Guid id, bool doNotContact = true)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.MarkAsDoNotContact();
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    public async Task<SalesLeadDto> UpdateLastContactAsync(Guid id)
    {
        var lead = await _salesLeadRepository.GetAsync(id);

        lead.UpdateLastContact();
        await _salesLeadRepository.UpdateAsync(lead);

        return ObjectMapper.Map<SalesLead, SalesLeadDto>(lead);
    }

    public async Task<PagedResultDto<SalesLeadDto>> GetMyLeadsAsync(GetSalesLeadsInput input)
    {
        var currentUserId = CurrentUser.Id ?? Guid.Empty;
        input.AssignedUserId = currentUserId;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesLeadDto>> GetUnassignedLeadsAsync(GetSalesLeadsInput input)
    {
        input.AssignedUserId = null;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesLeadDto>> GetLeadsNeedingFollowUpAsync(GetSalesLeadsInput input)
    {
        IQueryable<SalesLead> queryable = await _salesLeadRepository.GetQueryableAsync();

        queryable = queryable
            .Where(x => !x.Converted &&
                       !x.DoNotContact &&
                       x.NextFollowUpDate.HasValue &&
                       x.NextFollowUpDate.Value <= DateTime.UtcNow)
            .WhereIf(input.AssignedUserId.HasValue, x => x.AssignedUserId == input.AssignedUserId.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy(x => x.NextFollowUpDate.HasValue ? x.NextFollowUpDate.Value : DateTime.MaxValue)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var leads = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<SalesLeadDto>(
            totalCount,
            ObjectMapper.Map<List<SalesLead>, List<SalesLeadDto>>(leads)
        );
    }

    public Task<ListResultDto<LeadStatus>> GetStatusOptionsAsync()
    {
        var statuses = Enum.GetValues(typeof(LeadStatus)).Cast<LeadStatus>().ToList();
        return Task.FromResult(new ListResultDto<LeadStatus>(statuses));
    }

    public Task<ListResultDto<LeadSource>> GetSourceOptionsAsync()
    {
        var sources = Enum.GetValues(typeof(LeadSource)).Cast<LeadSource>().ToList();
        return Task.FromResult(new ListResultDto<LeadSource>(sources));
    }
}
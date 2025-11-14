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

[Authorize(ZenCrmPermissions.SalesOpportunities.Default)]
public class SalesOpportunityAppService : ApplicationService, ISalesOpportunityAppService
{
    private readonly IRepository<SalesOpportunity, Guid> _salesOpportunityRepository;

    public SalesOpportunityAppService(IRepository<SalesOpportunity, Guid> salesOpportunityRepository)
    {
        _salesOpportunityRepository = salesOpportunityRepository;
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetListAsync(GetSalesOpportunitiesInput input)
    {
        IQueryable<SalesOpportunity> queryable = await _salesOpportunityRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x =>
                x.Name.Contains(input.Filter) ||
                x.Description.Contains(input.Filter) ||
                x.Competitor.Contains(input.Filter))
            .WhereIf(input.Stage.HasValue, x => x.Stage == input.Stage.Value)
            .WhereIf(input.Priority.HasValue, x => x.Priority == input.Priority.Value)
            .WhereIf(input.OwnerUserId.HasValue, x => x.OwnerUserId == input.OwnerUserId.Value)
            .WhereIf(input.SalesLeadId.HasValue, x => x.SalesLeadId == input.SalesLeadId.Value)
            .WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId.Value)
            .WhereIf(input.ParentOpportunityId.HasValue, x => x.ParentOpportunityId == input.ParentOpportunityId.Value)
            .WhereIf(input.StartDate.HasValue, x => x.CreationTime >= input.StartDate.Value)
            .WhereIf(input.EndDate.HasValue, x => x.CreationTime <= input.EndDate.Value.AddDays(1).AddTicks(-1))
            .WhereIf(input.ExpectedCloseDateStart.HasValue, x => x.ExpectedCloseDate >= input.ExpectedCloseDateStart.Value)
            .WhereIf(input.ExpectedCloseDateEnd.HasValue, x => x.ExpectedCloseDate <= input.ExpectedCloseDateEnd.Value.AddDays(1).AddTicks(-1))
            .WhereIf(input.MinEstimatedValue.HasValue, x => x.EstimatedValue >= input.MinEstimatedValue.Value)
            .WhereIf(input.MaxEstimatedValue.HasValue, x => x.EstimatedValue <= input.MaxEstimatedValue.Value)
            .WhereIf(input.MinProbability.HasValue, x => x.Probability >= input.MinProbability.Value)
            .WhereIf(input.MaxProbability.HasValue, x => x.Probability <= input.MaxProbability.Value)
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Competitor), x => x.Competitor == input.Competitor)
            .WhereIf(input.IsClosed.HasValue, x => x.IsClosed() == input.IsClosed.Value)
            .WhereIf(input.IsOverdue.HasValue, x => x.IsOverdue() == input.IsOverdue.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy<SalesOpportunity, DateTime?>(x => x.ExpectedCloseDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var opportunities = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<SalesOpportunityDto>(
            totalCount,
            ObjectMapper.Map<List<SalesOpportunity>, List<SalesOpportunityDto>>(opportunities)
        );
    }

    public async Task<SalesOpportunityDto> GetAsync(Guid id)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Create)]
    public async Task<SalesOpportunityDto> CreateAsync(CreateUpdateSalesOpportunityDto input)
    {
        var opportunity = ObjectMapper.Map<CreateUpdateSalesOpportunityDto, SalesOpportunity>(input);

        await _salesOpportunityRepository.InsertAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Edit)]
    public async Task<SalesOpportunityDto> UpdateAsync(Guid id, CreateUpdateSalesOpportunityDto input)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        ObjectMapper.Map(input, opportunity);

        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _salesOpportunityRepository.DeleteAsync(id);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.AdvanceStage)]
    public async Task<SalesOpportunityDto> MoveToStageAsync(Guid id, PipelineStage newStage)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.MoveToStage(newStage);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Close)]
    public async Task<SalesOpportunityDto> CloseWonAsync(Guid id, decimal? actualValue = null)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.CloseWon(actualValue);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Close)]
    public async Task<SalesOpportunityDto> CloseLostAsync(Guid id, string lostReason, string? competitor = null)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.CloseLost(lostReason, competitor);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.SetPriority)]
    public async Task<SalesOpportunityDto> SetPriorityAsync(Guid id, Priority priority)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.SetPriority(priority);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Edit)]
    public async Task<SalesOpportunityDto> UpdateExpectedValueAsync(Guid id, decimal value)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.SetEstimatedValue(value);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Edit)]
    public async Task<SalesOpportunityDto> UpdateExpectedCloseDateAsync(Guid id, DateTime date)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.SetExpectedCloseDate(date);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Edit)]
    public async Task<SalesOpportunityDto> AssociateWithClientAsync(Guid id, Guid clientId)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.AssociateWithClient(clientId);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    [Authorize(ZenCrmPermissions.SalesOpportunities.Edit)]
    public async Task<SalesOpportunityDto> SetParentOpportunityAsync(Guid id, Guid? parentOpportunityId)
    {
        var opportunity = await _salesOpportunityRepository.GetAsync(id);

        opportunity.SetParentOpportunity(parentOpportunityId);
        await _salesOpportunityRepository.UpdateAsync(opportunity);

        return ObjectMapper.Map<SalesOpportunity, SalesOpportunityDto>(opportunity);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetMyOpportunitiesAsync(GetSalesOpportunitiesInput input)
    {
        var currentUserId = CurrentUser.Id ?? Guid.Empty;
        input.OwnerUserId = currentUserId;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetByStageAsync(PipelineStage stage, GetSalesOpportunitiesInput input)
    {
        input.Stage = stage;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetByOwnerAsync(Guid ownerId, GetSalesOpportunitiesInput input)
    {
        input.OwnerUserId = ownerId;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetWonOpportunitiesAsync(GetSalesOpportunitiesInput input)
    {
        input.Stage = PipelineStage.Won;
        input.IsClosed = true;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetLostOpportunitiesAsync(GetSalesOpportunitiesInput input)
    {
        input.Stage = PipelineStage.Lost;
        input.IsClosed = true;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetOverdueOpportunitiesAsync(GetSalesOpportunitiesInput input)
    {
        input.IsOverdue = true;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<SalesOpportunityDto>> GetHighValueOpportunitiesAsync(decimal minValue, GetSalesOpportunitiesInput input)
    {
        input.MinEstimatedValue = minValue;
        return await GetListAsync(input);
    }

    public Task<ListResultDto<PipelineStage>> GetStageOptionsAsync()
    {
        var stages = Enum.GetValues(typeof(PipelineStage)).Cast<PipelineStage>().ToList();
        return Task.FromResult(new ListResultDto<PipelineStage>(stages));
    }
}
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ZenCrm.Sales;

public interface ISalesOpportunityAppService : ICrudAppService<
    SalesOpportunityDto,
    Guid,
    GetSalesOpportunitiesInput,
    CreateUpdateSalesOpportunityDto>
{
    Task<SalesOpportunityDto> MoveToStageAsync(Guid id, PipelineStage newStage);

    Task<SalesOpportunityDto> CloseWonAsync(Guid id, decimal? actualValue = null);

    Task<SalesOpportunityDto> CloseLostAsync(Guid id, string lostReason, string? competitor = null);

    Task<SalesOpportunityDto> SetPriorityAsync(Guid id, Priority priority);

    Task<SalesOpportunityDto> UpdateExpectedValueAsync(Guid id, decimal value);

    Task<SalesOpportunityDto> UpdateExpectedCloseDateAsync(Guid id, DateTime date);

    Task<SalesOpportunityDto> AssociateWithClientAsync(Guid id, Guid clientId);

    Task<SalesOpportunityDto> SetParentOpportunityAsync(Guid id, Guid? parentOpportunityId);

    Task<PagedResultDto<SalesOpportunityDto>> GetMyOpportunitiesAsync(GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetByStageAsync(PipelineStage stage, GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetByOwnerAsync(Guid ownerId, GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetWonOpportunitiesAsync(GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetLostOpportunitiesAsync(GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetOverdueOpportunitiesAsync(GetSalesOpportunitiesInput input);

    Task<PagedResultDto<SalesOpportunityDto>> GetHighValueOpportunitiesAsync(decimal minValue, GetSalesOpportunitiesInput input);

    Task<ListResultDto<PipelineStage>> GetStageOptionsAsync();
}
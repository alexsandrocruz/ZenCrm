using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ZenCrm.Sales;

public interface ISalesLeadAppService : ICrudAppService<
    SalesLeadDto,
    Guid,
    GetSalesLeadsInput,
    CreateUpdateSalesLeadDto>
{
    Task<SalesLeadDto> AssignToUserAsync(Guid id, Guid userId);

    Task<SalesLeadDto> UpdateStatusAsync(Guid id, LeadStatus status);

    Task<SalesLeadDto> ConvertToOpportunityAsync(Guid id);

    Task<SalesLeadDto> SetFollowUpDateAsync(Guid id, DateTime followUpDate);

    Task<SalesLeadDto> MarkAsDoNotContactAsync(Guid id, bool doNotContact = true);

    Task<SalesLeadDto> UpdateLastContactAsync(Guid id);

    Task<PagedResultDto<SalesLeadDto>> GetMyLeadsAsync(GetSalesLeadsInput input);

    Task<PagedResultDto<SalesLeadDto>> GetUnassignedLeadsAsync(GetSalesLeadsInput input);

    Task<PagedResultDto<SalesLeadDto>> GetLeadsNeedingFollowUpAsync(GetSalesLeadsInput input);

    Task<ListResultDto<LeadStatus>> GetStatusOptionsAsync();

    Task<ListResultDto<LeadSource> > GetSourceOptionsAsync();
}
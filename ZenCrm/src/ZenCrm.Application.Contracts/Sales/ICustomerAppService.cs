using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ZenCrm.Sales;

public interface ICustomerAppService : ICrudAppService<
    CustomerDto,
    Guid,
    GetCustomersInput,
    CreateUpdateCustomerDto>
{
    Task<CustomerDto> AssignToUserAsync(Guid id, Guid userId);

    Task<CustomerDto> SetStatusAsync(Guid id, bool isActive);

    Task<CustomerDto> SetAsPrimaryContactAsync(Guid id, bool isPrimary = true);

    Task<CustomerDto> SetAsKeyDecisionMakerAsync(Guid id, bool isKeyDecisionMaker = true);

    Task<CustomerDto> UpdateLastContactAsync(Guid id);

    Task<CustomerDto> AddNotesAsync(Guid id, string notes);

    Task<CustomerDto> AssociateWithClientAsync(Guid id, Guid clientId);

    Task<PagedResultDto<CustomerDto>> GetMyCustomersAsync(GetCustomersInput input);

    Task<PagedResultDto<CustomerDto>> GetUnassignedCustomersAsync(GetCustomersInput input);

    Task<PagedResultDto<CustomerDto>> GetByClientAsync(Guid clientId, GetCustomersInput input);

    Task<PagedResultDto<CustomerDto>> GetPrimaryContactsAsync(GetCustomersInput input);

    Task<PagedResultDto<CustomerDto>> GetKeyDecisionMakersAsync(GetCustomersInput input);

    Task<ListResultDto<CustomerDto>> GetByClientIdAsync(Guid clientId);
}
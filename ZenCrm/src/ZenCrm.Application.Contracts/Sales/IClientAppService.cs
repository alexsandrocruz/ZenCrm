using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ZenCrm.Sales;

public interface IClientAppService : ICrudAppService<
    ClientDto,
    Guid,
    GetClientsInput,
    CreateUpdateClientDto>
{
    Task<ClientDto> AssignToUserAsync(Guid id, Guid userId);

    Task<ClientDto> SetStatusAsync(Guid id, bool isActive);

    Task<ClientDto> UpdateLastInteractionAsync(Guid id);

    Task<PagedResultDto<ClientDto>> GetMyClientsAsync(GetClientsInput input);

    Task<PagedResultDto<ClientDto>> GetUnassignedClientsAsync(GetClientsInput input);

    Task<PagedResultDto<ClientDto>> GetActiveClientsAsync(GetClientsInput input);

    Task<PagedResultDto<ClientDto>> GetInactiveClientsAsync(GetClientsInput input);

    Task<ListResultDto<ClientType>> GetTypeOptionsAsync();

    Task<ListResultDto<ClientIndustry>> GetIndustryOptionsAsync();

    Task<PagedResultDto<ClientDto>> GetByCityAsync(string city, GetClientsInput input);

    Task<PagedResultDto<ClientDto>> GetByStateAsync(string state, GetClientsInput input);
}
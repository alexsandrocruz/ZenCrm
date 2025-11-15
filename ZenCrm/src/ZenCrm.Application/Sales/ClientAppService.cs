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

// [Authorize(ZenCrmPermissions.Clients.Default)] // Temporarily disabled for testing
public class ClientAppService : ApplicationService, IClientAppService
{
    private readonly IRepository<Client, Guid> _clientRepository;

    public ClientAppService(IRepository<Client, Guid> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<PagedResultDto<ClientDto>> GetListAsync(GetClientsInput input)
    {
        IQueryable<Client> queryable = await _clientRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x =>
                x.Name.Contains(input.Filter) ||
                x.DocumentNumber.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.Website.Contains(input.Filter))
            .WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value)
            .WhereIf(input.Industry.HasValue, x => x.Industry == input.Industry.Value)
            .WhereIf(input.AssignedUserId.HasValue, x => x.AssignedUserId == input.AssignedUserId.Value)
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
            .WhereIf(input.StartDate.HasValue, x => x.CreationTime >= input.StartDate.Value)
            .WhereIf(input.EndDate.HasValue, x => x.CreationTime <= input.EndDate.Value.AddDays(1).AddTicks(-1))
            .WhereIf(input.MinAnnualRevenue.HasValue, x => x.AnnualRevenue >= input.MinAnnualRevenue.Value)
            .WhereIf(input.MaxAnnualRevenue.HasValue, x => x.AnnualRevenue <= input.MaxAnnualRevenue.Value)
            .WhereIf(input.MinEmployees.HasValue, x => x.NumberOfEmployees >= input.MinEmployees.Value)
            .WhereIf(input.MaxEmployees.HasValue, x => x.NumberOfEmployees <= input.MaxEmployees.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.City), x => x.City == input.City)
            .WhereIf(!string.IsNullOrWhiteSpace(input.State), x => x.State == input.State)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Country), x => x.Country == input.Country);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy<Client, string>(x => x.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var clients = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<ClientDto>(
            totalCount,
            ObjectMapper.Map<List<Client>, List<ClientDto>>(clients)
        );
    }

    public async Task<ClientDto> GetAsync(Guid id)
    {
        var client = await _clientRepository.GetAsync(id);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    // [Authorize(ZenCrmPermissions.Clients.Create)] // Temporarily disabled for testing
    public async Task<ClientDto> CreateAsync(CreateUpdateClientDto input)
    {
        var client = ObjectMapper.Map<CreateUpdateClientDto, Client>(input);

        await _clientRepository.InsertAsync(client);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    [Authorize(ZenCrmPermissions.Clients.Edit)]
    public async Task<ClientDto> UpdateAsync(Guid id, CreateUpdateClientDto input)
    {
        var client = await _clientRepository.GetAsync(id);

        ObjectMapper.Map(input, client);

        await _clientRepository.UpdateAsync(client);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    [Authorize(ZenCrmPermissions.Clients.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _clientRepository.DeleteAsync(id);
    }

    [Authorize(ZenCrmPermissions.Clients.Assign)]
    public async Task<ClientDto> AssignToUserAsync(Guid id, Guid userId)
    {
        var client = await _clientRepository.GetAsync(id);

        client.AssignToUser(userId);
        await _clientRepository.UpdateAsync(client);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    [Authorize(ZenCrmPermissions.Clients.Edit)]
    public async Task<ClientDto> SetStatusAsync(Guid id, bool isActive)
    {
        var client = await _clientRepository.GetAsync(id);

        client.SetStatus(isActive);
        await _clientRepository.UpdateAsync(client);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    public async Task<ClientDto> UpdateLastInteractionAsync(Guid id)
    {
        var client = await _clientRepository.GetAsync(id);

        client.UpdateLastInteraction();
        await _clientRepository.UpdateAsync(client);

        return ObjectMapper.Map<Client, ClientDto>(client);
    }

    public async Task<PagedResultDto<ClientDto>> GetMyClientsAsync(GetClientsInput input)
    {
        var currentUserId = CurrentUser.Id ?? Guid.Empty;
        input.AssignedUserId = currentUserId;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<ClientDto>> GetUnassignedClientsAsync(GetClientsInput input)
    {
        input.AssignedUserId = null;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<ClientDto>> GetActiveClientsAsync(GetClientsInput input)
    {
        input.IsActive = true;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<ClientDto>> GetInactiveClientsAsync(GetClientsInput input)
    {
        input.IsActive = false;

        return await GetListAsync(input);
    }

    public Task<ListResultDto<ClientType>> GetTypeOptionsAsync()
    {
        var types = Enum.GetValues(typeof(ClientType)).Cast<ClientType>().ToList();
        return Task.FromResult(new ListResultDto<ClientType>(types));
    }

    public Task<ListResultDto<ClientIndustry>> GetIndustryOptionsAsync()
    {
        var industries = Enum.GetValues(typeof(ClientIndustry)).Cast<ClientIndustry>().ToList();
        return Task.FromResult(new ListResultDto<ClientIndustry>(industries));
    }

    public async Task<PagedResultDto<ClientDto>> GetByCityAsync(string city, GetClientsInput input)
    {
        input.City = city;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<ClientDto>> GetByStateAsync(string state, GetClientsInput input)
    {
        input.State = state;
        return await GetListAsync(input);
    }
}
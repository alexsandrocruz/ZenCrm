using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using ZenCrm.Permissions;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

[Authorize(ZenCrmPermissions.Customers.Default)]
public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly IRepository<Customer, Guid> _customerRepository;
    private readonly IdentityUserManager _userManager;

    public CustomerAppService(
        IRepository<Customer, Guid> customerRepository,
        IdentityUserManager userManager)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
    }

    public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomersInput input)
    {
        Logger.LogInformation($"GetListAsync called with filter: {input.Filter}");

        IQueryable<Customer> queryable = await _customerRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x =>
                x.FirstName.Contains(input.Filter) ||
                x.LastName.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.JobTitle.Contains(input.Filter))
            .WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId.Value)
            .WhereIf(input.AssignedUserId.HasValue, x => x.AssignedUserId == input.AssignedUserId.Value)
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
            .WhereIf(input.IsPrimaryContact.HasValue, x => x.IsPrimaryContact == input.IsPrimaryContact.Value)
            .WhereIf(input.IsKeyDecisionMaker.HasValue, x => x.IsKeyDecisionMaker == input.IsKeyDecisionMaker.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitle), x => x.JobTitle == input.JobTitle)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Department), x => x.Department == input.Department)
            .WhereIf(input.StartDate.HasValue, x => x.CreationTime >= input.StartDate.Value)
            .WhereIf(input.EndDate.HasValue, x => x.CreationTime <= input.EndDate.Value.AddDays(1).AddTicks(-1));

        var totalCount = await AsyncExecuter.CountAsync(queryable);
        Logger.LogInformation($"Total customers found: {totalCount}");

        queryable = queryable
            .OrderBy<Customer, string>(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var customers = await AsyncExecuter.ToListAsync(queryable);
        Logger.LogInformation($"Query results count: {customers.Count}");

        var customerDtos = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers);

        // Populate AssignedUserName for each customer
        foreach (var customerDto in customerDtos)
        {
            if (customerDto.AssignedUserId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(customerDto.AssignedUserId.Value.ToString());

                if (user != null)
                {
                    customerDto.AssignedUserName = !string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Surname)
                        ? $"{user.Name} {user.Surname}"
                        : !string.IsNullOrWhiteSpace(user.Name)
                            ? user.Name
                            : user.UserName;
                }

                Logger.LogInformation($"Customer: {customerDto.FirstName} {customerDto.LastName}, AssignedUserId: {customerDto.AssignedUserId}, AssignedUserName: {customerDto.AssignedUserName}");
            }
            else
            {
                Logger.LogInformation($"Customer: {customerDto.FirstName} {customerDto.LastName}, No AssignedUserId");
            }
        }

        return new PagedResultDto<CustomerDto>(totalCount, customerDtos);
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customerRepository = await _customerRepository.GetQueryableAsync();
        var userRepository = LazyServiceProvider.LazyGetRequiredService<IRepository<IdentityUser, Guid>>();

        var query = from customer in customerRepository
                   join user in await userRepository.GetQueryableAsync() on customer.AssignedUserId equals user.Id into userGroup
                   from user in userGroup.DefaultIfEmpty()
                   where customer.Id == id
                   select new { customer, user };

        var result = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (result == null)
        {
            throw new Volo.Abp.BusinessException("CustomerNotFound");
        }

        var customerDto = ObjectMapper.Map<Customer, CustomerDto>(result.customer);
        customerDto.AssignedUserName = result.user != null ?
            (!string.IsNullOrWhiteSpace(result.user.Name) && !string.IsNullOrWhiteSpace(result.user.Surname)
                ? $"{result.user.Name} {result.user.Surname}"
                : !string.IsNullOrWhiteSpace(result.user.Name)
                    ? result.user.Name
                    : result.user.UserName)
            : null;

        return customerDto;
    }

    [Authorize(ZenCrmPermissions.Customers.Create)]
    public async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
    {
        // Debug: Log para verificar o que está sendo recebido
        Logger.LogInformation($"Creating customer with data: FirstName={input.FirstName}, LastName={input.LastName}, AssignedUserId={input.AssignedUserId}");

        var customer = ObjectMapper.Map<CreateUpdateCustomerDto, Customer>(input);

        // Debug: Log para verificar o que foi mapeado
        Logger.LogInformation($"Mapped customer entity: Id={customer.Id}, FirstName={customer.FirstName}, LastName={customer.LastName}, AssignedUserId={customer.AssignedUserId}");

        await _customerRepository.InsertAsync(customer);

        var result = ObjectMapper.Map<Customer, CustomerDto>(customer);

        // Debug: Log para verificar o que foi retornado
        Logger.LogInformation($"Created customer returned: Id={result.Id}, FirstName={result.FirstName}, LastName={result.LastName}, AssignedUserId={result.AssignedUserId}");

        return result;
    }

    [Authorize(ZenCrmPermissions.Customers.Edit)]
    public async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
    {
        var customer = await _customerRepository.GetAsync(id);

        ObjectMapper.Map(input, customer);

        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    [Authorize(ZenCrmPermissions.Customers.Assign)]
    public async Task<CustomerDto> AssignToUserAsync(Guid id, Guid userId)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.AssignToUser(userId);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.Edit)]
    public async Task<CustomerDto> SetStatusAsync(Guid id, bool isActive)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.SetStatus(isActive);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.SetAsPrimary)]
    public async Task<CustomerDto> SetAsPrimaryContactAsync(Guid id, bool isPrimary = true)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.SetAsPrimaryContact(isPrimary);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.SetAsDecisionMaker)]
    public async Task<CustomerDto> SetAsKeyDecisionMakerAsync(Guid id, bool isKeyDecisionMaker = true)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.SetAsKeyDecisionMaker(isKeyDecisionMaker);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public async Task<CustomerDto> UpdateLastContactAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.UpdateLastContact();
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.Edit)]
    public async Task<CustomerDto> AddNotesAsync(Guid id, string notes)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.AddNotes(notes);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    [Authorize(ZenCrmPermissions.Customers.Edit)]
    public async Task<CustomerDto> AssociateWithClientAsync(Guid id, Guid clientId)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.SetClient(clientId);
        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public async Task<PagedResultDto<CustomerDto>> GetMyCustomersAsync(GetCustomersInput input)
    {
        var currentUserId = CurrentUser.Id ?? Guid.Empty;
        input.AssignedUserId = currentUserId;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<CustomerDto>> GetUnassignedCustomersAsync(GetCustomersInput input)
    {
        input.AssignedUserId = null;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<CustomerDto>> GetByClientAsync(Guid clientId, GetCustomersInput input)
    {
        input.ClientId = clientId;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<CustomerDto>> GetPrimaryContactsAsync(GetCustomersInput input)
    {
        input.IsPrimaryContact = true;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<CustomerDto>> GetKeyDecisionMakersAsync(GetCustomersInput input)
    {
        input.IsKeyDecisionMaker = true;
        return await GetListAsync(input);
    }

    public async Task<ListResultDto<CustomerDto>> GetByClientIdAsync(Guid clientId)
    {
        IQueryable<Customer> queryable = await _customerRepository.GetQueryableAsync();
        var customers = await AsyncExecuter.ToListAsync(
            queryable.Where(x => x.ClientId == clientId)
                     .OrderBy(x => x.LastName)
                     .ThenBy(x => x.FirstName));

        var customerDtos = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers);
        return new ListResultDto<CustomerDto>(customerDtos);
    }
}
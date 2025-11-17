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

[Authorize(ZenCrmPermissions.Interactions.Default)]
public class InteractionAppService : ApplicationService, IInteractionAppService
{
    private readonly IRepository<Interaction, Guid> _interactionRepository;

    public InteractionAppService(IRepository<Interaction, Guid> interactionRepository)
    {
        _interactionRepository = interactionRepository;
    }

    public async Task<PagedResultDto<InteractionDto>> GetListAsync(GetInteractionsInput input)
    {
        IQueryable<Interaction> queryable = await _interactionRepository.GetQueryableAsync();

        queryable = queryable
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x =>
                x.Subject.Contains(input.Filter) ||
                x.Description.Contains(input.Filter) ||
                x.Outcome.Contains(input.Filter))
            .WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value)
            .WhereIf(input.Priority.HasValue, x => x.Priority == input.Priority.Value)
            .WhereIf(input.OwnerUserId.HasValue, x => x.OwnerUserId == input.OwnerUserId.Value)
            .WhereIf(input.SalesLeadId.HasValue, x => x.SalesLeadId == input.SalesLeadId.Value)
            .WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId.Value)
            .WhereIf(input.CustomerId.HasValue, x => x.CustomerId == input.CustomerId.Value)
            .WhereIf(input.StartDate.HasValue, x => x.ScheduledDate >= input.StartDate.Value)
            .WhereIf(input.EndDate.HasValue, x => x.ScheduledDate <= input.EndDate.Value.AddDays(1).AddTicks(-1))
            .WhereIf(input.RequiresReminder.HasValue, x => x.RequiresReminder == input.RequiresReminder.Value)
            .WhereIf(input.IsAllDay.HasValue, x => x.IsAllDay == input.IsAllDay.Value)
            .WhereIf(!input.IncludeCompleted, x => x.Status != InteractionStatus.Completed)
            .WhereIf(!input.IncludeCancelled, x => x.Status != InteractionStatus.Cancelled);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderByDescending<Interaction, DateTime>(x => x.ScheduledDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var interactions = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<InteractionDto>(
            totalCount,
            ObjectMapper.Map<List<Interaction>, List<InteractionDto>>(interactions)
        );
    }

    public async Task<InteractionDto> GetAsync(Guid id)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Create)]
    public async Task<InteractionDto> CreateAsync(CreateUpdateInteractionDto input)
    {
        var interaction = ObjectMapper.Map<CreateUpdateInteractionDto, Interaction>(input);

        await _interactionRepository.InsertAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Edit)]
    public async Task<InteractionDto> UpdateAsync(Guid id, CreateUpdateInteractionDto input)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        ObjectMapper.Map(input, interaction);

        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _interactionRepository.DeleteAsync(id);
    }

    [Authorize(ZenCrmPermissions.Interactions.Complete)]
    public async Task<InteractionDto> StartAsync(Guid id)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        interaction.Start();
        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Complete)]
    public async Task<InteractionDto> CompleteAsync(Guid id, string? outcome = null)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        interaction.Complete(outcome);
        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Cancel)]
    public async Task<InteractionDto> CancelAsync(Guid id)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        interaction.Cancel();
        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Postpone)]
    public async Task<InteractionDto> PostponeAsync(Guid id, DateTime newScheduledDate)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        interaction.Postpone(newScheduledDate);
        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    [Authorize(ZenCrmPermissions.Interactions.Edit)]
    public async Task<InteractionDto> SetReminderAsync(Guid id, bool requiresReminder, DateTime? reminderDate = null)
    {
        var interaction = await _interactionRepository.GetAsync(id);

        interaction.SetReminder(requiresReminder, reminderDate);
        await _interactionRepository.UpdateAsync(interaction);

        return ObjectMapper.Map<Interaction, InteractionDto>(interaction);
    }

    public async Task<PagedResultDto<InteractionDto>> GetMyInteractionsAsync(GetInteractionsInput input)
    {
        var currentUserId = CurrentUser.Id ?? Guid.Empty;
        input.OwnerUserId = currentUserId;

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<InteractionDto>> GetByDateAsync(DateTime date, GetInteractionsInput input)
    {
        input.StartDate = date.Date;
        input.EndDate = date.Date.AddDays(1).AddTicks(-1);

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<InteractionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, GetInteractionsInput input)
    {
        input.StartDate = startDate.Date;
        input.EndDate = endDate.Date.AddDays(1).AddTicks(-1);

        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<InteractionDto>> GetOverdueInteractionsAsync(GetInteractionsInput input)
    {
        IQueryable<Interaction> queryable = await _interactionRepository.GetQueryableAsync();

        queryable = queryable
            .Where(x => x.Status != InteractionStatus.Completed &&
                       x.Status != InteractionStatus.Cancelled &&
                       x.ScheduledDate < DateTime.UtcNow)
            .WhereIf(input.OwnerUserId.HasValue, x => x.OwnerUserId == input.OwnerUserId.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy(x => x.ScheduledDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var interactions = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<InteractionDto>(
            totalCount,
            ObjectMapper.Map<List<Interaction>, List<InteractionDto>>(interactions)
        );
    }

    public async Task<PagedResultDto<InteractionDto>> GetUpcomingInteractionsAsync(GetInteractionsInput input)
    {
        IQueryable<Interaction> queryable = await _interactionRepository.GetQueryableAsync();

        queryable = queryable
            .Where(x => x.Status != InteractionStatus.Completed &&
                       x.Status != InteractionStatus.Cancelled &&
                       x.ScheduledDate >= DateTime.UtcNow &&
                       x.ScheduledDate <= DateTime.UtcNow.AddDays(7))
            .WhereIf(input.OwnerUserId.HasValue, x => x.OwnerUserId == input.OwnerUserId.Value);

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable
            .OrderBy(x => x.ScheduledDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var interactions = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<InteractionDto>(
            totalCount,
            ObjectMapper.Map<List<Interaction>, List<InteractionDto>>(interactions)
        );
    }

    public async Task<PagedResultDto<InteractionDto>> GetByLeadAsync(Guid leadId, GetInteractionsInput input)
    {
        input.SalesLeadId = leadId;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<InteractionDto>> GetByClientAsync(Guid clientId, GetInteractionsInput input)
    {
        input.ClientId = clientId;
        return await GetListAsync(input);
    }

    public async Task<PagedResultDto<InteractionDto>> GetByCustomerAsync(Guid customerId, GetInteractionsInput input)
    {
        input.CustomerId = customerId;
        return await GetListAsync(input);
    }

    public Task<ListResultDto<InteractionType>> GetTypeOptionsAsync()
    {
        var types = Enum.GetValues(typeof(InteractionType)).Cast<InteractionType>().ToList();
        return Task.FromResult(new ListResultDto<InteractionType>(types));
    }

    public Task<ListResultDto<InteractionStatus>> GetStatusOptionsAsync()
    {
        var statuses = Enum.GetValues(typeof(InteractionStatus)).Cast<InteractionStatus>().ToList();
        return Task.FromResult(new ListResultDto<InteractionStatus>(statuses));
    }
}
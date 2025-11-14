using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ZenCrm.Sales;

public interface IInteractionAppService : ICrudAppService<
    InteractionDto,
    Guid,
    GetInteractionsInput,
    CreateUpdateInteractionDto>
{
    Task<InteractionDto> StartAsync(Guid id);

    Task<InteractionDto> CompleteAsync(Guid id, string? outcome = null);

    Task<InteractionDto> CancelAsync(Guid id);

    Task<InteractionDto> PostponeAsync(Guid id, DateTime newScheduledDate);

    Task<InteractionDto> SetReminderAsync(Guid id, bool requiresReminder, DateTime? reminderDate = null);

    Task<PagedResultDto<InteractionDto>> GetMyInteractionsAsync(GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetByDateAsync(DateTime date, GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetOverdueInteractionsAsync(GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetUpcomingInteractionsAsync(GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetByLeadAsync(Guid leadId, GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetByClientAsync(Guid clientId, GetInteractionsInput input);

    Task<PagedResultDto<InteractionDto>> GetByCustomerAsync(Guid customerId, GetInteractionsInput input);

    Task<ListResultDto<InteractionType>> GetTypeOptionsAsync();

    Task<ListResultDto<InteractionStatus>> GetStatusOptionsAsync();
}
using System;
using Volo.Abp.Application.Dtos;

namespace ZenCrm.Sales;

public class GetInteractionsInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public InteractionType? Type { get; set; }

    public InteractionStatus? Status { get; set; }

    public Priority? Priority { get; set; }

    public Guid? OwnerUserId { get; set; }

    public Guid? SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? CustomerId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? RequiresReminder { get; set; }

    public bool? IsAllDay { get; set; }

    public bool IncludeCompleted { get; set; } = true;

    public bool IncludeCancelled { get; set; } = false;
}
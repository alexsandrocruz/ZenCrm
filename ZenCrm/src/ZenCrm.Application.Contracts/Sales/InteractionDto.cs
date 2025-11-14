using System;
using Volo.Abp.Application.Dtos;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class InteractionDto : AuditedEntityDto<Guid>
{
    public string Subject { get; set; } = string.Empty;

    public string? Description { get; set; }

    public InteractionType Type { get; set; }

    public InteractionStatus Status { get; set; }

    public Priority Priority { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int DurationMinutes { get; set; }

    public string? Location { get; set; }

    public string? Outcome { get; set; }

    public Guid? SalesLeadId { get; set; }

    public string? SalesLeadName { get; set; }

    public Guid? ClientId { get; set; }

    public string? ClientName { get; set; }

    public Guid? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public Guid OwnerUserId { get; set; }

    public string? OwnerUserName { get; set; }

    public bool IsAllDay { get; set; }

    public bool RequiresReminder { get; set; }

    public DateTime? ReminderDate { get; set; }

    public string? AdditionalData { get; set; }

    public string GetTargetEntity
    {
        get
        {
            if (CustomerId.HasValue)
                return $"Customer: {CustomerName}";
            if (ClientId.HasValue)
                return $"Client: {ClientName}";
            if (SalesLeadId.HasValue)
                return $"Lead: {SalesLeadName}";
            return "No target";
        }
    }

    public string GetTypeDisplay => Type.ToString();

    public string GetStatusDisplay => Status.ToString();

    public bool IsOverdue => Status != InteractionStatus.Completed &&
                            Status != InteractionStatus.Cancelled &&
                            ScheduledDate < DateTime.UtcNow;

    public string GetDurationDisplay
    {
        get
        {
            if (DurationMinutes == 0)
                return "Not set";

            if (DurationMinutes < 60)
                return $"{DurationMinutes} min";

            var hours = DurationMinutes / 60;
            var minutes = DurationMinutes % 60;

            if (minutes == 0)
                return $"{hours}h";

            return $"{hours}h {minutes}min";
        }
    }
}
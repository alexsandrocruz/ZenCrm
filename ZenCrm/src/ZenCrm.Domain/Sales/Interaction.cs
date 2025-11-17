using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class Interaction : AuditedAggregateRoot<Guid>
{
    [Required]
    [StringLength(256)]
    public string Subject { get; set; } = string.Empty;

    [CanBeNull]
    [StringLength(2000)]
    public string? Description { get; set; }

    public InteractionType Type { get; set; } = InteractionType.Note;

    public InteractionStatus Status { get; set; } = InteractionStatus.Pending;

    public Priority Priority { get; set; } = Priority.Normal;

    public DateTime ScheduledDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int DurationMinutes { get; set; } = 0;

    [CanBeNull]
    [StringLength(512)]
    public string? Location { get; set; }

    [CanBeNull]
    [StringLength(1024)]
    public string? Outcome { get; set; }

    public Guid? SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid OwnerUserId { get; set; }

    public bool IsAllDay { get; set; } = false;

    public bool RequiresReminder { get; set; } = false;

    public DateTime? ReminderDate { get; set; }

    [CanBeNull]
    [StringLength(2048)]
    public string? AdditionalData { get; set; }

    protected Interaction()
    {
    }

    public Interaction(
        Guid id,
        string subject,
        InteractionType type,
        Guid ownerUserId,
        DateTime scheduledDate
    ) : base(id)
    {
        SetSubject(subject);
        Type = type;
        OwnerUserId = ownerUserId;
        SetScheduledDate(scheduledDate);
    }

    public Interaction SetSubject(string subject)
    {
        Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject), maxLength: 256);
        return this;
    }

    public Interaction SetDescription(string? description)
    {
        Description = description?.Trim();
        return this;
    }

    public Interaction SetType(InteractionType type)
    {
        Type = type;
        return this;
    }

    public Interaction SetStatus(InteractionStatus status)
    {
        if (status == InteractionStatus.Completed && !EndDate.HasValue)
        {
            throw new BusinessException("Cannot mark interaction as completed without end date");
        }
        Status = status;
        return this;
    }

    public Interaction SetPriority(Priority priority)
    {
        Priority = priority;
        return this;
    }

    public Interaction SetScheduledDate(DateTime scheduledDate)
    {
        if (scheduledDate < DateTime.UtcNow.Date)
        {
            throw new BusinessException("Scheduled date cannot be in the past");
        }
        ScheduledDate = scheduledDate;
        return this;
    }

    public Interaction SetDates(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
        {
            throw new BusinessException("Start date cannot be after end date");
        }
        StartDate = startDate;
        EndDate = endDate;
        return this;
    }

    public Interaction SetDuration(int durationMinutes)
    {
        if (durationMinutes < 0)
        {
            throw new BusinessException("Duration cannot be negative");
        }
        DurationMinutes = durationMinutes;
        return this;
    }

    public Interaction SetLocation(string? location)
    {
        Location = location?.Trim();
        return this;
    }

    public Interaction SetOutcome(string? outcome)
    {
        Outcome = outcome?.Trim();
        return this;
    }

    public Interaction AssociateWithLead(Guid? leadId)
    {
        SalesLeadId = leadId;
        return this;
    }

    public Interaction AssociateWithClient(Guid? clientId)
    {
        ClientId = clientId;
        return this;
    }

    public Interaction AssociateWithCustomer(Guid? customerId)
    {
        CustomerId = customerId;
        return this;
    }

    public Interaction SetOwner(Guid ownerUserId)
    {
        OwnerUserId = ownerUserId;
        return this;
    }

    public Interaction SetAllDay(bool isAllDay)
    {
        IsAllDay = isAllDay;
        if (isAllDay)
        {
            DurationMinutes = 1440; // 24 hours
            StartDate = ScheduledDate.Date;
            EndDate = ScheduledDate.Date.AddDays(1).AddTicks(-1);
        }
        return this;
    }

    public Interaction SetReminder(bool requiresReminder, DateTime? reminderDate = null)
    {
        RequiresReminder = requiresReminder;
        if (requiresReminder && reminderDate.HasValue)
        {
            if (reminderDate.Value > ScheduledDate)
            {
                throw new BusinessException("Reminder date cannot be after scheduled date");
            }
            ReminderDate = reminderDate;
        }
        else if (!requiresReminder)
        {
            ReminderDate = null;
        }
        return this;
    }

    public Interaction Start()
    {
        if (Status == InteractionStatus.Completed)
        {
            throw new BusinessException("Cannot start a completed interaction");
        }
        Status = InteractionStatus.InProgress;
        StartDate = DateTime.UtcNow;
        return this;
    }

    public Interaction Complete(string? outcome = null)
    {
        if (Status == InteractionStatus.Completed)
        {
            throw new BusinessException("Interaction is already completed");
        }
        Status = InteractionStatus.Completed;
        EndDate = DateTime.UtcNow;
        Outcome = outcome?.Trim();

        // Calculate duration if not set
        if (StartDate.HasValue && DurationMinutes == 0)
        {
            DurationMinutes = (int)(EndDate.Value - StartDate.Value).TotalMinutes;
        }
        return this;
    }

    public Interaction Cancel()
    {
        if (Status == InteractionStatus.Completed)
        {
            throw new BusinessException("Cannot cancel a completed interaction");
        }
        Status = InteractionStatus.Cancelled;
        return this;
    }

    public Interaction Postpone(DateTime newScheduledDate)
    {
        if (newScheduledDate < DateTime.UtcNow.Date)
        {
            throw new BusinessException("New scheduled date cannot be in the past");
        }
        Status = InteractionStatus.Postponed;
        ScheduledDate = newScheduledDate;
        return this;
    }

    public string GetTargetEntity()
    {
        if (CustomerId.HasValue)
            return $"Customer ({CustomerId})";
        if (ClientId.HasValue)
            return $"Client ({ClientId})";
        if (SalesLeadId.HasValue)
            return $"Lead ({SalesLeadId})";
        return "No target";
    }

    public bool IsOverdue()
    {
        return Status != InteractionStatus.Completed &&
               Status != InteractionStatus.Cancelled &&
               ScheduledDate < DateTime.UtcNow;
    }
}
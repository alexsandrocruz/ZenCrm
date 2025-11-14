using System;
using System.ComponentModel.DataAnnotations;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class CreateUpdateInteractionDto
{
    [Required]
    [StringLength(256)]
    public string Subject { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public InteractionType Type { get; set; } = InteractionType.Note;

    public InteractionStatus Status { get; set; } = InteractionStatus.Pending;

    public Priority Priority { get; set; } = Priority.Normal;

    public DateTime ScheduledDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int DurationMinutes { get; set; } = 0;

    [StringLength(512)]
    public string? Location { get; set; }

    [StringLength(1024)]
    public string? Outcome { get; set; }

    public Guid? SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid OwnerUserId { get; set; }

    public bool IsAllDay { get; set; } = false;

    public bool RequiresReminder { get; set; } = false;

    public DateTime? ReminderDate { get; set; }

    [StringLength(2048)]
    public string? AdditionalData { get; set; }
}
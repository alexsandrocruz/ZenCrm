using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ZenCrm.Sales;

namespace ZenCrm.Sales;

public class SalesOpportunity : AuditedAggregateRoot<Guid>
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [CanBeNull]
    [StringLength(2000)]
    public string? Description { get; set; }

    public PipelineStage Stage { get; set; } = PipelineStage.Lead;

    public decimal EstimatedValue { get; set; } = 0;

    public decimal Probability { get; set; } = 10;

    public Priority Priority { get; set; } = Priority.Normal;

    public DateTime ExpectedCloseDate { get; set; }

    public Guid SalesLeadId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid OwnerUserId { get; set; }

    public DateTime StageChangeDate { get; set; }

    public PipelineStage? PreviousStage { get; set; }

    public DateTime? ActualCloseDate { get; set; }

    public decimal? ActualValue { get; set; }

    [CanBeNull]
    [StringLength(2000)]
    public string? LostReason { get; set; }

    [CanBeNull]
    [StringLength(512)]
    public string? Competitor { get; set; }

    public Guid? ParentOpportunityId { get; set; }

    public bool IsActive { get; set; } = true;

    protected SalesOpportunity()
    {
    }

    public SalesOpportunity(
        Guid id,
        string name,
        Guid salesLeadId,
        Guid ownerUserId,
        decimal estimatedValue,
        DateTime expectedCloseDate
    ) : base(id)
    {
        SetName(name);
        SalesLeadId = salesLeadId;
        OwnerUserId = ownerUserId;
        SetEstimatedValue(estimatedValue);
        SetExpectedCloseDate(expectedCloseDate);
        Stage = PipelineStage.Qualified;
        StageChangeDate = DateTime.UtcNow;
    }

    public SalesOpportunity SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 256);
        return this;
    }

    public SalesOpportunity SetDescription(string? description)
    {
        Description = description?.Trim();
        return this;
    }

    public SalesOpportunity SetEstimatedValue(decimal value)
    {
        if (value < 0)
        {
            throw new BusinessException("Estimated value cannot be negative");
        }
        EstimatedValue = value;
        UpdateProbability();
        return this;
    }

    public SalesOpportunity SetExpectedCloseDate(DateTime expectedCloseDate)
    {
        if (expectedCloseDate < DateTime.UtcNow.Date)
        {
            throw new BusinessException("Expected close date cannot be in the past");
        }
        ExpectedCloseDate = expectedCloseDate;
        return this;
    }

    public SalesOpportunity SetPriority(Priority priority)
    {
        Priority = priority;
        return this;
    }

    public SalesOpportunity AssociateWithClient(Guid? clientId)
    {
        ClientId = clientId;
        return this;
    }

    public SalesOpportunity AssignToUser(Guid ownerUserId)
    {
        OwnerUserId = ownerUserId;
        return this;
    }

    public SalesOpportunity MoveToStage(PipelineStage newStage)
    {
        if (Stage == PipelineStage.Won || Stage == PipelineStage.Lost)
        {
            throw new BusinessException("Cannot move opportunity that is already closed");
        }

        if (newStage == PipelineStage.Won)
        {
            return CloseWon();
        }

        if (newStage == PipelineStage.Lost)
        {
            throw new BusinessException("Use CloseLost method to close opportunity as lost");
        }

        // Validate stage progression
        if (!IsValidStageTransition(Stage, newStage))
        {
            throw new BusinessException($"Cannot move from {Stage} to {newStage}");
        }

        PreviousStage = Stage;
        Stage = newStage;
        StageChangeDate = DateTime.UtcNow;
        UpdateProbability();

        return this;
    }

    public SalesOpportunity CloseWon(decimal? actualValue = null)
    {
        if (Stage == PipelineStage.Won)
        {
            throw new BusinessException("Opportunity is already won");
        }

        Stage = PipelineStage.Won;
        StageChangeDate = DateTime.UtcNow;
        ActualCloseDate = DateTime.UtcNow;
        ActualValue = actualValue ?? EstimatedValue;
        Probability = 100;

        return this;
    }

    public SalesOpportunity CloseLost(string lostReason, string? competitor = null)
    {
        if (Stage == PipelineStage.Lost)
        {
            throw new BusinessException("Opportunity is already lost");
        }

        if (string.IsNullOrWhiteSpace(lostReason))
        {
            throw new BusinessException("Lost reason is required when closing opportunity as lost");
        }

        Stage = PipelineStage.Lost;
        StageChangeDate = DateTime.UtcNow;
        ActualCloseDate = DateTime.UtcNow;
        LostReason = lostReason.Trim();
        Competitor = competitor?.Trim();
        Probability = 0;

        return this;
    }

    public SalesOpportunity SetStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public SalesOpportunity SetCompetitor(string? competitor)
    {
        Competitor = competitor?.Trim();
        return this;
    }

    public SalesOpportunity SetParentOpportunity(Guid? parentOpportunityId)
    {
        if (parentOpportunityId.HasValue && parentOpportunityId.Value == Id)
        {
            throw new BusinessException("Opportunity cannot be its own parent");
        }
        ParentOpportunityId = parentOpportunityId;
        return this;
    }

    private void UpdateProbability()
    {
        Probability = Stage switch
        {
            PipelineStage.Lead => 10,
            PipelineStage.Qualifying => 25,
            PipelineStage.Qualified => 40,
            PipelineStage.Analysis => 50,
            PipelineStage.Proposal => 60,
            PipelineStage.ProposalSent => 70,
            PipelineStage.Negotiation => 80,
            PipelineStage.VerbalCommitment => 90,
            PipelineStage.Closing => 95,
            PipelineStage.Won => 100,
            PipelineStage.Lost => 0,
            PipelineStage.OnHold => Probability, // Keep current probability
            _ => Probability
        };
    }

    private bool IsValidStageTransition(PipelineStage currentStage, PipelineStage newStage)
    {
        // Allow moving forward in the pipeline
        if (newStage > currentStage && newStage != PipelineStage.Lost)
        {
            return true;
        }

        // Allow moving back to previous stages (but not past Lead)
        if (newStage < currentStage && newStage >= PipelineStage.Qualifying)
        {
            return true;
        }

        // Allow moving to OnHold from any active stage
        if (newStage == PipelineStage.OnHold)
        {
            return true;
        }

        // Allow returning from OnHold to the previous stage
        if (currentStage == PipelineStage.OnHold && PreviousStage.HasValue && newStage == PreviousStage.Value)
        {
            return true;
        }

        return false;
    }

    public bool IsClosed()
    {
        return Stage == PipelineStage.Won || Stage == PipelineStage.Lost;
    }

    public bool IsOverdue()
    {
        return !IsClosed() && ExpectedCloseDate < DateTime.UtcNow.Date;
    }

    public int DaysInCurrentStage()
    {
        return (DateTime.UtcNow - StageChangeDate).Days;
    }

    public decimal GetWeightedValue()
    {
        return EstimatedValue * (Probability / 100);
    }
}
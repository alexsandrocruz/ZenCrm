using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ZenCrm.Communication.Entities;

namespace ZenCrm.EntityFrameworkCore.Communication;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("CommunicationMessages");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.Channel)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.ToAddress)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(x => x.FromAddress)
            .HasMaxLength(512);

        builder.Property(x => x.CcAddress)
            .HasMaxLength(1000);

        builder.Property(x => x.BccAddress)
            .HasMaxLength(1000);

        builder.Property(x => x.ExternalMessageId)
            .HasMaxLength(256);

        builder.Property(x => x.ProviderResponse)
            .HasMaxLength(2000);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.TemplateVariables)
            .HasMaxLength(2000);

        builder.Property(x => x.RelatedEntityType)
            .HasMaxLength(64);

        // Indexes for performance
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Channel);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.ToAddress);
        builder.HasIndex(x => x.ScheduledSendDate);
        builder.HasIndex(x => x.CreationTime);
        builder.HasIndex(x => new { x.RelatedEntityType, x.RelatedEntityId });
        builder.HasIndex(x => x.InteractionId);
        builder.HasIndex(x => x.ExternalMessageId);
        builder.HasIndex(x => x.TemplateId);

        // Query filters
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
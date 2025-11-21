using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ZenCrm.Communication.Entities;

namespace ZenCrm.EntityFrameworkCore.Communication;

public class SmsDeliveryRecordConfiguration : IEntityTypeConfiguration<SmsDeliveryRecord>
{
    public void Configure(EntityTypeBuilder<SmsDeliveryRecord> builder)
    {
        builder.ToTable("CommunicationSmsDeliveryRecords");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.MessageId)
            .IsRequired();

        builder.Property(x => x.ExternalMessageId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ToPhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.FromPhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.PreviousStatus);

        builder.Property(x => x.Provider)
            .IsRequired();

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.MessageContent)
            .HasMaxLength(500);

        builder.Property(x => x.Segments)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(x => x.Cost)
            .HasPrecision(10, 4);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(x => x.SentAt);

        builder.Property(x => x.DeliveredAt);

        builder.Property(x => x.ReadAt);

        builder.Property(x => x.FailedAt);

        builder.Property(x => x.ErrorCode)
            .HasMaxLength(50);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.MaxRetryAttempts)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(x => x.NextRetryAt);

        builder.Property(x => x.TrackingActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.LastStatusCheckAt);

        builder.Property(x => x.DeliveryInfo)
            .HasMaxLength(500);

        builder.Property(x => x.RecipientCountry)
            .HasMaxLength(3);

        builder.Property(x => x.RecipientTimeZone)
            .HasMaxLength(50);

        builder.Property(x => x.CampaignId)
            .HasMaxLength(100);

        builder.Property(x => x.Tags)
            .HasMaxLength(256);

        // Indexes for performance
        builder.HasIndex(x => x.MessageId);
        builder.HasIndex(x => x.ExternalMessageId);
        builder.HasIndex(x => x.ToPhoneNumber);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Provider);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.SentAt);
        builder.HasIndex(x => x.DeliveredAt);
        builder.HasIndex(x => x.CreationTime);
        builder.HasIndex(x => x.TrackingActive);
        builder.HasIndex(x => x.NextRetryAt);
        builder.HasIndex(x => x.CampaignId);
        builder.HasIndex(x => x.RecipientCountry);

        // Composite indexes for common queries
        builder.HasIndex(x => new { x.Status, x.TrackingActive });
        builder.HasIndex(x => new { x.Provider, x.TrackingActive });
        builder.HasIndex(x => new { x.Status, x.CreationTime });
        builder.HasIndex(x => new { x.ToPhoneNumber, x.CreationTime });

        // Ensure unique external message ID per provider
        builder.HasIndex(x => new { x.ExternalMessageId, x.Provider }).IsUnique();

        // Configure ABP base properties
        builder.ConfigureByConvention();

        // Query filters
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
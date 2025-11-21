using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ZenCrm.Communication.Entities;

namespace ZenCrm.EntityFrameworkCore.Communication;

public class SmsTemplateConfiguration : IEntityTypeConfiguration<SmsTemplate>
{
    public void Configure(EntityTypeBuilder<SmsTemplate> builder)
    {
        builder.ToTable("CommunicationSmsTemplates");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.ContentTemplate)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.VariableDefinitions)
            .HasMaxLength(2000);

        builder.Property(x => x.Culture)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("en");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.Tags)
            .HasMaxLength(256);

        builder.Property(x => x.MaxCharactersPerSegment)
            .IsRequired()
            .HasDefaultValue(160);

        builder.Property(x => x.UseUnicode)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.AutoSplitLongMessages)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.SenderName)
            .HasMaxLength(11);

        builder.Property(x => x.IsMarketingTemplate)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.RequiredConsentType)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.DefaultPriority)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.UsageCount)
            .IsRequired()
            .HasDefaultValue(0);

        // Indexes for performance
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.Culture);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.UsageCount);
        builder.HasIndex(x => x.Tags);

        // Ensure unique template name
        builder.HasIndex(x => x.Name).IsUnique();

        // Configure ABP base properties
        builder.ConfigureByConvention();

        // Query filters
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
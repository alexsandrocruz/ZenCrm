using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ZenCrm.Communication.Entities;

namespace ZenCrm.EntityFrameworkCore.Communication;

public class MessageTemplateConfiguration : IEntityTypeConfiguration<MessageTemplate>
{
    public void Configure(EntityTypeBuilder<MessageTemplate> builder)
    {
        builder.ToTable("CommunicationMessageTemplates");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Channel)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.SubjectTemplate)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ContentTemplate)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.VariableDefinitions)
            .HasMaxLength(2000);

        builder.Property(x => x.Category)
            .HasMaxLength(64);

        builder.Property(x => x.Tags)
            .HasMaxLength(256);

        builder.Property(x => x.Culture)
            .HasMaxLength(10);

        // Indexes for performance
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Channel);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.Culture);

        // Ensure unique template name per channel
        builder.HasIndex(x => new { x.Name, x.Channel }).IsUnique();

        // Query filters
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ZenCrm.Communication.Entities;

namespace ZenCrm.EntityFrameworkCore.Communication;

public static class CommunicationModelBuilderExtensions
{
    public static void ConfigureCommunication(this ModelBuilder builder)
    {
        builder.Entity<Message>(b =>
        {
            b.ToTable("CommunicationMessages");
            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(4000);

            b.Property(x => x.ToAddress)
                .IsRequired()
                .HasMaxLength(512);

            b.Property(x => x.FromAddress)
                .HasMaxLength(512);

            b.Property(x => x.CcAddress)
                .HasMaxLength(1000);

            b.Property(x => x.BccAddress)
                .HasMaxLength(1000);

            b.Property(x => x.ExternalMessageId)
                .HasMaxLength(256);

            b.Property(x => x.ProviderResponse)
                .HasMaxLength(2000);

            b.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            b.Property(x => x.TemplateVariables)
                .HasMaxLength(2000);

            b.Property(x => x.RelatedEntityType)
                .HasMaxLength(64);

            // Indexes
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Channel);
            b.HasIndex(x => x.Type);
            b.HasIndex(x => x.ToAddress);
            b.HasIndex(x => x.ScheduledSendDate);
            b.HasIndex(x => x.CreationTime);
            b.HasIndex(x => new { x.RelatedEntityType, x.RelatedEntityId });
            b.HasIndex(x => x.InteractionId);
            b.HasIndex(x => x.ExternalMessageId);
            b.HasIndex(x => x.TemplateId);
        });

        builder.Entity<MessageTemplate>(b =>
        {
            b.ToTable("CommunicationMessageTemplates");
            b.ConfigureByConvention();

            // Properties
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            b.Property(x => x.Description)
                .HasMaxLength(500);

            b.Property(x => x.SubjectTemplate)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.ContentTemplate)
                .IsRequired()
                .HasMaxLength(4000);

            b.Property(x => x.VariableDefinitions)
                .HasMaxLength(2000);

            b.Property(x => x.Category)
                .HasMaxLength(64);

            b.Property(x => x.Tags)
                .HasMaxLength(256);

            b.Property(x => x.Culture)
                .HasMaxLength(10);

            // Indexes
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.Channel);
            b.HasIndex(x => x.Type);
            b.HasIndex(x => x.Category);
            b.HasIndex(x => x.IsActive);
            b.HasIndex(x => x.Culture);

            // Ensure unique template name per channel
            b.HasIndex(x => new { x.Name, x.Channel }).IsUnique();
        });
    }
}
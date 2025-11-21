using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenCrm.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunicationModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CommunicationSent",
                table: "AppInteractions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CommunicationSentDate",
                table: "AppInteractions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "AppInteractions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunicationMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    Channel = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledSendDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReadDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ToAddress = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    FromAddress = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CcAddress = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    BccAddress = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ExternalMessageId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ProviderResponse = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TemplateId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TemplateVariables = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    RelatedEntityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    InteractionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationMessageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Channel = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectTemplate = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ContentTemplate = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    VariableDefinitions = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Culture = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationMessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_CommunicationSent",
                table: "AppInteractions",
                column: "CommunicationSent");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_CommunicationSentDate",
                table: "AppInteractions",
                column: "CommunicationSentDate");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_MessageId",
                table: "AppInteractions",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_Channel",
                table: "CommunicationMessages",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_CreationTime",
                table: "CommunicationMessages",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_ExternalMessageId",
                table: "CommunicationMessages",
                column: "ExternalMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_InteractionId",
                table: "CommunicationMessages",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_RelatedEntityType_RelatedEntityId",
                table: "CommunicationMessages",
                columns: new[] { "RelatedEntityType", "RelatedEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_ScheduledSendDate",
                table: "CommunicationMessages",
                column: "ScheduledSendDate");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_Status",
                table: "CommunicationMessages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_TemplateId",
                table: "CommunicationMessages",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_ToAddress",
                table: "CommunicationMessages",
                column: "ToAddress");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessages_Type",
                table: "CommunicationMessages",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Category",
                table: "CommunicationMessageTemplates",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Channel",
                table: "CommunicationMessageTemplates",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Culture",
                table: "CommunicationMessageTemplates",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_IsActive",
                table: "CommunicationMessageTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Name",
                table: "CommunicationMessageTemplates",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Name_Channel",
                table: "CommunicationMessageTemplates",
                columns: new[] { "Name", "Channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationMessageTemplates_Type",
                table: "CommunicationMessageTemplates",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationMessages");

            migrationBuilder.DropTable(
                name: "CommunicationMessageTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AppInteractions_CommunicationSent",
                table: "AppInteractions");

            migrationBuilder.DropIndex(
                name: "IX_AppInteractions_CommunicationSentDate",
                table: "AppInteractions");

            migrationBuilder.DropIndex(
                name: "IX_AppInteractions_MessageId",
                table: "AppInteractions");

            migrationBuilder.DropColumn(
                name: "CommunicationSent",
                table: "AppInteractions");

            migrationBuilder.DropColumn(
                name: "CommunicationSentDate",
                table: "AppInteractions");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "AppInteractions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenCrm.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmSalesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    DocumentNumber = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Industry = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    State = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastInteractionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    MobilePhone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPrimaryContact = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsKeyDecisionMaker = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastContactDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AssignedUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppInteractions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Outcome = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true),
                    SalesLeadId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OwnerUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsAllDay = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AdditionalData = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInteractions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSalesLeads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    MobilePhone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Company = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExpectedCloseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastContactDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NextFollowUpDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DoNotContact = table.Column<bool>(type: "INTEGER", nullable: false),
                    Converted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConvertedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OpportunityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSalesLeads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSalesOpportunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Stage = table.Column<int>(type: "INTEGER", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Probability = table.Column<decimal>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpectedCloseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SalesLeadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OwnerUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StageChangeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PreviousStage = table.Column<int>(type: "INTEGER", nullable: true),
                    ActualCloseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    LostReason = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Competitor = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ParentOpportunityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSalesOpportunities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppClients_AssignedUserId",
                table: "AppClients",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClients_DocumentNumber",
                table: "AppClients",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AppClients_Industry",
                table: "AppClients",
                column: "Industry");

            migrationBuilder.CreateIndex(
                name: "IX_AppClients_Name",
                table: "AppClients",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AppClients_Type",
                table: "AppClients",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_AssignedUserId",
                table: "AppCustomers",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_ClientId",
                table: "AppCustomers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_Email",
                table: "AppCustomers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_FirstName",
                table: "AppCustomers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_IsKeyDecisionMaker",
                table: "AppCustomers",
                column: "IsKeyDecisionMaker");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_IsPrimaryContact",
                table: "AppCustomers",
                column: "IsPrimaryContact");

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_LastName",
                table: "AppCustomers",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_ClientId",
                table: "AppInteractions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_CustomerId",
                table: "AppInteractions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_OwnerUserId",
                table: "AppInteractions",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_SalesLeadId",
                table: "AppInteractions",
                column: "SalesLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_ScheduledDate",
                table: "AppInteractions",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_Status",
                table: "AppInteractions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AppInteractions_Type",
                table: "AppInteractions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesLeads_AssignedUserId",
                table: "AppSalesLeads",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesLeads_ClientId",
                table: "AppSalesLeads",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesLeads_Converted",
                table: "AppSalesLeads",
                column: "Converted");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesLeads_Email",
                table: "AppSalesLeads",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesLeads_Status",
                table: "AppSalesLeads",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_ClientId",
                table: "AppSalesOpportunities",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_EstimatedValue",
                table: "AppSalesOpportunities",
                column: "EstimatedValue");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_ExpectedCloseDate",
                table: "AppSalesOpportunities",
                column: "ExpectedCloseDate");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_OwnerUserId",
                table: "AppSalesOpportunities",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_ParentOpportunityId",
                table: "AppSalesOpportunities",
                column: "ParentOpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_SalesLeadId",
                table: "AppSalesOpportunities",
                column: "SalesLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOpportunities_Stage",
                table: "AppSalesOpportunities",
                column: "Stage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClients");

            migrationBuilder.DropTable(
                name: "AppCustomers");

            migrationBuilder.DropTable(
                name: "AppInteractions");

            migrationBuilder.DropTable(
                name: "AppSalesLeads");

            migrationBuilder.DropTable(
                name: "AppSalesOpportunities");
        }
    }
}

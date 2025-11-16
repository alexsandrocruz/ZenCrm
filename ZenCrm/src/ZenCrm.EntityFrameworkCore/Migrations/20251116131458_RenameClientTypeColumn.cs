using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenCrm.Migrations
{
    /// <inheritdoc />
    public partial class RenameClientTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AppClients",
                newName: "ClientType");

            migrationBuilder.RenameIndex(
                name: "IX_AppClients_Type",
                table: "AppClients",
                newName: "IX_AppClients_ClientType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientType",
                table: "AppClients",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_AppClients_ClientType",
                table: "AppClients",
                newName: "IX_AppClients_Type");
        }
    }
}

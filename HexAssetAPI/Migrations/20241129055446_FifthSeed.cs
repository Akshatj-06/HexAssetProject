using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class FifthSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditRequests_Users_AdminUserId",
                table: "AuditRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuditRequests_AdminUserId",
                table: "AuditRequests");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "AuditRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "AuditRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditRequests_AdminUserId",
                table: "AuditRequests",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditRequests_Users_AdminUserId",
                table: "AuditRequests",
                column: "AdminUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}

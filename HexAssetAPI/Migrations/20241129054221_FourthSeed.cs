using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class FourthSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditRequests_Users_AdminId",
                table: "AuditRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuditRequests_AdminId",
                table: "AuditRequests");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "AuditRequests");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "AuditRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AuditRequests_AdminId",
                table: "AuditRequests",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditRequests_Users_AdminId",
                table: "AuditRequests",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

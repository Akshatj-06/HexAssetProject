using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class TenthSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllocationId",
                table: "AuditRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllocationId",
                table: "AuditRequests");
        }
    }
}

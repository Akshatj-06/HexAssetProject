using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class EighthSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueType",
                table: "ServiceRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueType",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

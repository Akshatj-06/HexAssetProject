using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class ThirdSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueType",
                table: "AssetRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueType",
                table: "AssetRequests");
        }
    }
}

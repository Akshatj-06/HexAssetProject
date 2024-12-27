using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HexAsset.Migrations
{
    /// <inheritdoc />
    public partial class SecondSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "UserType");
        }
    }
}

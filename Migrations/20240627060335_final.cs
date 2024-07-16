using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bshbbackend.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "chiefministerName",
                table: "homepage1s",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "departmentministerName",
                table: "homepage1s",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chiefministerName",
                table: "homepage1s");

            migrationBuilder.DropColumn(
                name: "departmentministerName",
                table: "homepage1s");
        }
    }
}

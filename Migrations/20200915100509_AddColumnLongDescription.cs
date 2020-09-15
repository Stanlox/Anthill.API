using Microsoft.EntityFrameworkCore.Migrations;

namespace Anthill.API.Migrations
{
    public partial class AddColumnLongDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

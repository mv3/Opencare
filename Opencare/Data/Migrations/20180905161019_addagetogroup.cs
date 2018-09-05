using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Data.Migrations
{
    public partial class addagetogroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxAge",
                table: "Group",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinAge",
                table: "Group",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAge",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "MinAge",
                table: "Group");
        }
    }
}

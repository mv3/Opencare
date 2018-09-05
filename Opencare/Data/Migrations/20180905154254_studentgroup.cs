using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Data.Migrations
{
    public partial class studentgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Student");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Student",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Student",
                nullable: true);
        }
    }
}

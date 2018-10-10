using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Migrations
{
    public partial class updatedocumenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "StudentDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "StudentDocuments");
        }
    }
}

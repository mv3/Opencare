using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Data.Migrations
{
    public partial class addDocumentTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "StudentDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Document",
                table: "StudentDocuments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Data.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ParentID",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_ParentID",
                table: "Student",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_ParentID",
                table: "Student",
                column: "ParentID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_ParentID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ParentID",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "ParentID",
                table: "Student",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

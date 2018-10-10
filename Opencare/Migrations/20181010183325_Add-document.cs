using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Migrations
{
    public partial class Adddocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadUserId",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_StudentId",
                table: "StudentDocuments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_UploadUserId",
                table: "StudentDocuments",
                column: "UploadUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocuments_Student_StudentId",
                table: "StudentDocuments",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocuments_AspNetUsers_UploadUserId",
                table: "StudentDocuments",
                column: "UploadUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocuments_Student_StudentId",
                table: "StudentDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocuments_AspNetUsers_UploadUserId",
                table: "StudentDocuments");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocuments_StudentId",
                table: "StudentDocuments");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocuments_UploadUserId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "UploadUserId",
                table: "StudentDocuments");
        }
    }
}

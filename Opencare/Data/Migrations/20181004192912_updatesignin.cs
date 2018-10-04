using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Data.Migrations
{
    public partial class updatesignin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignIn_Student_StudentId",
                table: "SignIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignIn",
                table: "SignIn");

            migrationBuilder.RenameTable(
                name: "SignIn",
                newName: "SignIns");

            migrationBuilder.RenameIndex(
                name: "IX_SignIn_StudentId",
                table: "SignIns",
                newName: "IX_SignIns_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignIns",
                table: "SignIns",
                column: "SignInId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignIns_Student_StudentId",
                table: "SignIns",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignIns_Student_StudentId",
                table: "SignIns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignIns",
                table: "SignIns");

            migrationBuilder.RenameTable(
                name: "SignIns",
                newName: "SignIn");

            migrationBuilder.RenameIndex(
                name: "IX_SignIns_StudentId",
                table: "SignIn",
                newName: "IX_SignIn_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignIn",
                table: "SignIn",
                column: "SignInId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignIn_Student_StudentId",
                table: "SignIn",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

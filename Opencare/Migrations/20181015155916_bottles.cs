using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Migrations
{
    public partial class bottles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Diapers",
                newName: "Note");

            migrationBuilder.CreateTable(
                name: "Bottles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    Ounces = table.Column<double>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    TeacherId = table.Column<string>(nullable: true),
                    StudentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bottles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bottles_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bottles_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bottles_StudentId",
                table: "Bottles",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bottles_TeacherId",
                table: "Bottles",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bottles");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Diapers",
                newName: "Notes");
        }
    }
}

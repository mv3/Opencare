using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opencare.Migrations
{
    public partial class updateDocumenttypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "StudentDocuments");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "StudentDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginCareDate",
                table: "Student",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndCareDate",
                table: "Student",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "FieldTripAuthorized",
                table: "Student",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhysicianName",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicianPhone",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_DocumentTypeId",
                table: "StudentDocuments",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocuments_DocumentType_DocumentTypeId",
                table: "StudentDocuments",
                column: "DocumentTypeId",
                principalTable: "DocumentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocuments_DocumentType_DocumentTypeId",
                table: "StudentDocuments");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocuments_DocumentTypeId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "BeginCareDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "EndCareDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "FieldTripAuthorized",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PhysicianName",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PhysicianPhone",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "StudentDocuments",
                nullable: false,
                defaultValue: "");
        }
    }
}

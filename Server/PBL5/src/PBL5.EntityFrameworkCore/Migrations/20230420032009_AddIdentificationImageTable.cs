using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentificationImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentificationImages_Employees_EmployeeId",
                table: "IdentificationImages");

            migrationBuilder.DropIndex(
                name: "IX_IdentificationImages_EmployeeId",
                table: "IdentificationImages");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "IdentificationImages",
                newName: "TimeSheetId");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificationImageId",
                table: "TimeSheets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentificationImages_TimeSheets_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId",
                principalTable: "TimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentificationImages_TimeSheets_TimeSheetId",
                table: "IdentificationImages");

            migrationBuilder.DropIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages");

            migrationBuilder.DropColumn(
                name: "IdentificationImageId",
                table: "TimeSheets");

            migrationBuilder.RenameColumn(
                name: "TimeSheetId",
                table: "IdentificationImages",
                newName: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentificationImages_EmployeeId",
                table: "IdentificationImages",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentificationImages_Employees_EmployeeId",
                table: "IdentificationImages",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

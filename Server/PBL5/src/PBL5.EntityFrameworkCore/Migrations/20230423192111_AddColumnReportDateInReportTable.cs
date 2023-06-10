using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnReportDateInReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_TimeSheets_TimeSheetId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "TimeSheetId",
                table: "Reports",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_TimeSheetId",
                table: "Reports",
                newName: "IX_Reports_EmployeeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Employees_EmployeeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Reports",
                newName: "TimeSheetId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_EmployeeId",
                table: "Reports",
                newName: "IX_Reports_TimeSheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_TimeSheets_TimeSheetId",
                table: "Reports",
                column: "TimeSheetId",
                principalTable: "TimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

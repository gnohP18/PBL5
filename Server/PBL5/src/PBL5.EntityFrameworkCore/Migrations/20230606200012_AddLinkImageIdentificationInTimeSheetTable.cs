using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkImageIdentificationInTimeSheetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificationImageId",
                table: "TimeSheets");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationImageCheckIn",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationImageCheckOut",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificationImageCheckIn",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "IdentificationImageCheckOut",
                table: "TimeSheets");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificationImageId",
                table: "TimeSheets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

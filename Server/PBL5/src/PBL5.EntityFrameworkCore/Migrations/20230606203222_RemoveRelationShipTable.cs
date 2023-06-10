using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationShipTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentificationImages_TimeSheets_TimeSheetId",
                table: "IdentificationImages");

            migrationBuilder.DropIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentificationImages_TimeSheets_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId",
                principalTable: "TimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

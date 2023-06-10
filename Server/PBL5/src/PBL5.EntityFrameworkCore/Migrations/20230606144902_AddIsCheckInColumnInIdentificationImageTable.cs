using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCheckInColumnInIdentificationImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckIn",
                table: "IdentificationImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages");

            migrationBuilder.DropColumn(
                name: "IsCheckIn",
                table: "IdentificationImages");

            migrationBuilder.CreateIndex(
                name: "IX_IdentificationImages_TimeSheetId",
                table: "IdentificationImages",
                column: "TimeSheetId",
                unique: true);
        }
    }
}

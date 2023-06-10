using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL5.Migrations
{
    /// <inheritdoc />
    public partial class ReRemoveCommuneDistrictProvinceEthnicNationalityColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commune",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Ethnic",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Commune",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ethnic",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

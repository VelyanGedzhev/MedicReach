using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicReach.Data.Migrations
{
    public partial class AddPracticePermissionAndIsApprovedInPhysicianTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Physicians",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PracticePermissionNumber",
                table: "Physicians",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Physicians");

            migrationBuilder.DropColumn(
                name: "PracticePermissionNumber",
                table: "Physicians");
        }
    }
}

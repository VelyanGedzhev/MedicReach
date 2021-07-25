using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicReach.Data.Migrations
{
    public partial class PhysiciansTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Physicians");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Physicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Physicians_UserId",
                table: "Physicians",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Physicians_AspNetUsers_UserId",
                table: "Physicians",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Physicians_AspNetUsers_UserId",
                table: "Physicians");

            migrationBuilder.DropIndex(
                name: "IX_Physicians_UserId",
                table: "Physicians");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Physicians");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Physicians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

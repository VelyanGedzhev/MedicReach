using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicReach.Data.Migrations
{
    public partial class UpdateMedicalCenterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalCenters_MedicalCenterTypes_MedicalCenterTypeId",
                table: "MedicalCenters");

            migrationBuilder.RenameColumn(
                name: "MedicalCenterTypeId",
                table: "MedicalCenters",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalCenters_MedicalCenterTypeId",
                table: "MedicalCenters",
                newName: "IX_MedicalCenters_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalCenters_MedicalCenterTypes_TypeId",
                table: "MedicalCenters",
                column: "TypeId",
                principalTable: "MedicalCenterTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalCenters_MedicalCenterTypes_TypeId",
                table: "MedicalCenters");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "MedicalCenters",
                newName: "MedicalCenterTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalCenters_TypeId",
                table: "MedicalCenters",
                newName: "IX_MedicalCenters_MedicalCenterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalCenters_MedicalCenterTypes_MedicalCenterTypeId",
                table: "MedicalCenters",
                column: "MedicalCenterTypeId",
                principalTable: "MedicalCenterTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

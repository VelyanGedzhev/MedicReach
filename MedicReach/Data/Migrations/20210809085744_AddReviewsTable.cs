using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicReach.Data.Migrations
{
    public partial class AddReviewsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Patients_PatientId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Physicians_PhysicianId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_PhysicianId",
                table: "Reviews",
                newName: "IX_Reviews_PhysicianId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_PatientId",
                table: "Reviews",
                newName: "IX_Reviews_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Patients_PatientId",
                table: "Reviews",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Physicians_PhysicianId",
                table: "Reviews",
                column: "PhysicianId",
                principalTable: "Physicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Patients_PatientId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Physicians_PhysicianId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_PhysicianId",
                table: "Review",
                newName: "IX_Review_PhysicianId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_PatientId",
                table: "Review",
                newName: "IX_Review_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Patients_PatientId",
                table: "Review",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Physicians_PhysicianId",
                table: "Review",
                column: "PhysicianId",
                principalTable: "Physicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

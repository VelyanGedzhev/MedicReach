using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicReach.Data.Migrations
{
    public partial class AddIsReviewInAppointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Patients_PatientId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Physicians_PhysicianId",
                table: "Reviews");

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Patients_PatientId",
                table: "Reviews",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Physicians_PhysicianId",
                table: "Reviews",
                column: "PhysicianId",
                principalTable: "Physicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Patients_PatientId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Physicians_PhysicianId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}

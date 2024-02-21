using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Services.ClinicAPI.Migrations
{
    public partial class appid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_appointmentId",
                table: "Prescriptions",
                column: "appointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_PatientAppointments_appointmentId",
                table: "Prescriptions",
                column: "appointmentId",
                principalTable: "PatientAppointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_PatientAppointments_appointmentId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_appointmentId",
                table: "Prescriptions");
        }
    }
}

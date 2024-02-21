using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Services.ClinicAPI.Migrations
{
    public partial class pd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointments_Did",
                table: "PatientAppointments",
                column: "Did");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointments_Pid",
                table: "PatientAppointments",
                column: "Pid");

           

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointments_Doctors_Did",
                table: "PatientAppointments",
                column: "Did",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointments_Patients_Pid",
                table: "PatientAppointments",
                column: "Pid",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointments_Doctors_Did",
                table: "PatientAppointments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointments_Patients_Pid",
                table: "PatientAppointments");

         
            migrationBuilder.DropIndex(
                name: "IX_PatientAppointments_Did",
                table: "PatientAppointments");

            migrationBuilder.DropIndex(
                name: "IX_PatientAppointments_Pid",
                table: "PatientAppointments");
        }
    }
}

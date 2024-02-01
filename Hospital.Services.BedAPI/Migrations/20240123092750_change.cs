using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Services.BedAPI.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
               

            migrationBuilder.CreateTable(
                name: "BedAllotments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    BedCatId = table.Column<int>(type: "int", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    AllotmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Released = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedAllotments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedAllotments_BedCategories_BedCatId",
                        column: x => x.BedCatId,
                        principalTable: "BedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BedAllotments_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BedAllotments_Patient_Pid",
                        column: x => x.Pid,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BedAllotments_BedCatId",
                table: "BedAllotments",
                column: "BedCatId");

            migrationBuilder.CreateIndex(
                name: "IX_BedAllotments_BedId",
                table: "BedAllotments",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_BedAllotments_Pid",
                table: "BedAllotments",
                column: "Pid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BedAllotments");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}

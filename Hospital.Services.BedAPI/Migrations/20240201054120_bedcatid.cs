using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Services.BedAPI.Migrations
{
    public partial class bedcatid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BedAllotments_BedCategories_BedCatId",
                table: "BedAllotments");

            migrationBuilder.DropIndex(
                name: "IX_BedAllotments_BedCatId",
                table: "BedAllotments");


            migrationBuilder.DropColumn(
                name: "BedCatId",
                table: "BedAllotments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "BedCatId",
                table: "BedAllotments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BedAllotments_BedCatId",
                table: "BedAllotments",
                column: "BedCatId");

            migrationBuilder.AddForeignKey(
                name: "FK_BedAllotments_BedCategories_BedCatId",
                table: "BedAllotments",
                column: "BedCatId",
                principalTable: "BedCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

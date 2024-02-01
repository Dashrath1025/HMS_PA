using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Services.BedAPI.Migrations
{
    public partial class bed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BedCatId = table.Column<int>(type: "int", nullable: false),
                    No = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beds_BedCategories_BedCatId",
                        column: x => x.BedCatId,
                        principalTable: "BedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beds_BedCatId",
                table: "Beds",
                column: "BedCatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beds");
        }
    }
}

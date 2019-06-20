using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Data.Migrations
{
    public partial class OneToOneCompanyAndWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "WareHouses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WareHouses");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies",
                column: "WareHouseId");
        }
    }
}

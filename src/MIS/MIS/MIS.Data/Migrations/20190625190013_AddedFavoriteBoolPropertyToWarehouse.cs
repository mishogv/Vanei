using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Data.Migrations
{
    public partial class AddedFavoriteBoolPropertyToWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "WareHouses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "WareHouses");
        }
    }
}

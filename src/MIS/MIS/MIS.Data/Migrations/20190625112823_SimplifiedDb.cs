using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Data.Migrations
{
    public partial class SimplifiedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_WareHouses_WareHouseId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "SystemProducts");

            migrationBuilder.DropIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WareHouses_CompanyId",
                table: "WareHouses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CompanyId",
                table: "Receipts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Companies_CompanyId",
                table: "Receipts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouses_Companies_CompanyId",
                table: "WareHouses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Companies_CompanyId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_WareHouses_Companies_CompanyId",
                table: "WareHouses");

            migrationBuilder.DropIndex(
                name: "IX_WareHouses_CompanyId",
                table: "WareHouses");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_CompanyId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Receipts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Companies",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    From = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_WareHouseId",
                table: "Companies",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId1",
                table: "AspNetUsers",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CompanyId",
                table: "Invitations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemProducts_UserId",
                table: "SystemProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId1",
                table: "AspNetUsers",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_WareHouses_WareHouseId",
                table: "Companies",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Data.Migrations
{
    public partial class AddedReceiptProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Receipts_ReceiptId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Companies_CompanyId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_CompanyId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Products_ReceiptId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Receipts",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Receipts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceiptProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReceiptId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptProducts_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CompanyId1",
                table: "Receipts",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptProducts_ProductId",
                table: "ReceiptProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptProducts_ReceiptId",
                table: "ReceiptProducts",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Companies_CompanyId1",
                table: "Receipts",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Companies_CompanyId1",
                table: "Receipts");

            migrationBuilder.DropTable(
                name: "ReceiptProducts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_CompanyId1",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Receipts");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Receipts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CompanyId",
                table: "Receipts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReceiptId",
                table: "Products",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Receipts_ReceiptId",
                table: "Products",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Companies_CompanyId",
                table: "Receipts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

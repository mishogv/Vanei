using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Data.Migrations
{
    public partial class OnDeleteSetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_WareHouses_WareHouseId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Companies_CompanyId",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_UserId",
                table: "Invitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation");

            migrationBuilder.RenameTable(
                name: "Invitation",
                newName: "Invitations");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_UserId",
                table: "Invitations",
                newName: "IX_Invitations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_CompanyId",
                table: "Invitations",
                newName: "IX_Invitations_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Companies_CompanyId",
                table: "Invitations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_UserId",
                table: "Invitations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_WareHouses_WareHouseId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Companies_CompanyId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_UserId",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitation");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_UserId",
                table: "Invitation",
                newName: "IX_Invitation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_CompanyId",
                table: "Invitation",
                newName: "IX_Invitation_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_WareHouses_WareHouseId",
                table: "Companies",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Companies_CompanyId",
                table: "Invitation",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_UserId",
                table: "Invitation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

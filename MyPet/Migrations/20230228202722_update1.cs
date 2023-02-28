using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImages_Headphones_headphoneModelId",
                table: "ExtraImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Headphones",
                table: "Headphones");

            migrationBuilder.RenameTable(
                name: "Headphones",
                newName: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ExtraImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImages_Products_headphoneModelId",
                table: "ExtraImages",
                column: "headphoneModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImages_Products_headphoneModelId",
                table: "ExtraImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Headphones");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ExtraImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Headphones",
                table: "Headphones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImages_Headphones_headphoneModelId",
                table: "ExtraImages",
                column: "headphoneModelId",
                principalTable: "Headphones",
                principalColumn: "Id");
        }
    }
}

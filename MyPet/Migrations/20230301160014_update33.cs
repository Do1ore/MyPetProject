using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class update33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImages_Products_headphoneModelId",
                table: "ExtraImages");

            migrationBuilder.RenameColumn(
                name: "headphoneModelId",
                table: "ExtraImages",
                newName: "ProductModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraImages_headphoneModelId",
                table: "ExtraImages",
                newName: "IX_ExtraImages_ProductModelId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ExtraImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImages_Products_ProductModelId",
                table: "ExtraImages",
                column: "ProductModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImages_Products_ProductModelId",
                table: "ExtraImages");

            migrationBuilder.RenameColumn(
                name: "ProductModelId",
                table: "ExtraImages",
                newName: "headphoneModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraImages_ProductModelId",
                table: "ExtraImages",
                newName: "IX_ExtraImages_headphoneModelId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ExtraImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImages_Products_headphoneModelId",
                table: "ExtraImages",
                column: "headphoneModelId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}

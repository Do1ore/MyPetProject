using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImageModel_Headphones_headphoneModelId",
                table: "ExtraImageModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtraImageModel",
                table: "ExtraImageModel");

            migrationBuilder.RenameTable(
                name: "ExtraImageModel",
                newName: "ExtraImages");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraImageModel_headphoneModelId",
                table: "ExtraImages",
                newName: "IX_ExtraImages_headphoneModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtraImages",
                table: "ExtraImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImages_Headphones_headphoneModelId",
                table: "ExtraImages",
                column: "headphoneModelId",
                principalTable: "Headphones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraImages_Headphones_headphoneModelId",
                table: "ExtraImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtraImages",
                table: "ExtraImages");

            migrationBuilder.RenameTable(
                name: "ExtraImages",
                newName: "ExtraImageModel");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraImages_headphoneModelId",
                table: "ExtraImageModel",
                newName: "IX_ExtraImageModel_headphoneModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtraImageModel",
                table: "ExtraImageModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraImageModel_Headphones_headphoneModelId",
                table: "ExtraImageModel",
                column: "headphoneModelId",
                principalTable: "Headphones",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class review23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Reviews_ReviewId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MyPetUser_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ReviewId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "ReviewMark");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ReviewStorageId");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "ProductReviews",
                newName: "ReviewStorageId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "MyPetUserId",
                table: "ProductReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "ReviewStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewStorageId",
                table: "Reviews",
                column: "ReviewStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_MyPetUserId",
                table: "ProductReviews",
                column: "MyPetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_MyPetUser_MyPetUserId",
                table: "ProductReviews",
                column: "MyPetUserId",
                principalTable: "MyPetUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProductReviews_ReviewStorageId",
                table: "Reviews",
                column: "ReviewStorageId",
                principalTable: "ProductReviews",
                principalColumn: "ReviewStorageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_MyPetUser_MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProductReviews_ReviewStorageId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewStorageId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "ReviewStorageId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReviewMark",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "ReviewStorageId",
                table: "ProductReviews",
                newName: "ReviewId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductReviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ReviewId",
                table: "ProductReviews",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Reviews_ReviewId",
                table: "ProductReviews",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MyPetUser_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "MyPetUser",
                principalColumn: "Id");
        }
    }
}

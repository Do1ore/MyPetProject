using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class review24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_MyPetUser_MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "MyPetUserId",
                table: "ProductReviews");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
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

            migrationBuilder.AddColumn<int>(
                name: "ReviewMark",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReviewText",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "ReviewId");

            migrationBuilder.CreateTable(
                name: "ReviewStorages",
                columns: table => new
                {
                    ReviewStorageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MyPetUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewStorages", x => x.ReviewStorageId);
                    table.ForeignKey(
                        name: "FK_ReviewStorages_MyPetUser_MyPetUserId",
                        column: x => x.MyPetUserId,
                        principalTable: "MyPetUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ReviewStorageId",
                table: "ProductReviews",
                column: "ReviewStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewStorages_MyPetUserId",
                table: "ReviewStorages",
                column: "MyPetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_ReviewStorages_ReviewStorageId",
                table: "ProductReviews",
                column: "ReviewStorageId",
                principalTable: "ReviewStorages",
                principalColumn: "ReviewStorageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_ReviewStorages_ReviewStorageId",
                table: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ReviewStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ReviewStorageId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ReviewMark",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ReviewText",
                table: "ProductReviews");

            migrationBuilder.AddColumn<string>(
                name: "MyPetUserId",
                table: "ProductReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "ReviewStorageId");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ReviewStorageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewMark = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_ProductReviews_ReviewStorageId",
                        column: x => x.ReviewStorageId,
                        principalTable: "ProductReviews",
                        principalColumn: "ReviewStorageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_MyPetUserId",
                table: "ProductReviews",
                column: "MyPetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewStorageId",
                table: "Reviews",
                column: "ReviewStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_MyPetUser_MyPetUserId",
                table: "ProductReviews",
                column: "MyPetUserId",
                principalTable: "MyPetUser",
                principalColumn: "Id");
        }
    }
}

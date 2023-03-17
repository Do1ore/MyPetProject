using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class news1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsApiSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sourses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsApiSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsApiSettings");
        }
    }
}

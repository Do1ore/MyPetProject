using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class inti2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Headphones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SummaryStroke = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketLaunchDate = table.Column<int>(type: "int", nullable: true),
                    Appointment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BluetoothVersion = table.Column<double>(type: "float", nullable: true),
                    BatteryСapacity = table.Column<double>(type: "float", nullable: true),
                    ChargingTime = table.Column<double>(type: "float", nullable: true),
                    MaxRunTime = table.Column<double>(type: "float", nullable: true),
                    MaxRunTimeWithCase = table.Column<double>(type: "float", nullable: true),
                    MainFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTimeEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParsedUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headphones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraImageModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    headphoneModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraImageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraImageModel_Headphones_headphoneModelId",
                        column: x => x.headphoneModelId,
                        principalTable: "Headphones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtraImageModel_headphoneModelId",
                table: "ExtraImageModel",
                column: "headphoneModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraImageModel");

            migrationBuilder.DropTable(
                name: "Headphones");
        }
    }
}

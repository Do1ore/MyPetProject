using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations
{
    /// <inheritdoc />
    public partial class newUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appointment",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BatteryСapacity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BluetoothVersion",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ChargingTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ConnectionType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ConstructionType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MarketLaunchDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaxRunTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaxRunTimeWithCase",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Headphones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    SummaryStroke = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketLaunchDate = table.Column<int>(type: "int", nullable: true),
                    Appointment = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BluetoothVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatteryСapacity = table.Column<double>(type: "float", nullable: true),
                    ChargingTime = table.Column<double>(type: "float", nullable: true),
                    MaxRunTime = table.Column<double>(type: "float", nullable: true),
                    MaxRunTimeWithCase = table.Column<double>(type: "float", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTimeEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headphones", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headphones");

            migrationBuilder.AddColumn<int>(
                name: "Appointment",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BatteryСapacity",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BluetoothVersion",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ChargingTime",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstructionType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MarketLaunchDate",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxRunTime",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxRunTimeWithCase",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

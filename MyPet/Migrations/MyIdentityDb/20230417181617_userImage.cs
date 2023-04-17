using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPet.Migrations.MyIdentityDb
{
    /// <inheritdoc />
    public partial class userImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathToProfileImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToProfileImage",
                table: "AspNetUsers");
        }
    }
}

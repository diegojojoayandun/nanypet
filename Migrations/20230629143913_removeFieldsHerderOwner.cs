using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NanyPet.Api.Migrations
{
    public partial class removeFieldsHerderOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "herders");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "herders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "owners",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "owners",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "herders",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "herders",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

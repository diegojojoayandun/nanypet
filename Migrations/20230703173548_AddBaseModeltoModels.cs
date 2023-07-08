using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NanyPet.Api.Migrations
{
    public partial class AddBaseModeltoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "pets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "pets",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "owners",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "owners",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "herders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "herders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "appointments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "appointments",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "herders");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "herders");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "appointments");
        }
    }
}

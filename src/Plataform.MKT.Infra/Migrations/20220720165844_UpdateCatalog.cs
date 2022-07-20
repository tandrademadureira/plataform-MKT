using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Plataform.MKT.Infra.Migrations
{
    public partial class UpdateCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Catalog",
                newName: "Mark");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Catalog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Catalog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataRequisition",
                table: "Catalog",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Catalog",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "DataRequisition",
                table: "Catalog");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Catalog");

            migrationBuilder.RenameColumn(
                name: "Mark",
                table: "Catalog",
                newName: "Name");
        }
    }
}

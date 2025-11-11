using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Upgrader.Migrations
{
    /// <inheritdoc />
    public partial class ExtendTaskWithTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxListItemsCount",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinListItemsCount",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<string>>(
                name: "ListItems",
                table: "TaskResults",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxListItemsCount",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MinListItemsCount",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ListItems",
                table: "TaskResults");
        }
    }
}

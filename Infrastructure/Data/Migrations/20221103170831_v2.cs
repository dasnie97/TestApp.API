using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "TestStep");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "LogFiles");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "TestStep",
                newName: "TestDateTimeStarted");

            migrationBuilder.RenameColumn(
                name: "ExecutionStarted",
                table: "TestStep",
                newName: "RecordCreated");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "LogFiles",
                newName: "TestDateTimeStarted");

            migrationBuilder.RenameColumn(
                name: "ExecutionStarted",
                table: "LogFiles",
                newName: "RecordCreated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestDateTimeStarted",
                table: "TestStep",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "RecordCreated",
                table: "TestStep",
                newName: "ExecutionStarted");

            migrationBuilder.RenameColumn(
                name: "TestDateTimeStarted",
                table: "LogFiles",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "RecordCreated",
                table: "LogFiles",
                newName: "ExecutionStarted");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TestStep",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "LogFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RecreatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workstations",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workstations", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TestReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkstationName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsFirstPass = table.Column<bool>(type: "bit", nullable: false),
                    IsFalseCall = table.Column<bool>(type: "bit", nullable: false),
                    ProcessStep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Failure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FixtureSocket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDateTimeStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestReports_Workstations_WorkstationName",
                        column: x => x.WorkstationName,
                        principalTable: "Workstations",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestReports_WorkstationName",
                table: "TestReports",
                column: "WorkstationName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestReports");

            migrationBuilder.DropTable(
                name: "Workstations");
        }
    }
}

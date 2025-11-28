using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Upgrader.Migrations
{
    /// <inheritdoc />
    public partial class CourseAnalyze : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseAnalyzeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAnalyzeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAnalyzeRequests_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAnalyzeRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseAnalyzeResults",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAnalyzeResults", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_CourseAnalyzeResults_CourseAnalyzeRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CourseAnalyzeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAnalyzeRequests_CourseId_UserId",
                table: "CourseAnalyzeRequests",
                columns: new[] { "CourseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAnalyzeRequests_UserId",
                table: "CourseAnalyzeRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAnalyzeResults");

            migrationBuilder.DropTable(
                name: "CourseAnalyzeRequests");
        }
    }
}

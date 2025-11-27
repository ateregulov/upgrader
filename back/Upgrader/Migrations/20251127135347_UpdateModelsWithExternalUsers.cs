using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Upgrader.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsWithExternalUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAnalyzeRequests_Users_UserId",
                table: "CourseAnalyzeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePurchases_Users_UserId",
                table: "CoursePurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskResults_Users_UserId",
                table: "TaskResults");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TaskResults",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUserId",
                table: "TaskResults",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CoursePurchases",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUserId",
                table: "CoursePurchases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CourseAnalyzeRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalUserId",
                table: "CourseAnalyzeRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskResults_ExternalUserId_TaskId",
                table: "TaskResults",
                columns: new[] { "ExternalUserId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAnalyzeRequests_CourseId_ExternalUserId",
                table: "CourseAnalyzeRequests",
                columns: new[] { "CourseId", "ExternalUserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAnalyzeRequests_Users_UserId",
                table: "CourseAnalyzeRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePurchases_Users_UserId",
                table: "CoursePurchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskResults_Users_UserId",
                table: "TaskResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAnalyzeRequests_Users_UserId",
                table: "CourseAnalyzeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePurchases_Users_UserId",
                table: "CoursePurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskResults_Users_UserId",
                table: "TaskResults");

            migrationBuilder.DropIndex(
                name: "IX_TaskResults_ExternalUserId_TaskId",
                table: "TaskResults");

            migrationBuilder.DropIndex(
                name: "IX_CourseAnalyzeRequests_CourseId_ExternalUserId",
                table: "CourseAnalyzeRequests");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "TaskResults");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "CoursePurchases");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "CourseAnalyzeRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TaskResults",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CoursePurchases",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CourseAnalyzeRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAnalyzeRequests_Users_UserId",
                table: "CourseAnalyzeRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePurchases_Users_UserId",
                table: "CoursePurchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskResults_Users_UserId",
                table: "TaskResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Communities_CommunityId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CommunityId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "Communities",
                newName: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_ApplicationUserId",
                schema: "Identity",
                table: "Communities",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_ApplicationUserId",
                schema: "Identity",
                table: "Communities",
                column: "ApplicationUserId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_ApplicationUserId",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_ApplicationUserId",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                schema: "Identity",
                table: "Communities",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                schema: "Identity",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "Identity",
                table: "Communities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Identity",
                table: "Communities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "Identity",
                table: "Communities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommunityId",
                schema: "Identity",
                table: "Users",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Communities_CommunityId",
                schema: "Identity",
                table: "Users",
                column: "CommunityId",
                principalSchema: "Identity",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations.ApplicationDb
{
    public partial class AddMessengerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Communities_CommunityId",
                table: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Communities",
                table: "Communities");

            migrationBuilder.RenameTable(
                name: "Communities",
                newName: "Community");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Community",
                table: "Community",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Community_CommunityId",
                table: "ApplicationUser",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Community_CommunityId",
                table: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Community",
                table: "Community");

            migrationBuilder.RenameTable(
                name: "Community",
                newName: "Communities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Communities",
                table: "Communities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Communities_CommunityId",
                table: "ApplicationUser",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

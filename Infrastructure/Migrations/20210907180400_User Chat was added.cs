using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UserChatwasadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Districts_DistrictId",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "Users",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chat",
                schema: "Identity",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                schema: "Identity",
                table: "Communities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Districts_DistrictId",
                schema: "Identity",
                table: "Communities",
                column: "DistrictId",
                principalSchema: "Identity",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Districts_DistrictId",
                schema: "Identity",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Chat",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "Users",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                schema: "Identity",
                table: "Communities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Districts_DistrictId",
                schema: "Identity",
                table: "Communities",
                column: "DistrictId",
                principalSchema: "Identity",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

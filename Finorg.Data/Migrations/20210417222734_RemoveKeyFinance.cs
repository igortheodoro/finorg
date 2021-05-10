using Microsoft.EntityFrameworkCore.Migrations;

namespace Finorg.Data.Migrations
{
    public partial class RemoveKeyFinance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Finances",
                table: "Finances");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Finances",
                newName: "ChatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Finances",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Finances",
                table: "Finances",
                column: "Id");
        }
    }
}

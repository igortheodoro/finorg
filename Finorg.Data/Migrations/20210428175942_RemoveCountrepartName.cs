using Microsoft.EntityFrameworkCore.Migrations;

namespace Finorg.Data.Migrations
{
    public partial class RemoveCountrepartName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CounterpartName",
                table: "Finances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CounterpartName",
                table: "Finances",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

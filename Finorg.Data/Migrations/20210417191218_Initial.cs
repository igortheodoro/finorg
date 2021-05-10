using Microsoft.EntityFrameworkCore.Migrations;

namespace Finorg.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Finances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Transaction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CounterpartName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Finances");
        }
    }
}

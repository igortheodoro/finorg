using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Finorg.Data.Migrations
{
    public partial class AddPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Finances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Finances",
                table: "Finances",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Finances",
                table: "Finances");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Finances");
        }
    }
}

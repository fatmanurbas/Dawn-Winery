using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawn_Winery.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quality",
                table: "RawMaterial",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "EndProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quality",
                table: "RawMaterial");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "EndProduct");
        }
    }
}

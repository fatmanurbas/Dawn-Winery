using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawn_Winery.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndProduct",
                columns: table => new
                {
                    Mname = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Aging = table.Column<int>(type: "integer", nullable: false),
                    Quality = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<bool>(type: "boolean", nullable: false),
                    Milil = table.Column<int>(type: "integer", nullable: false),
                    Bottle = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndProduct", x => x.Mname);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterial",
                columns: table => new
                {
                    Hid = table.Column<string>(type: "text", nullable: false),
                    Hname = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<bool>(type: "boolean", nullable: false),
                    Alcohol = table.Column<int>(type: "integer", nullable: false),
                    Sweet = table.Column<int>(type: "integer", nullable: false),
                    Acidity = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<int>(type: "integer", nullable: false),
                    Tannin = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterial", x => x.Hid);
                });

            migrationBuilder.CreateTable(
                name: "Receipe",
                columns: table => new
                {
                    Rname = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<bool>(type: "boolean", nullable: false),
                    Grape1 = table.Column<string>(type: "text", nullable: false),
                    G1Kilo = table.Column<int>(type: "integer", nullable: false),
                    Grape2 = table.Column<string>(type: "text", nullable: true),
                    G2Kilo = table.Column<int>(type: "integer", nullable: true),
                    Grape3 = table.Column<string>(type: "text", nullable: true),
                    G3Kilo = table.Column<int>(type: "integer", nullable: true),
                    Grape4 = table.Column<string>(type: "text", nullable: true),
                    G4Kilo = table.Column<int>(type: "integer", nullable: true),
                    Grape5 = table.Column<string>(type: "text", nullable: true),
                    G5Kilo = table.Column<int>(type: "integer", nullable: true),
                    Grape6 = table.Column<string>(type: "text", nullable: true),
                    G6Kilo = table.Column<int>(type: "integer", nullable: true),
                    SO2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipe", x => x.Rname);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndProduct");

            migrationBuilder.DropTable(
                name: "RawMaterial");

            migrationBuilder.DropTable(
                name: "Receipe");
        }
    }
}

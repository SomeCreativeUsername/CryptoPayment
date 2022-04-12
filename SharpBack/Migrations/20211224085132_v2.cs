using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Address = table.Column<string>(nullable: false),
                    PrivateKey = table.Column<string>(nullable: false),
                    InputAmount = table.Column<string>(nullable: true),
                    AmountGas = table.Column<string>(nullable: true),
                    Comission = table.Column<string>(nullable: true),
                    ComissionGas = table.Column<string>(nullable: true),
                    TransactionHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Address);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}

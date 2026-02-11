using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "automobili",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kilometraza = table.Column<long>(type: "bigint", nullable: false),
                    Godiste = table.Column<long>(type: "bigint", nullable: false),
                    Sedista = table.Column<long>(type: "bigint", nullable: false),
                    CenaPoDanu = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automobili", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "korisnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojVozacke = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_korisnici", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "iznajmljeni",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DanIznajmljen = table.Column<long>(type: "bigint", nullable: true),
                    IznajmljenAutomobilFK = table.Column<int>(type: "int", nullable: true),
                    KorisnikIznajmljujeFK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iznajmljeni", x => x.ID);
                    table.ForeignKey(
                        name: "FK_iznajmljeni_automobili_IznajmljenAutomobilFK",
                        column: x => x.IznajmljenAutomobilFK,
                        principalTable: "automobili",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_iznajmljeni_korisnici_KorisnikIznajmljujeFK",
                        column: x => x.KorisnikIznajmljujeFK,
                        principalTable: "korisnici",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_iznajmljeni_IznajmljenAutomobilFK",
                table: "iznajmljeni",
                column: "IznajmljenAutomobilFK",
                unique: true,
                filter: "[IznajmljenAutomobilFK] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_iznajmljeni_KorisnikIznajmljujeFK",
                table: "iznajmljeni",
                column: "KorisnikIznajmljujeFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iznajmljeni");

            migrationBuilder.DropTable(
                name: "automobili");

            migrationBuilder.DropTable(
                name: "korisnici");
        }
    }
}

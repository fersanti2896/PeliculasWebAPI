using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPeliculasAPI.Migrations
{
    /// <inheritdoc />
    public partial class SalasCines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalasCines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalasCines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeliculasSalasCines",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    SalaCineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasSalasCines", x => new { x.PeliculaId, x.SalaCineId });
                    table.ForeignKey(
                        name: "FK_PeliculasSalasCines_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasSalasCines_SalasCines_SalaCineId",
                        column: x => x.SalaCineId,
                        principalTable: "SalasCines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasSalasCines_SalaCineId",
                table: "PeliculasSalasCines",
                column: "SalaCineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculasSalasCines");

            migrationBuilder.DropTable(
                name: "SalasCines");
        }
    }
}

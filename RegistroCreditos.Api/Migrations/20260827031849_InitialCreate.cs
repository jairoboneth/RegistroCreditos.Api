using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RegistroCreditos.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Creditos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    NombreCliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CedulaCliente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ValorCredito = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TasaInteres = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PlazoMeses = table.Column<int>(type: "integer", nullable: false),
                    ComercialNombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creditos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creditos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_CedulaCliente",
                table: "Creditos",
                column: "CedulaCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_ComercialNombre",
                table: "Creditos",
                column: "ComercialNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_FechaRegistro",
                table: "Creditos",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_UsuarioId",
                table: "Creditos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creditos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

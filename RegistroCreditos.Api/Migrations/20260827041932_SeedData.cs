using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RegistroCreditos.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nombre", "PasswordHash" },
                values: new object[] { 1, "test@empresa.com", "Usuario de Pruebas", "$2a$11$g6.B3h9gR18B.E.X8bZ31ui/5jA1p49G1eS0Fw8m7.N885D5K67O6" });

            migrationBuilder.InsertData(
                table: "Creditos",
                columns: new[] { "Id", "CedulaCliente", "ComercialNombre", "FechaRegistro", "NombreCliente", "PlazoMeses", "TasaInteres", "UsuarioId", "ValorCredito" },
                values: new object[,]
                {
                    { new Guid("    "), "1000000001", "Sede Norte", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pepito Perez", 10, 2m, 1, 7800000m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "1000000002", "Sede Sur", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maria Perez", 5, 2m, 1, 12500000m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "1000000003", "Sede Centro", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Antonio Rodriguez", 5, 2m, 1, 10312673m },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "1000000004", "Sede Este", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Giselle López", 12, 2m, 1, 8628510m },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "1000000005", "Sede Oeste", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Martha Perez", 24, 2m, 1, 5889085m },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "1000000006", "Sede Norte", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Isaac llanos", 48, 2m, 1, 14793565m },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "1000000007", "Sede Sur", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teresa Gutierrez", 50, 2m, 1, 8072348m },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "1000000008", "Sede Centro", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Isabel Llanos", 60, 2m, 1, 5143860m },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "1000000009", "Sede Este", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Paola Tao", 24, 2m, 1, 12881963m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "1000000010", "Sede Oeste", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wendy Moscoso", 40, 2m, 1, 13484682m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Creditos",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

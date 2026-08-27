using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroCreditos.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHashOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$GCi/1BMMsbfUyEANY2xaNu5t.5j0Vw4bg1HBHr0ojat0BITZxZNeG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$g6.B3h9gR18B.E.X8bZ31ui/5jA1p49G1eS0Fw8m7.N885D5K67O6");
        }
    }
}

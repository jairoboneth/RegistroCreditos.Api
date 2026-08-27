using System.Data;
using Dapper;
using RegistroCreditos.Api.DTOs.Credito;

namespace RegistroCreditos.Api.Services;

public class CreditoQueryService : ICreditoQueryService
{
    private readonly IDbConnection _dbConnection;

    public CreditoQueryService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CreditoDto?> GetCreditoByIdAsync(Guid id)
    {
        var sql = @"
            SELECT c.""Id"", c.""UsuarioId"", c.""NombreCliente"", c.""CedulaCliente"", c.""ComercialNombre"", 
                   c.""ValorCredito"", c.""TasaInteres"", c.""PlazoMeses"", c.""FechaRegistro"",
                   u.""Nombre"" as ""NombreUsuario""
            FROM ""Creditos"" c
            INNER JOIN ""Usuarios"" u ON c.""UsuarioId"" = u.""Id""
            WHERE c.""Id"" = @Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<CreditoDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<CreditoDto>> GetAllCreditosAsync(string? filter, string? sortBy)
    {
        var sql = @"
            SELECT c.""Id"", c.""UsuarioId"", c.""NombreCliente"", c.""CedulaCliente"", c.""ComercialNombre"", 
                   c.""ValorCredito"", c.""TasaInteres"", c.""PlazoMeses"", c.""FechaRegistro"",
                   u.""Nombre"" as ""NombreUsuario""
            FROM ""Creditos"" c
            INNER JOIN ""Usuarios"" u ON c.""UsuarioId"" = u.""Id""
            WHERE 1=1";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            sql += @" AND (c.""CedulaCliente"" ILIKE @Filter OR c.""ComercialNombre"" ILIKE @Filter)";
            parameters.Add("Filter", $"%{filter}%");
        }

        sql += sortBy?.ToLower() switch
        {
            "fecha" => @" ORDER BY c.""FechaRegistro"" DESC",
            "valor" => @" ORDER BY c.""ValorCredito"" DESC",
            _ => @" ORDER BY c.""FechaRegistro"" DESC"
        };

        return await _dbConnection.QueryAsync<CreditoDto>(sql, parameters);
    }
}

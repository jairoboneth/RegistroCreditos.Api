namespace RegistroCreditos.Api.Models;

public class Credito
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string CedulaCliente { get; set; } = string.Empty;
    public decimal ValorCredito { get; set; }
    public decimal TasaInteres { get; set; }
    public int PlazoMeses { get; set; }
    public string ComercialNombre { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // RelaciÃ³n
    public Usuario Usuario { get; set; } = null!;
}

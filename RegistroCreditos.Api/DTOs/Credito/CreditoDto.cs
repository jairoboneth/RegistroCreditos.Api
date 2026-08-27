namespace RegistroCreditos.Api.DTOs.Credito;

public class CreditoDto
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public string CedulaCliente { get; set; } = string.Empty;
    public string ComercialNombre { get; set; } = string.Empty;
    public decimal ValorCredito { get; set; }
    public decimal TasaInteres { get; set; }
    public int PlazoMeses { get; set; }
    public DateTime FechaRegistro { get; set; }
}

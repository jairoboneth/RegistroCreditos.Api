namespace RegistroCreditos.Api.DTOs;

public class EmailPayload
{
    public string NombreCliente { get; set; } = string.Empty;
    public decimal ValorCredito { get; set; }
    public string NombreComercial { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
}

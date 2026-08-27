using RegistroCreditos.Api.DTOs.Credito;

namespace RegistroCreditos.Api.Services;

public interface ICreditoService
{
    Task<CreditoDto> CreateCreditoAsync(CrearCreditoDto dto, int usuarioId, string nombreUsuario);
}

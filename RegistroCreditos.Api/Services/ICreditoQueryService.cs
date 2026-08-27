using RegistroCreditos.Api.DTOs.Credito;

namespace RegistroCreditos.Api.Services;

public interface ICreditoQueryService
{
    Task<CreditoDto?> GetCreditoByIdAsync(Guid id);
    Task<IEnumerable<CreditoDto>> GetAllCreditosAsync(string? filter, string? sortBy);
}

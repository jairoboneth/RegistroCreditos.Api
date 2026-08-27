using RegistroCreditos.Api.Data;
using RegistroCreditos.Api.DTOs;
using RegistroCreditos.Api.DTOs.Credito;
using RegistroCreditos.Api.Models;

namespace RegistroCreditos.Api.Services;

public class CreditoService : ICreditoService
{
    private readonly ApplicationDbContext _context;
    private readonly Coravel.Queuing.Interfaces.IQueue _queue;

    public CreditoService(ApplicationDbContext context, Coravel.Queuing.Interfaces.IQueue queue)
    {
        _context = context;
        _queue = queue;
    }

    public async Task<CreditoDto> CreateCreditoAsync(CrearCreditoDto dto, int usuarioId, string nombreUsuario)
    {
        var credito = new Credito
        {
            UsuarioId = usuarioId,
            NombreCliente = dto.NombreCliente,
            CedulaCliente = dto.CedulaCliente,
            ComercialNombre = dto.ComercialNombre,
            ValorCredito = dto.ValorCredito,
            TasaInteres = dto.TasaInteres,
            PlazoMeses = dto.PlazoMeses,
            FechaRegistro = DateTime.UtcNow
        };

        _context.Creditos.Add(credito);
        await _context.SaveChangesAsync();

        var resultDto = new CreditoDto
        {
            Id = credito.Id,
            UsuarioId = credito.UsuarioId,
            NombreUsuario = nombreUsuario,
            NombreCliente = credito.NombreCliente,
            CedulaCliente = credito.CedulaCliente,
            ComercialNombre = credito.ComercialNombre,
            ValorCredito = credito.ValorCredito,
            TasaInteres = credito.TasaInteres,
            PlazoMeses = credito.PlazoMeses,
            FechaRegistro = credito.FechaRegistro
        };

        _queue.QueueInvocableWithPayload<EmailJob, EmailPayload>(new EmailPayload
        {
            NombreCliente = credito.NombreCliente,
            ValorCredito = credito.ValorCredito,
            NombreComercial = credito.ComercialNombre,
            FechaRegistro = credito.FechaRegistro
        });

        return resultDto;
    }
}

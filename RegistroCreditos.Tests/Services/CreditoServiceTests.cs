using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RegistroCreditos.Api.Data;
using RegistroCreditos.Api.DTOs;
using RegistroCreditos.Api.DTOs.Credito;
using RegistroCreditos.Api.Services;
using Coravel.Queuing.Interfaces;
using Xunit;

namespace RegistroCreditos.Tests.Services;

public class CreditoServiceTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IQueue> _queueMock;
    private readonly CreditoService _creditoService;

    public CreditoServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _queueMock = new Mock<IQueue>();
        
        _creditoService = new CreditoService(_dbContext, _queueMock.Object);
    }

    [Fact]
    [Trait("Category", "positive")]
    [Trait("Category", "critical-path")]
    [Trait("Category", "integration")]
    public async Task CreateCreditoAsync_Should_Save_Credito_And_Queue_Email()
    {
        // Arrange
        var dto = new CrearCreditoDto
        {
            NombreCliente = "Test Cliente",
            CedulaCliente = "1234567890",
            ComercialNombre = "Comercial 1",
            ValorCredito = 5000,
            TasaInteres = 10.5m,
            PlazoMeses = 24
        };
        var usuarioId = 1;
        var nombreUsuario = "UserTest";

        // Act
        var result = await _creditoService.CreateCreditoAsync(dto, usuarioId, nombreUsuario);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.NombreCliente.Should().Be(dto.NombreCliente);
        result.CedulaCliente.Should().Be(dto.CedulaCliente);
        result.ValorCredito.Should().Be(dto.ValorCredito);
        result.TasaInteres.Should().Be(dto.TasaInteres);
        result.PlazoMeses.Should().Be(dto.PlazoMeses);
        result.UsuarioId.Should().Be(usuarioId);
        result.NombreUsuario.Should().Be(nombreUsuario);
        result.FechaRegistro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        // Verify in memory db
        var savedCredito = await _dbContext.Creditos.FirstOrDefaultAsync(c => c.Id == result.Id);
        savedCredito.Should().NotBeNull();
        savedCredito!.NombreCliente.Should().Be(dto.NombreCliente);
        savedCredito.FechaRegistro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        
        // Verify queue
        _queueMock.Verify(q => q.QueueInvocableWithPayload<EmailJob, EmailPayload>(
            It.Is<EmailPayload>(p => 
                p.NombreCliente == dto.NombreCliente &&
                p.ValorCredito == dto.ValorCredito &&
                p.NombreComercial == dto.ComercialNombre
            )), Times.Once);
    }
}



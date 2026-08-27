using FluentValidation.TestHelper;
using RegistroCreditos.Api.DTOs.Credito;
using RegistroCreditos.Api.Validators;
using Xunit;

namespace RegistroCreditos.Tests.Validators;

public class CrearCreditoDtoValidatorTests
{
    private readonly CrearCreditoDtoValidator _validator;

    public CrearCreditoDtoValidatorTests()
    {
        _validator = new CrearCreditoDtoValidator();
    }

    [Fact]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_NombreCliente_Is_Empty()
    {
        var model = new CrearCreditoDto { NombreCliente = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NombreCliente);
    }

    [Fact]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_NombreCliente_Is_Specified()
    {
        var model = new CrearCreditoDto { NombreCliente = "Juan Perez" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.NombreCliente);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_CedulaCliente_Is_NullOrEmpty(string? cedula)
    {
        var model = new CrearCreditoDto { CedulaCliente = cedula };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CedulaCliente);
    }

    [Fact]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_CedulaCliente_Is_Specified()
    {
        var model = new CrearCreditoDto { CedulaCliente = "123456789" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.CedulaCliente);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_ComercialNombre_Is_NullOrEmpty(string? comercial)
    {
        var model = new CrearCreditoDto { ComercialNombre = comercial };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ComercialNombre);
    }

    [Fact]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_ComercialNombre_Is_Specified()
    {
        var model = new CrearCreditoDto { ComercialNombre = "Comercial de prueba" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ComercialNombre);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_ValorCredito_Is_Less_Than_Or_Equal_To_Zero(decimal valor)
    {
        var model = new CrearCreditoDto { ValorCredito = valor };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ValorCredito);
    }

    [Fact]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_ValorCredito_Is_Greater_Than_Zero()
    {
        var model = new CrearCreditoDto { ValorCredito = 5000 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ValorCredito);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_TasaInteres_Is_Less_Than_Or_Equal_To_Zero(decimal tasa)
    {
        var model = new CrearCreditoDto { TasaInteres = tasa };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.TasaInteres);
    }

    [Fact]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_TasaInteres_Is_Greater_Than_Zero()
    {
        var model = new CrearCreditoDto { TasaInteres = 10.5m };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TasaInteres);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-12)]
    [InlineData(361)]
    [Trait("Category", "negative")]
    [Trait("Category", "boundary")]
    public void Should_Have_Error_When_PlazoMeses_Is_Invalid(int plazo)
    {
        var model = new CrearCreditoDto { PlazoMeses = plazo };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PlazoMeses);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    [InlineData(360)]
    [Trait("Category", "positive")]
    public void Should_Not_Have_Error_When_PlazoMeses_Is_Valid(int plazo)
    {
        var model = new CrearCreditoDto { PlazoMeses = plazo };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.PlazoMeses);
    }
}



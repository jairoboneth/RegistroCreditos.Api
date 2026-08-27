using FluentValidation;
using RegistroCreditos.Api.DTOs.Credito;

namespace RegistroCreditos.Api.Validators;

public class CrearCreditoDtoValidator : AbstractValidator<CrearCreditoDto>
{
    public CrearCreditoDtoValidator()
    {
        RuleFor(x => x.NombreCliente).NotEmpty().WithMessage("El nombre del cliente es obligatorio.");
        RuleFor(x => x.CedulaCliente).NotEmpty().WithMessage("La cÃ©dula del cliente es obligatoria.");
        RuleFor(x => x.ComercialNombre).NotEmpty().WithMessage("El nombre del comercial es obligatorio.");

        RuleFor(x => x.ValorCredito)
            .GreaterThan(0).WithMessage("El valor del crÃ©dito debe ser mayor a cero.");
            
        RuleFor(x => x.TasaInteres)
            .GreaterThan(0).WithMessage("La tasa de interÃ©s debe ser mayor a cero.");

        RuleFor(x => x.PlazoMeses)
            .GreaterThan(0).WithMessage("El plazo debe ser mayor a cero meses.")
            .LessThanOrEqualTo(360).WithMessage("El plazo mÃ¡ximo permitido es de 360 meses.");
    }
}

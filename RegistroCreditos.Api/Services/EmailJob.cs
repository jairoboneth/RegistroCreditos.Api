using System.Net;
using System.Net.Mail;
using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using RegistroCreditos.Api.DTOs;

namespace RegistroCreditos.Api.Services;

public class EmailJob : IInvocable, IInvocableWithPayload<EmailPayload>
{
    private readonly IConfiguration _configuration;
    public EmailPayload Payload { get; set; } = null!;

    public EmailJob(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Invoke()
    {
        var smtpSettings = _configuration.GetSection("Smtp");
        var host = smtpSettings["Host"] ?? "localhost";
        var port = int.Parse(smtpSettings["Port"] ?? "1025");
        var from = smtpSettings["From"] ?? "no-reply@registrocreditos.com";
        var to = smtpSettings["To"] ?? "notificaciones@registrocreditos.com";
        var user = smtpSettings["User"];
        var password = smtpSettings["Password"];

        var message = new MailMessage
        {
            From = new MailAddress(from),
            Subject = "Nuevo Crédito Registrado",
            Body = $@"
                <h1>Nuevo Crédito Registrado</h1>
                <p><strong>Cliente:</strong> {Payload.NombreCliente}</p>
                <p><strong>Comercial:</strong> {Payload.NombreComercial}</p>
                <p><strong>Valor del Crédito:</strong> {Payload.ValorCredito:C}</p>
                <p><strong>Fecha:</strong> {Payload.FechaRegistro}</p>
            ",
            IsBodyHtml = true
        };
        message.To.Add(to);

        using var client = new SmtpClient(host, port);
        
        if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
        {
            client.Credentials = new NetworkCredential(user, password);
            client.EnableSsl = true;
        }
        
        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando correo: {ex.Message}");
        }
    }
}
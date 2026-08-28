using System.Net;
using System.Net.Mail;
using System.Net.Http.Headers;
using System.Text;
using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RegistroCreditos.Api.DTOs;

namespace RegistroCreditos.Api.Services;

public class EmailJob : IInvocable, IInvocableWithPayload<EmailPayload>
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private static readonly HttpClient _httpClient = new HttpClient();
    
    public EmailPayload Payload { get; set; } = null!;

    public EmailJob(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task Invoke()
    {
        var htmlBody = $@"
            <h1>Nuevo Crédito Registrado</h1>
            <p><strong>Cliente:</strong> {Payload.NombreCliente}</p>
            <p><strong>Comercial:</strong> {Payload.NombreComercial}</p>
            <p><strong>Valor del Crédito:</strong> {Payload.ValorCredito:C}</p>
            <p><strong>Fecha:</strong> {Payload.FechaRegistro:yyyy-MM-dd HH:mm}</p>";

        if (_environment.IsDevelopment())
        {
            var smtpSettings = _configuration.GetSection("Smtp");
            await SendViaSmtpAsync(smtpSettings, htmlBody);
        }
        else
        {
            var mailgunSettings = _configuration.GetSection("Mailgun");
            await SendViaMailgunAsync(mailgunSettings, htmlBody);
        }
    }

    private async Task SendViaSmtpAsync(IConfigurationSection smtpSettings, string htmlBody)
    {
        var host = smtpSettings["Host"] ?? "localhost";
        var port = int.Parse(smtpSettings["Port"] ?? "1025");
        var from = smtpSettings["From"] ?? "no-reply@registrocreditos.com";
        var to = smtpSettings["To"] ?? "notificaciones@registrocreditos.com";

        var message = new MailMessage
        {
            From = new MailAddress(from),
            Subject = "Nuevo Crédito Registrado",
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        try
        {
            using var client = new SmtpClient(host, port);
            await client.SendMailAsync(message);
            Console.WriteLine($"[Dev] Correo enviado exitosamente vía SMTP a {host}:{port}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Dev] Error enviando correo SMTP: {ex.Message}");
        }
    }

    private async Task SendViaMailgunAsync(IConfigurationSection mailgunSettings, string htmlBody)
    {
        var domain = mailgunSettings["Domain"];
        var apiKey = mailgunSettings["ApiKey"];
        var from = mailgunSettings["From"] ?? "no-reply@registrocreditos.com";
        var to = mailgunSettings["To"] ?? "notificaciones@registrocreditos.com";

        if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("[Prod] Advertencia: Faltan credenciales de Mailgun en la configuración.");
            return;
        }

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", from),
            new KeyValuePair<string, string>("to", to),
            new KeyValuePair<string, string>("subject", "Nuevo Crédito Registrado"),
            new KeyValuePair<string, string>("html", htmlBody)
        });

        var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.mailgun.net/v3/{domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
        requestMessage.Content = requestContent;

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                Console.WriteLine($"[Prod] Correo enviado exitosamente vía Mailgun API.");
            else
                Console.WriteLine($"[Prod] Error enviando correo con Mailgun: Status {response.StatusCode} - {responseBody}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Prod] Excepción al conectar con Mailgun: {ex.Message}");
        }
    }
}
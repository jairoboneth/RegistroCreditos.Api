using System.Net.Http.Headers;
using System.Text;
using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using RegistroCreditos.Api.DTOs;

namespace RegistroCreditos.Api.Services;

public class EmailJob : IInvocable, IInvocableWithPayload<EmailPayload>
{
    private readonly IConfiguration _configuration;
    private static readonly HttpClient _httpClient = new HttpClient();
    
    public EmailPayload Payload { get; set; } = null!;

    public EmailJob(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Invoke()
    {
        var mailgunSettings = _configuration.GetSection("Mailgun");
        var domain = mailgunSettings["Domain"];
        var apiKey = mailgunSettings["ApiKey"];
        var from = mailgunSettings["From"] ?? "no-reply@registrocreditos.com";
        var to = mailgunSettings["To"] ?? "notificaciones@registrocreditos.com";

        if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Advertencia: No se encontraron credenciales de Mailgun en la configuración.");
            return;
        }

        var htmlBody = $@"
            <h1>Nuevo Crédito Registrado</h1>
            <p><strong>Cliente:</strong> {Payload.NombreCliente}</p>
            <p><strong>Comercial:</strong> {Payload.NombreComercial}</p>
            <p><strong>Valor del Crédito:</strong> {Payload.ValorCredito:C}</p>
            <p><strong>Fecha:</strong> {Payload.FechaRegistro:yyyy-MM-dd HH:mm}</p>";

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", from),
            new KeyValuePair<string, string>("to", to),
            new KeyValuePair<string, string>("subject", "Nuevo Crédito Registrado"),
            new KeyValuePair<string, string>("html", htmlBody)
        });

        // Configuración de autenticación básica requerida por Mailgun HTTP API
        var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
        
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.mailgun.net/v3/{domain}/messages");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
        requestMessage.Content = requestContent;

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Correo enviado exitosamente vía Mailgun API. Respuesta: {responseBody}");
            }
            else
            {
                Console.WriteLine($"Error enviando correo con Mailgun: Status {response.StatusCode} - {responseBody}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción al conectar con Mailgun: {ex.Message}");
        }
    }
}
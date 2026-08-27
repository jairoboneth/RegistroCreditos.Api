namespace RegistroCreditos.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    // RelaciÃ³n
    public ICollection<Credito> Creditos { get; set; } = new List<Credito>();
}

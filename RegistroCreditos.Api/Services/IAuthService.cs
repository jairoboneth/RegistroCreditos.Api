namespace RegistroCreditos.Api.Services;

public interface IAuthService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    Task<string?> LoginAsync(string email, string password);
}

namespace RegistroCreditos.Api.Services;

public class AuthService : IAuthService
{
    private readonly RegistroCreditos.Api.Data.ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(RegistroCreditos.Api.Data.ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var usuario = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(
            _context.Usuarios, u => u.Email == email);

        if (usuario == null)
            return null;

        if (!VerifyPassword(password, usuario.PasswordHash))
            return null;

        return _jwtService.GenerateToken(usuario);
    }
}

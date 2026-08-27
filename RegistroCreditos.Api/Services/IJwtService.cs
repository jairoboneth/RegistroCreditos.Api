using RegistroCreditos.Api.Models;

namespace RegistroCreditos.Api.Services;

public interface IJwtService
{
    string GenerateToken(Usuario usuario);
}

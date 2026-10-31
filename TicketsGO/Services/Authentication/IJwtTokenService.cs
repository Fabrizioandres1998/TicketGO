using TicketsGO.Models;

namespace TicketsGO.Services;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(Usuario usuario);
}

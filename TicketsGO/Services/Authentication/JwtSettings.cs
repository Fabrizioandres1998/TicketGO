namespace TicketsGO.Services;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = "TicketsGO";
    public string Audience { get; set; } = "TicketsGO.Api";
    public string SigningKey { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; } = 60;
}

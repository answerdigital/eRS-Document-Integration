namespace eRS.API.Models;

public record JwtBearerConfiguration
{
    public string Key { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int LifetimeHours { get; init; }
}
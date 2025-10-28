namespace StudentServicesWebApi.Infrastructure.Configuration;
public class JwtConfiguration
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiryInMinutes { get; set; } = 60; 
    public int RefreshTokenExpiryInDays { get; set; } = 7; 
}

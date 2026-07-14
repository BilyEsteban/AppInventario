namespace AppInventario.Models
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string SecretKey { get; set; } = "AppInventario-Super-Secret-Key-2024-Minimum-32-Chars-Long!";

        public string Issuer { get; set; } = "AppInventario";

        public string Audience { get; set; } = "AppInventario.Users";

        public int ExpirationHours { get; set; } = 24;

        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
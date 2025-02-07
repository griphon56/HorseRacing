namespace HorseRacing.Domain.Common.Models.Authentication.Configuration
{
    /// <summary>
	/// Настройки jwt.
	/// </summary>
	public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        
        public JwtSettings() { }

        public JwtSettings(string secret, int expiryDays, string issuer, string audience)
        {
            Secret = secret;
            ExpiryDays = expiryDays;
            Issuer = issuer;
            Audience = audience;
        }

        /// <summary>
        /// Ключ безопасности
        /// </summary>
        public string Secret { get; init; } = string.Empty;
        /// <summary>
        /// Время действия токена
        /// </summary>
        public int ExpiryDays { get; init; } = 0;
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; init; } = string.Empty;
        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; init; } = string.Empty;

    }
}

namespace HorseRacing.Domain.Common.Models.Authentication.Configuration
{
    /// <summary>
	/// Опции модуля аутентификации jwt.
	/// </summary>
	public class JwtAuthenticationModuleOption : AuthenticationModuleOption
    {
        /// <summary>
        /// Название раздела.
        /// </summary>
        public const string SectionName = $"{AuthenticationModuleSettings.SectionName}:JWT";
        /// <summary>
        /// Название схемы.
        /// </summary>
        public const string SchemeName = $"JwtAuthentication";
        /// <summary>
        /// Название политики.
        /// </summary>
        public const string PolicyName = $"{SchemeName}Policy";
        /// <summary>
        /// Идентификатор сведений о настройках.
        public JwtSettings Settings { get; set; } = new JwtSettings();
    }
}

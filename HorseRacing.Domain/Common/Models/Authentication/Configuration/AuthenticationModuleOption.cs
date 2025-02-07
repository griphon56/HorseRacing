using static HorseRacing.Domain.Common.Models.Authentication.Configuration.Enums;

namespace HorseRacing.Domain.Common.Models.Authentication.Configuration
{
    /// <summary>
	/// Опции модуля аутентификации.
	/// </summary>
	public class AuthenticationModuleOption
    {
        /// <summary>
        /// Идентификатор сведений о типах.
        /// </summary>
        public AuthenticationProviders Type { get; set; } = AuthenticationProviders.NotSet;

        /// <summary>
        /// Идентификатор сведений об имени.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}

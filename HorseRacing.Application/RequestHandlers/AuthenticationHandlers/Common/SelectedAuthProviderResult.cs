using static HorseRacing.Domain.Common.Models.Authentication.Configuration.Enums;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common
{
    /// <summary>
    /// Результат команды выбора провайдера аутентификации.
    /// </summary>
    public class SelectedAuthProviderResult
    {
        /// <summary>
        /// Используем сведения, полученные из провайдера аутентификации <see cref="SelectedAuthProviderResult"/>
        /// </summary>
        /// <param name="providers">Поставщики.</param>
        /// <param name="priority">Приоритет.</param>
        public SelectedAuthProviderResult(List<AuthenticationProviders> providers, AuthenticationProviders priority)
        {
            Providers = providers;
            Priority = priority;
        }
        /// <summary>
        /// Список провайдеров
        /// </summary>
        public IReadOnlyList<AuthenticationProviders> Providers { get; private set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public AuthenticationProviders Priority { get; private set; }
    }
}

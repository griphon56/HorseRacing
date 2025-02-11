using ErrorOr;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.SelectAuthProvider
{
    /// <summary>
    /// Команда выбора провайдера аутентификации.
    /// </summary>
    public class SelectAuthProviderCommand : IRequest<ErrorOr<SelectedAuthProviderResult>>
    {
        /// <summary>
        /// Используем authenticationModules, полученную из опций модулей проверки подлинности, модули из провайдера <see cref="SelectAuthProviderCommand"/> .
        /// </summary>
        /// <param name="authenticationModules">Пользовательские модули проверки подлинности.</param>
        public SelectAuthProviderCommand(List<AuthenticationModuleOption> authenticationModules)
        {
            AuthenticationModules = authenticationModules;
        }
        /// <summary>
        /// Сведения пользовательских модулей проверки подлинности
        /// </summary>
        public IList<AuthenticationModuleOption> AuthenticationModules { get; set; } = new List<AuthenticationModuleOption>();
    }
}

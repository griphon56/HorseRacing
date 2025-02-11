using FluentValidation;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.SelectAuthProvider
{
    /// <summary>
    /// Валидация выбора провайдера аутентификации.
    /// </summary>
    public class SelectAuthProviderCommandValidation : AbstractValidator<SelectAuthProviderCommand>
    {
        /// <summary>
        /// Проверяем, чтобы объект AuthenticationModules не был пустым
        /// </summary>
        public SelectAuthProviderCommandValidation()
        {
            RuleFor(m => m.AuthenticationModules).NotEmpty();
        }
    }
}

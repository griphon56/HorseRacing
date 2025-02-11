using ErrorOr;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.SelectAuthProvider
{
    /// <summary>
    /// Обработчик команды выбора провайдера аутентификации.
    /// </summary>
    public class SelectAuthProviderCommandHandler :
        IRequestHandler<SelectAuthProviderCommand, ErrorOr<SelectedAuthProviderResult>>
    {
        public async Task<ErrorOr<SelectedAuthProviderResult>> Handle(SelectAuthProviderCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Enums.AuthenticationProviders? Priority = Enums.AuthenticationProviders.JWT;

            var ADProvider = request.AuthenticationModules.FirstOrDefault(m => m.Type == Enums.AuthenticationProviders.JWT);
            if (ADProvider != null)
            {
                Priority = ADProvider.Type;
            }

            return new SelectedAuthProviderResult(request.AuthenticationModules.Select(m => m.Type).ToList(), Priority.Value);
        }
    }
}

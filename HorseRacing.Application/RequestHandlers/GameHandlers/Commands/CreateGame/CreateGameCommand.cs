using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame
{
    /// <summary>
    /// Команда создания игры
    /// </summary>
    public class CreateGameCommand : IRequest<ErrorOr<CreateGameResult>>
    {
        public string Name { get; set; } = string.Empty;
    }
}

using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    /// <summary>
    /// Команда подключения к игре
    /// </summary>
    public class JoinGameCommand : IRequest<ErrorOr<JoinGameResult>>
    {
    }
}

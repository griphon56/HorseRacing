using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<ErrorOr<CreateGameResult>>
    {
        public string Name { get; set; } = string.Empty;
    }
}

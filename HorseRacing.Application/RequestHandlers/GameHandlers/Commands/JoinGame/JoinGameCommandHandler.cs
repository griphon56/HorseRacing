using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, ErrorOr<JoinGameResult>>
    {
        public Task<ErrorOr<JoinGameResult>> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

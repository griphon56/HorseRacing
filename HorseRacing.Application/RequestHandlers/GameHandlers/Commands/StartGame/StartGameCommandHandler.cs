using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, ErrorOr<StartGameResult>>
    {
        public Task<ErrorOr<StartGameResult>> Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using ErrorOr;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, ErrorOr<Unit>>
    {
        public Task<ErrorOr<Unit>> Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

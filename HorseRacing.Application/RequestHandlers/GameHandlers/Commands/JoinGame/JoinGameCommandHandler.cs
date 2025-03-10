using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, ErrorOr<JoinGameResult>>
    {
        private readonly ILogger<JoinGameCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public JoinGameCommandHandler(ILogger<JoinGameCommandHandler> logger, IGameRepository gameRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }
        public Task<ErrorOr<JoinGameResult>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(command.UserId);

            throw new NotImplementedException();
        }
    }
}

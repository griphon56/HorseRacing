using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using HorseRacing.Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetAccountBalance
{
    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, ErrorOr<GetAccountBalanceResult>>
    {
        private readonly ILogger<GetAccountBalanceQueryHandler> _logger;
        private readonly IUserRepository _userRepository;

        public GetAccountBalanceQueryHandler(ILogger<GetAccountBalanceQueryHandler> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<GetAccountBalanceResult>> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(request.UserId, cancellationToken) is not User user)
            {
                return Errors.User.UserNotFound;
            }

            return new GetAccountBalanceResult()
            {
                UserId = user.Id,
                Balance = user.Account.Balance
            };
        }
    }
}

using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.UserAggregate;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<GetUserResult>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(ILogger<GetUserQueryHandler> logger, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<GetUserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(request.UserId, cancellationToken) is not User user)
            {
                return Errors.User.UserNotFound;
            }

            return new GetUserResult()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
            };
        }
    }
}

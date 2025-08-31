using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;

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
            var userResult = await _userRepository.GetById(request.UserId);
            if(userResult is null)
                return Errors.User.UserNotFound;

            return new GetUserResult()
            {
                UserName = userResult.UserName,
                FirstName = userResult.FirstName,
                LastName = userResult.LastName,
                Email = userResult.Email,
                Phone = userResult.Phone,
            };
        }
    }
}

using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<Unit>>
    {
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IHashPasswordService _hashPasswordService;

        public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IUserRepository userRepository
            , IHashPasswordService hashPasswordService, IUserService userService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _userService = userService;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userView = await _userService.GetWorkingUser();
            if (userView.IsError)
            {
                if (userView.FirstError == Errors.Authentication.UserNotFound)
                {
                    return Error.Unauthorized(Errors.Authentication.UserNotFound.Code,
                        Errors.Authentication.UserNotFound.Description);
                }
                return userView.FirstError;
            }

            if(request.UserId == userView.Value!.UserId)
            {

            }
        }
    }
}

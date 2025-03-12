using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Authentication;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.EventLogAggregate.Enums;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using HorseRacing.LoggerExtenstions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.RegistrationUsingForms
{
    /// <summary>
    /// Обработчик команды регистрации пользователя
    /// </summary>
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegistrationCommandHandler> _logger;
        private readonly IHashPasswordService _hashPasswordService;

        public RegistrationCommandHandler(IJwtTokenGenerator JwtTokenGenerator, IUserRepository userRepository,
            ILogger<RegistrationCommandHandler> logger, IHashPasswordService hashPasswordService)
        {
            _jwtTokenGenerator = JwtTokenGenerator;
            _userRepository = userRepository;
            _logger = logger;
            _hashPasswordService = hashPasswordService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegistrationCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByUserName(command.UserName) is not null)
            {
                _logger.LogWithInitiatorInfoText(new LoggerExtenstions.Common.EventInfo(
                    "Ошибка регистрации", EventType.RegistrationError
                    , $"В процессе регистрации Пользователя {command.UserName} возникла ошибка {Errors.User.DuplicateUserName.Description}"
                    , LogLevel.Warning, SubsystemType.UsersControlSubSys, TechProcessType.WorkUsers, null, null, null));

                return Errors.User.DuplicateUserName;
            }

            var user = PrepareUserForRegistrationCommand(command, _hashPasswordService, null);
            await _userRepository.Add(user);
            var token = _jwtTokenGenerator.GenerateToken(user!);

            _logger.Log(LogLevel.Information, $"RegistrationCommand: {user.UserName} ({user.Id.Value})");

            return new AuthenticationResult() { User = user, Token = token };
        }
        /// <summary>
        /// Метод подготовки данных о пользователе перед регистрацией
        /// </summary>
        public static User PrepareUserForRegistrationCommand(RegistrationCommand command, IHashPasswordService hashPasswordService, UserId? createdUser)
        {
            return User.Create(UserId.CreateUnique(), command.UserName, hashPasswordService.HashPassword(command.Password), command.FirstName, command.LastName, command.Email ?? "", command.Phone ?? ""
                , new EntityChangeInfo(DateTime.UtcNow, createdUser));
        }
    }
}

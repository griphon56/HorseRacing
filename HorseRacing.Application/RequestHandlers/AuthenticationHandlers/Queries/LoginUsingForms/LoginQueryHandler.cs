using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Authentication;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.UserAggregate;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Queries.LoginUsingForms
{
    /// <summary>
    /// Обработчика команды авторизации.
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;

        /// <summary>
        /// Конструктор для обработчика команды запроса на вход <see cref="LoginQueryHandler"/> .
        /// </summary>
        /// <param name="JwtTokenGenerator">Генератор токенов jwt.</param>
        /// <param name="userRepository">Пользовательский репозиторий.</param>
        /// <param name="hashPasswordService">Сервис хэширования паролей</param>
        public LoginQueryHandler(IJwtTokenGenerator JwtTokenGenerator, IUserRepository userRepository
            , IHashPasswordService hashPasswordService)
        {
            _jwtTokenGenerator = JwtTokenGenerator;
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByUserName(query.UserName) is not User user)
            {
                return Errors.Authentication.NotFoundUser;
            }
            if (!_hashPasswordService.VerifyPassword(query.Password, user.Password))
            {
                return Errors.Authentication.PasswordIncorrect;
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult()
            {
                User = user,
                Token = token
            };
        }
    }
}

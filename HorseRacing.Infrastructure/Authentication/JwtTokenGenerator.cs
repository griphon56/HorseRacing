using HorseRacing.Application.Common.Interfaces.Authentication;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using HorseRacing.Domain.UserAggregate;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HorseRacing.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly Application.Common.Interfaces.Services.IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(Application.Common.Interfaces.Services.IDateTimeProvider dateTimeProvider, IOptions<JwtAuthenticationModuleOption> jwtSettings)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtSettings.Value.Settings;
        }

        /// <summary>
        /// Метод генерации токена jwt
        /// </summary>
        public string GenerateToken(User user)
        {
            return BaseGenerateToken(user, _jwtSettings, _dateTimeProvider);
        }
        /// <summary>
        /// Базовый метод генерации jwt токена
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="settings">Настройки.</param>
        /// <param name="_dateTimeProvider">Поставщик даты и времени.</param>
        public static string BaseGenerateToken(User user, JwtSettings settings
            , Application.Common.Interfaces.Services.IDateTimeProvider _dateTimeProvider)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret))
                , SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.Value.ToString()),
            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                issuer: settings.Issuer,
                audience: settings.Audience,
                expires: _dateTimeProvider.UtcNow.AddDays(settings.ExpiryDays),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
